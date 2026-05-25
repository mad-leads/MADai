using System;
using MADai.Domain.Common;

namespace MADai.Domain.Tasks;

public class TaskRecommendation : TenantEntity
{
	public Guid? TaskId { get; set; }

	public TaskItem? Task { get; set; }

	public string Title { get; set; } = string.Empty;


	public string Body { get; set; } = string.Empty;


	public string Source { get; set; } = "ai";


	public string Status { get; set; } = "Open";


	public double Confidence { get; set; }

	public DateTime? AppliedAt { get; set; }
}
