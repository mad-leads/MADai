using System;
using MADai.Domain.Common;

namespace MADai.Domain.Workers;

public class ProcessHealth : Entity
{
	public string ProcessKey { get; set; } = string.Empty;


	public string ProcessName { get; set; } = string.Empty;


	public int? ProcessId { get; set; }

	public string Status { get; set; } = "Unknown";


	public double MemoryMb { get; set; }

	public DateTime CheckedAt { get; set; } = DateTime.UtcNow;


	public string? Details { get; set; }
}
