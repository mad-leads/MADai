using System;
using MADai.Domain.Common;
using MADai.Domain.Enums;

namespace MADai.Domain.Audit;

public class AuditRecommendation : AuditableEntity
{
	public Guid? AuditRunId { get; set; }

	public AuditRun? AuditRun { get; set; }

	public string Title { get; set; } = string.Empty;


	public string Body { get; set; } = string.Empty;


	public AuditFindingSeverity Severity { get; set; }

	public string Status { get; set; } = "Open";


	public Guid? GeneratedTaskId { get; set; }
}
