using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MADai.Application.Abstractions;

public interface IFileStorage
{
	Task<StoredFileInfo> SaveAsync(string companySlug, string folder, string fileName, Stream content, string contentType, CancellationToken cancellationToken = default(CancellationToken));

	Task<Stream> OpenReadAsync(string storagePath, CancellationToken cancellationToken = default(CancellationToken));

	Task<bool> DeleteAsync(string storagePath, CancellationToken cancellationToken = default(CancellationToken));

	Task<string> GetPublicUrlAsync(string storagePath, TimeSpan expiry, CancellationToken cancellationToken = default(CancellationToken));
}
