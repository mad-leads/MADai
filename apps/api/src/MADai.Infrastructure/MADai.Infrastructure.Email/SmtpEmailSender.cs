using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Infrastructure.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MADai.Infrastructure.Email;

public class SmtpEmailSender : IEmailSender
{
	private readonly SmtpOptions _opts;

	private readonly ILogger<SmtpEmailSender> _logger;

	public SmtpEmailSender(IOptions<SmtpOptions> opts, ILogger<SmtpEmailSender> logger)
	{
		_opts = opts.Value;
		_logger = logger;
	}

	public async Task SendAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (string.IsNullOrWhiteSpace(_opts.Host) || string.IsNullOrWhiteSpace(_opts.FromAddress))
		{
			_logger.LogWarning("SMTP not configured (Host or FromAddress missing). Email to {To} subject {Subject} not sent.", to, subject);
			return;
		}
		MimeMessage msg = new MimeMessage
		{
			From = { (InternetAddress)new MailboxAddress(_opts.FromName, _opts.FromAddress) },
			To = { (InternetAddress)MailboxAddress.Parse(to) },
			Subject = subject,
			Body = new BodyBuilder
			{
				HtmlBody = htmlBody
			}.ToMessageBody()
		};
		using SmtpClient client = new SmtpClient();
		try
		{
			SecureSocketOptions securityOption = (_opts.Secure ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls);
			await client.ConnectAsync(_opts.Host, _opts.Port, securityOption, cancellationToken);
			if (!string.IsNullOrEmpty(_opts.Username) && !string.IsNullOrEmpty(_opts.Password))
			{
				await client.AuthenticateAsync(_opts.Username, _opts.Password, cancellationToken);
			}
			await client.SendAsync(msg, cancellationToken);
			_logger.LogInformation("Sent email to {To} subject {Subject}", to, subject);
		}
		finally
		{
			if (client.IsConnected)
			{
				await client.DisconnectAsync(quit: true, cancellationToken);
			}
		}
	}
}
