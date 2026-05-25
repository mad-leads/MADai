namespace MADai.Infrastructure.Options;

public class JwtOptions
{
	public const string SectionName = "Jwt";

	public string Secret { get; set; } = string.Empty;


	public string Issuer { get; set; } = "MADai";


	public string Audience { get; set; } = "MADai-Clients";


	public int AccessTokenLifetimeMinutes { get; set; } = 60;


	public int RefreshTokenLifetimeDays { get; set; } = 30;

}
