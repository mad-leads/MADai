using System;
using MADai.Domain.Common;
using MADai.Domain.Enums;
using MADai.Domain.Tasks;

namespace MADai.Domain.Audit;

public class AuditFinding : AuditableEntity
{
	public Guid AuditRunId { get; set; }

	public AuditRun? AuditRun { get; set; }

	public AuditScanType ScanType { get; set; }

	public AuditFindingSeverity Severity { get; set; }

	public AuditFindingStatus Status { get; set; }

	public string Title { get; set; } = string.Empty;


	public string? Details { get; set; }

	public string? AffectedResource { get; set; }

	public string? RecommendedAction { get; set; }

	public Guid? RelatedTaskId { get; set; }

	public TaskItem? RelatedTask { get; set; }

	public Guid? GeneratedTaskId { get; set; }

	public TaskItem? GeneratedTask { get; set; }
}
