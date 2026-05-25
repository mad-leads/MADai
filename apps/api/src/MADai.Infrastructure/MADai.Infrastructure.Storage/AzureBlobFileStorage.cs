using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using MADai.Application.Abstractions;
using MADai.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace MADai.Infrastructure.Storage;

public class AzureBlobFileStorage : IFileStorage
{
	private readonly StorageOptions _options;

	private readonly BlobContainerClient _container;

	public AzureBlobFileStorage(IOptions<StorageOptions> options)
	{
		_options = options.Value;
		if (string.IsNullOrWhiteSpace(_options.AzureConnectionString))
		{
			throw new InvalidOperationException("Storage:AzureConnectionString not configured.");
		}
		if (string.IsNullOrWhiteSpace(_options.AzureContainer))
		{
			throw new InvalidOperationException("Storage:AzureContainer not configured.");
		}
		_container = new BlobContainerClient(_options.AzureConnectionString, _options.AzureContainer);
		_container.CreateIfNotExists();
	}

	public async Task<StoredFileInfo> SaveAsync(string companySlug, string folder, string fileName, Stream content, string contentType, CancellationToken cancellationToken = default(CancellationToken))
	{
		string key = BuildKey(companySlug, folder, fileName);
		BlobClient blob = _container.GetBlobClient(key);
		await blob.UploadAsync(content, new BlobUploadOptions
		{
			HttpHeaders = new BlobHttpHeaders
			{
				ContentType = contentType
			}
		}, cancellationToken);
		content.Position = 0L;
		return new StoredFileInfo(Checksum: await ComputeSha256(content, cancellationToken), StorageProvider: "AzureBlob", StoragePath: key, SizeBytes: (await blob.GetPropertiesAsync(null, cancellationToken)).Value.ContentLength);
	}

	public async Task<Stream> OpenReadAsync(string storagePath, CancellationToken cancellationToken = default(CancellationToken))
	{
		return (await _container.GetBlobClient(storagePath).DownloadStreamingAsync(null, cancellationToken)).Value.Content;
	}

	public async Task<bool> DeleteAsync(string storagePath, CancellationToken cancellationToken = default(CancellationToken))
	{
		return (await _container.GetBlobClient(storagePath).DeleteIfExistsAsync(DeleteSnapshotsOption.None, null, cancellationToken)).Value;
	}

	public Task<string> GetPublicUrlAsync(string storagePath, TimeSpan expiry, CancellationToken cancellationToken = default(CancellationToken))
	{
		BlobClient blob = _container.GetBlobClient(storagePath);
		if (!blob.CanGenerateSasUri)
		{
			return Task.FromResult("/api/v1/files/raw/" + Uri.EscapeDataString(storagePath));
		}
		BlobSasBuilder sas = new BlobSasBuilder
		{
			BlobContainerName = _container.Name,
			BlobName = storagePath,
			Resource = "b",
			ExpiresOn = DateTimeOffset.UtcNow.Add(expiry)
		};
		sas.SetPermissions(BlobSasPermissions.Read);
		return Task.FromResult(blob.GenerateSasUri(sas).ToString());
	}

	private static string BuildKey(string companySlug, string folder, string fileName)
	{
		return $"{Safe(companySlug)}/{Safe(folder)}/{Safe(fileName)}";
		static string Safe(string s)
		{
			return new string(s.Select((char c) => (!char.IsLetterOrDigit(c) && c != '-' && c != '_' && c != '.') ? '_' : c).ToArray());
		}
	}

	private static async Task<string> ComputeSha256(Stream content, CancellationToken cancellationToken)
	{
		if (content.CanSeek)
		{
			content.Position = 0L;
		}
		using SHA256 sha = SHA256.Create();
		return Convert.ToHexString(await sha.ComputeHashAsync(content, cancellationToken));
	}
}
