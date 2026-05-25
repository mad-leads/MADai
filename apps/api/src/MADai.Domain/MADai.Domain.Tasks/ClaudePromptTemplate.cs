using MADai.Domain.Common;

namespace MADai.Domain.Tasks;

public class ClaudePromptTemplate : AuditableEntity
{
	public string Name { get; set; } = string.Empty;


	public string? Description { get; set; }

	public string Content { get; set; } = string.Empty;

}
