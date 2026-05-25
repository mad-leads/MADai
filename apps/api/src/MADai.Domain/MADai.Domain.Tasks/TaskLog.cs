using System;
using MADai.Domain.Common;

namespace MADai.Domain.Tasks;

public class TaskLog : Entity
{
	public Guid TaskId { get; set; }

	public TaskItem? Task { get; set; }

	public DateTime Timestamp { get; set; } = DateTime.UtcNow;


	public string Level { get; set; } = "Info";


	public string Message { get; set; } = string.Empty;


	public string? Source { get; set; }

	public string? ContextJson { get; set; }
}
