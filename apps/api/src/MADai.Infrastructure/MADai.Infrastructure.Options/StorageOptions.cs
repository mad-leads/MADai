namespace MADai.Infrastructure.Options;

public class StorageOptions
{
	public const string SectionName = "Storage";

	public string Provider { get; set; } = "Local";


	public string LocalRoot { get; set; } = "storage";


	public string? AzureConnectionString { get; set; }

	public string? AzureContainer { get; set; }

	public string? S3Bucket { get; set; }

	public string? S3Region { get; set; }

	public string? S3AccessKey { get; set; }

	public string? S3SecretKey { get; set; }
}
