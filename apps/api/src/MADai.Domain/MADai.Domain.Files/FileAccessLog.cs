using System;
using MADai.Domain.Common;
using MADai.Domain.Identity;

namespace MADai.Domain.Files;

public class FileAccessLog : Entity
{
	public Guid FileId { get; set; }

	public FileItem? File { get; set; }

	public Guid? UserId { get; set; }

	public ApplicationUser? User { get; set; }

	public DateTime AccessedAt { get; set; } = DateTime.UtcNow;


	public string Action { get; set; } = "Read";


	public string? IpAddress { get; set; }
}
