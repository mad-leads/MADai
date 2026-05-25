using System;
using MADai.Domain.Common;

namespace MADai.Domain.Workers;

public class ProcessRestart : Entity
{
	public string ProcessKey { get; set; } = string.Empty;


	public DateTime RestartedAt { get; set; } = DateTime.UtcNow;


	public string Reason { get; set; } = string.Empty;


	public bool Succeeded { get; set; }

	public string? Details { get; set; }
}
