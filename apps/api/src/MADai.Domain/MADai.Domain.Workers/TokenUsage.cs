using System;
using MADai.Domain.Common;

namespace MADai.Domain.Workers;

public class TokenUsage : Entity
{
	public string SessionId { get; set; } = string.Empty;


	public string RepositoryKey { get; set; } = string.Empty;


	public Guid? TaskId { get; set; }

	public DateTime Timestamp { get; set; } = DateTime.UtcNow;


	public int PromptTokens { get; set; }

	public int CompletionTokens { get; set; }

	public int TotalTokens { get; set; }
}
