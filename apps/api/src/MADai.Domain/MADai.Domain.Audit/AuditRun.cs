using System;
using System.Collections.Generic;
using MADai.Domain.Common;
using MADai.Domain.Enums;

namespace MADai.Domain.Audit;

public class AuditRun : AuditableEntity
{
	public DateTime StartedAt { get; set; } = DateTime.UtcNow;


	public DateTime? CompletedAt { get; set; }

	public AuditScanType ScanType { get; set; }

	public int FindingsCount { get; set; }

	public int RecommendationsCount { get; set; }

	public string? Summary { get; set; }

	public string Status { get; set; } = "Running";


	public ICollection<AuditFinding> Findings { get; set; } = new List<AuditFinding>();

}
