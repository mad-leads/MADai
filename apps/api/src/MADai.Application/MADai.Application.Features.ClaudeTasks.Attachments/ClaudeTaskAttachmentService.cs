using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Application.Features.ClaudeTasks.Queries;
using MADai.Domain.Tasks;
using MADai.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.ClaudeTasks.Attachments;

public class ClaudeTaskAttachmentService : IClaudeTaskAttachmentService
{
	private static readonly HashSet<string> AllowedContentTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "image/jpeg", "image/png", "image/webp", "image/gif", "application/pdf", "text/plain", "text/csv", "application/json" };

	private const long MaxFileSize = 10485760L;

	private const int MaxAttachmentsPerTask = 10;

	private readonly IDbContextAccess _db;

	private readonly IFileStorage _storage;

	private readonly IEventPublisher _publisher;

	public ClaudeTaskAttachmentService(IDbContextAccess db, IFileStorage storage, IEventPublisher publisher)
	{
		_db = db;
		_storage = storage;
		_publisher = publisher;
	}

	public async Task<ClaudeTaskAttachmentDto> UploadAsync(Guid taskId, string fileName, string contentType, long sizeBytes, Stream content, CancellationToken cancellationToken = default(CancellationToken))
	{
		string fileName2 = fileName;
		if (sizeBytes > 10485760)
		{
			throw new AppException($"File exceeds {10} MB limit.");
		}
		if (!AllowedContentTypes.Contains(contentType))
		{
			throw new AppException("Content type '" + contentType + "' not in allow-list (jpeg, png, webp, gif, pdf, plain, csv, json).");
		}
		ClaudeTask task = (await _db.ClaudeTasks.FirstOrDefaultAsync((ClaudeTask t) => t.Id == taskId, cancellationToken)) ?? throw new NotFoundException("ClaudeTask", taskId);
		List<ClaudeTaskAttachmentDto> existing = ClaudeTaskAttachmentsParser.Parse(task.AttachmentsJson).ToList();
		if (existing.Count >= 10)
		{
			throw new ConflictException($"Task already has the maximum {10} attachments.");
		}
		if (existing.Any((ClaudeTaskAttachmentDto a) => string.Equals(a.FileName, fileName2, StringComparison.OrdinalIgnoreCase)))
		{
			throw new ConflictException("Attachment '" + fileName2 + "' already exists on this task. Delete it first to replace.");
		}
		StoredFileInfo stored = await _storage.SaveAsync("claude-tasks", taskId.ToString("N"), fileName2, content, contentType, cancellationToken);
		ClaudeTaskAttachmentDto attachment = new ClaudeTaskAttachmentDto(fileName2, stored.StoragePath, contentType, stored.SizeBytes);
		existing.Add(attachment);
		task.AttachmentsJson = ClaudeTaskAttachmentsParser.Serialize(existing);
		await _db.SaveChangesAsync(cancellationToken);
		await _publisher.PublishClaudeTaskUpdatedAsync(taskId, new
		{
			type = "attachment-added",
			taskId = taskId,
			fileName = fileName2,
			contentType = contentType,
			sizeBytes = stored.SizeBytes
		}, cancellationToken);
		return attachment;
	}

	public async Task DeleteAsync(Guid taskId, string fileName, CancellationToken cancellationToken = default(CancellationToken))
	{
		string fileName2 = fileName;
		ClaudeTask task = (await _db.ClaudeTasks.FirstOrDefaultAsync((ClaudeTask t) => t.Id == taskId, cancellationToken)) ?? throw new NotFoundException("ClaudeTask", taskId);
		List<ClaudeTaskAttachmentDto> existing = ClaudeTaskAttachmentsParser.Parse(task.AttachmentsJson).ToList();
		ClaudeTaskAttachmentDto match = existing.FirstOrDefault((ClaudeTaskAttachmentDto a) => string.Equals(a.FileName, fileName2, StringComparison.OrdinalIgnoreCase)) ?? throw new NotFoundException("ClaudeTaskAttachment", fileName2);
		await _storage.DeleteAsync(match.StoragePath, cancellationToken);
		existing.Remove(match);
		task.AttachmentsJson = ((existing.Count > 0) ? ClaudeTaskAttachmentsParser.Serialize(existing) : null);
		await _db.SaveChangesAsync(cancellationToken);
		await _publisher.PublishClaudeTaskUpdatedAsync(taskId, new
		{
			type = "attachment-removed",
			taskId = taskId,
			fileName = fileName2
		}, cancellationToken);
	}
}
