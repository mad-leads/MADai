using System;

namespace MADai.Shared.Contracts;

public sealed record ClaudePromptTemplateDto(Guid Id, string Name, string? Description, string Content, DateTime CreatedDate, DateTime? ModifiedDate);
