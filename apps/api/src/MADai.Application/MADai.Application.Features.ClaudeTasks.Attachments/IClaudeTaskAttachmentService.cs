using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MADai.Shared.Contracts;

namespace MADai.Application.Features.ClaudeTasks.Attachments;

public interface IClaudeTaskAttachmentService
{
	Task<ClaudeTaskAttachmentDto> UploadAsync(Guid taskId, string fileName, string contentType, long sizeBytes, Stream content, CancellationToken cancellationToken = default(CancellationToken));

	Task DeleteAsync(Guid taskId, string fileName, CancellationToken cancellationToken = default(CancellationToken));
}
