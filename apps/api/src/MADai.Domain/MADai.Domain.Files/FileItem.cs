using System;
using System.Collections.Generic;
using MADai.Domain.Common;

namespace MADai.Domain.Files;

public class FileItem : TenantEntity
{
	public string Name { get; set; } = string.Empty;


	public string ContentType { get; set; } = "application/octet-stream";


	public long SizeBytes { get; set; }

	public string StoragePath { get; set; } = string.Empty;


	public string StorageProvider { get; set; } = "Local";


	public string? Checksum { get; set; }

	public Guid? FolderId { get; set; }

	public FileFolder? Folder { get; set; }

	public string? ThumbnailPath { get; set; }

	public string? PreviewPath { get; set; }

	public int Version { get; set; } = 1;


	public ICollection<FileVersion> Versions { get; set; } = new List<FileVersion>();


	public ICollection<FileAccessLog> AccessLogs { get; set; } = new List<FileAccessLog>();

}
