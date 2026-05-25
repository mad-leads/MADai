using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using MADai.Application.Abstractions;
using MADai.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace MADai.Infrastructure.Storage;

public class S3FileStorage : IFileStorage
{
	private readonly StorageOptions _options;

	private readonly IAmazonS3 _client;

	public S3FileStorage(IOptions<StorageOptions> options)
	{
		_options = options.Value;
		if (string.IsNullOrWhiteSpace(_options.S3Bucket))
		{
			throw new InvalidOperationException("Storage:S3Bucket not configured.");
		}
		RegionEndpoint region = ((!string.IsNullOrWhiteSpace(_options.S3Region)) ? RegionEndpoint.GetBySystemName(_options.S3Region) : RegionEndpoint.USEast1);
		if (!string.IsNullOrWhiteSpace(_options.S3AccessKey) && !string.IsNullOrWhiteSpace(_options.S3SecretKey))
		{
			_client = new AmazonS3Client(_options.S3AccessKey, _options.S3SecretKey, region);
		}
		else
		{
			_client = new AmazonS3Client(region);
		}
	}

	public async Task<StoredFileInfo> SaveAsync(string companySlug, string folder, string fileName, Stream content, string contentType, CancellationToken cancellationToken = default(CancellationToken))
	{
		string key = BuildKey(companySlug, folder, fileName);
		PutObjectRequest put = new PutObjectRequest
		{
			BucketName = _options.S3Bucket,
			Key = key,
			ContentType = contentType,
			InputStream = content,
			AutoCloseStream = false
		};
		await _client.PutObjectAsync(put, cancellationToken);
		content.Position = 0L;
		string hash = await ComputeSha256(content, cancellationToken);
		long size = (content.CanSeek ? content.Length : 0);
		return new StoredFileInfo("S3", key, size, hash);
	}

	public async Task<Stream> OpenReadAsync(string storagePath, CancellationToken cancellationToken = default(CancellationToken))
	{
		GetObjectRequest get = new GetObjectRequest
		{
			BucketName = _options.S3Bucket,
			Key = storagePath
		};
		GetObjectResponse obj = await _client.GetObjectAsync(get, cancellationToken);
		MemoryStream ms = new MemoryStream();
		await obj.ResponseStream.CopyToAsync(ms, cancellationToken);
		ms.Position = 0L;
		return ms;
	}

	public async Task<bool> DeleteAsync(string storagePath, CancellationToken cancellationToken = default(CancellationToken))
	{
		try
		{
			await _client.DeleteObjectAsync(_options.S3Bucket, storagePath, cancellationToken);
			return true;
		}
		catch (AmazonS3Exception)
		{
			return false;
		}
	}

	public Task<string> GetPublicUrlAsync(string storagePath, TimeSpan expiry, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.FromResult(_client.GetPreSignedURL(new GetPreSignedUrlRequest
		{
			BucketName = _options.S3Bucket,
			Key = storagePath,
			Verb = HttpVerb.GET,
			Expires = DateTime.UtcNow.Add(expiry)
		}));
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
