using System;
using MADai.Domain.Common;

namespace MADai.Domain.Tasks;

public class TaskArtifact : AuditableEntity
{
	public Guid TaskId { get; set; }

	public TaskItem? Task { get; set; }

	public string FileName { get; set; } = string.Empty;


	public string ContentType { get; set; } = "application/octet-stream";


	public long SizeBytes { get; set; }

	public string StoragePath { get; set; } = string.Empty;


	public string StorageProvider { get; set; } = "Local";


	public string? Checksum { get; set; }

	public bool IsFinal { get; set; }

	public string? ThumbnailPath { get; set; }

	public string? PreviewUrl { get; set; }

	public string? Kind { get; set; }

	public int Version { get; set; } = 1;

}
