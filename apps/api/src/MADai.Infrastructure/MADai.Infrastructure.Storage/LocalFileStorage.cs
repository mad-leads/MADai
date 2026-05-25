using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace MADai.Infrastructure.Storage;

public class LocalFileStorage : IFileStorage
{
	private readonly string _root;

	public LocalFileStorage(IOptions<StorageOptions> options)
	{
		_root = Path.GetFullPath(options.Value.LocalRoot);
		Directory.CreateDirectory(_root);
	}

	public async Task<StoredFileInfo> SaveAsync(string companySlug, string folder, string fileName, Stream content, string contentType, CancellationToken cancellationToken = default(CancellationToken))
	{
		string safeCompany = SafeSegment(companySlug);
		string safeFolder = SafeSegment(folder);
		string safeFile = SafeSegment(fileName);
		string directory = Path.Combine(_root, safeCompany, safeFolder);
		Directory.CreateDirectory(directory);
		string fullPath = Path.Combine(directory, safeFile);
		FileStream fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);
		StoredFileInfo result;
		try
		{
			await content.CopyToAsync(fileStream, cancellationToken);
			await fileStream.FlushAsync(cancellationToken);
			fileStream.Position = 0L;
			long size = new FileInfo(fullPath).Length;
			using SHA256 sha = SHA256.Create();
			FileStream read = File.OpenRead(fullPath);
			StoredFileInfo storedFileInfo;
			try
			{
				string hash = Convert.ToHexString(await sha.ComputeHashAsync(read, cancellationToken));
				string relative = Path.GetRelativePath(_root, fullPath).Replace('\\', '/');
				storedFileInfo = new StoredFileInfo("Local", relative, size, hash);
			}
			finally
			{
				if (read != null)
				{
					await read.DisposeAsync();
				}
			}
			result = storedFileInfo;
		}
		finally
		{
			if (fileStream != null)
			{
				await fileStream.DisposeAsync();
			}
		}
		return result;
	}

	public Task<Stream> OpenReadAsync(string storagePath, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.FromResult((Stream)new FileStream(Path.Combine(_root, storagePath.Replace('/', Path.DirectorySeparatorChar)), FileMode.Open, FileAccess.Read, FileShare.Read));
	}

	public Task<bool> DeleteAsync(string storagePath, CancellationToken cancellationToken = default(CancellationToken))
	{
		string full = Path.Combine(_root, storagePath.Replace('/', Path.DirectorySeparatorChar));
		if (!File.Exists(full))
		{
			return Task.FromResult(result: false);
		}
		File.Delete(full);
		return Task.FromResult(result: true);
	}

	public Task<string> GetPublicUrlAsync(string storagePath, TimeSpan expiry, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.FromResult("/api/v1/files/raw/" + Uri.EscapeDataString(storagePath));
	}

	private static string SafeSegment(string value)
	{
		string safe = new string(value.Select((char c) => (!Path.GetInvalidFileNameChars().Contains(c) && c != Path.DirectorySeparatorChar) ? c : '_').ToArray());
		if (!string.IsNullOrWhiteSpace(safe))
		{
			return safe;
		}
		return "default";
	}
}
