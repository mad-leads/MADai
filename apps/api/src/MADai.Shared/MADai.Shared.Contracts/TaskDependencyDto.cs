using System;

namespace MADai.Shared.Contracts;

public sealed record TaskDependencyDto(Guid Id, Guid DependsOnTaskId, string? DependsOnTitle, string DependencyType);
