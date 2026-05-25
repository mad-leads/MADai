using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace MADai.Infrastructure.Email;

public class NullEmailSender : IEmailSender
{
	private readonly ILogger<NullEmailSender> _logger;

	public NullEmailSender(ILogger<NullEmailSender> logger)
	{
		_logger = logger;
	}

	public Task SendAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken = default(CancellationToken))
	{
		_logger.LogInformation("[Email] To={To} Subject={Subject}", to, subject);
		return Task.CompletedTask;
	}
}
