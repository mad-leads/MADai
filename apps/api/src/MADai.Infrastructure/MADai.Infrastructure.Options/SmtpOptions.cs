namespace MADai.Infrastructure.Options;

public class SmtpOptions
{
	public const string SectionName = "Smtp";

	public string? Host { get; set; }

	public int Port { get; set; } = 587;


	public bool Secure { get; set; } = true;


	public string? Username { get; set; }

	public string? Password { get; set; }

	public string FromName { get; set; } = "MADai";


	public string? FromAddress { get; set; }
}
