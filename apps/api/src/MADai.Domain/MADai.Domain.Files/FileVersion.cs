using System;
using MADai.Domain.Common;

namespace MADai.Domain.Files;

public class FileVersion : Entity
{
	public Guid FileId { get; set; }

	public FileItem? File { get; set; }

	public int Version { get; set; }

	public string StoragePath { get; set; } = string.Empty;


	public long SizeBytes { get; set; }

	public string? Checksum { get; set; }

	public Guid? CreatedByUserId { get; set; }

	public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

}
