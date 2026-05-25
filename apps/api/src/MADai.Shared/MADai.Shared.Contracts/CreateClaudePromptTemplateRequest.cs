namespace MADai.Shared.Contracts;

public sealed record CreateClaudePromptTemplateRequest(string Name, string? Description, string Content);
