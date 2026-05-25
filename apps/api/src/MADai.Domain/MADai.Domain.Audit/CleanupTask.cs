using System;
using MADai.Domain.Common;

namespace MADai.Domain.Audit;

public class CleanupTask : AuditableEntity
{
	public string Target { get; set; } = string.Empty;


	public long ReclaimableBytes { get; set; }

	public int ItemCount { get; set; }

	public string Status { get; set; } = "Pending";


	public DateTime? ExecutedAt { get; set; }

	public string? Result { get; set; }
}
