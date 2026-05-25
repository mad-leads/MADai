using System;
using System.Collections.Generic;
using MADai.Domain.Common;

namespace MADai.Domain.Files;

public class FileFolder : TenantEntity
{
	public string Name { get; set; } = string.Empty;


	public Guid? ParentFolderId { get; set; }

	public FileFolder? ParentFolder { get; set; }

	public ICollection<FileFolder> Children { get; set; } = new List<FileFolder>();


	public ICollection<FileItem> Files { get; set; } = new List<FileItem>();


	public string? Path { get; set; }
}
