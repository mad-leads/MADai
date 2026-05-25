using System.Text.Json;
using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Application.Features.ClaudeTasks.Queries;
using MADai.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.ClaudeTasks.Attachments;

public interface IClaudeTaskAttachmentService
{
    Task<ClaudeTaskAttachmentDto> UploadAsync(Guid taskId, string fileName, string contentType, long sizeBytes, Stream content, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid taskId, string fileName, CancellationToken cancellationToken = default);
}

public class ClaudeTaskAttachmentService : IClaudeTaskAttachmentService
{
    private static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg", "image/png", "image/webp", "image/gif",
        "application/pdf", "text/plain", "text/csv", "application/json"
    };
    private const long MaxFileSize = 10 * 1024 * 1024;     // 10 MB
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

    public async Task<ClaudeTaskAttachmentDto> UploadAsync(Guid taskId, string fileName, string contentType, long sizeBytes, Stream content, CancellationToken cancellationToken = default)
    {
        if (sizeBytes > MaxFileSize) throw new AppException($"File exceeds {MaxFileSize / (1024 * 1024)} MB limit.");
        if (!AllowedContentTypes.Contains(contentType))
        {
            throw new AppException($"Content type '{contentType}' not in allow-list (jpeg, png, webp, gif, pdf, plain, csv, json).");
        }

        var task = await _db.ClaudeTasks.FirstOrDefaultAsync(t => t.Id == taskId, cancellationToken)
            ?? throw new NotFoundException("ClaudeTask", taskId);

        var existing = ClaudeTaskAttachmentsParser.Parse(task.AttachmentsJson).ToList();
        if (existing.Count >= MaxAttachmentsPerTask)
        {
            throw new ConflictException($"Task already has the maximum {MaxAttachmentsPerTask} attachments.");
        }
        if (existing.Any(a => string.Equals(a.FileName, fileName, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ConflictException($"Attachment '{fileName}' already exists on this task. Delete it first to replace.");
        }

        var stored = await _storage.SaveAsync("claude-tasks", taskId.ToString("N"), fileName, content, contentType, cancellationToken);
        var attachment = new ClaudeTaskAttachmentDto(fileName, stored.StoragePath, contentType, stored.SizeBytes);

        existing.Add(attachment);
        task.AttachmentsJson = ClaudeTaskAttachmentsParser.Serialize(existing);
        await _db.SaveChangesAsync(cancellationToken);

        await _publisher.PublishClaudeTaskUpdatedAsync(taskId,
            new { type = "attachment-added", taskId, fileName, contentType, sizeBytes = stored.SizeBytes },
            cancellationToken);
        return attachment;
    }

    public async Task DeleteAsync(Guid taskId, string fileName, CancellationToken cancellationToken = default)
    {
        var task = await _db.ClaudeTasks.FirstOrDefaultAsync(t => t.Id == taskId, cancellationToken)
            ?? throw new NotFoundException("ClaudeTask", taskId);

        var existing = ClaudeTaskAttachmentsParser.Parse(task.AttachmentsJson).ToList();
        var match = existing.FirstOrDefault(a => string.Equals(a.FileName, fileName, StringComparison.OrdinalIgnoreCase))
            ?? throw new NotFoundException("ClaudeTaskAttachment", fileName);

        await _storage.DeleteAsync(match.StoragePath, cancellationToken);
        existing.Remove(match);
        task.AttachmentsJson = existing.Count > 0 ? ClaudeTaskAttachmentsParser.Serialize(existing) : null;
        await _db.SaveChangesAsync(cancellationToken);

        await _publisher.PublishClaudeTaskUpdatedAsync(taskId,
            new { type = "attachment-removed", taskId, fileName },
            cancellationToken);
    }
}
