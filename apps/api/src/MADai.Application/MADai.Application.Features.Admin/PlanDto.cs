using System;

namespace MADai.Application.Features.Admin;

public sealed record PlanDto(Guid Id, string Name, string Code, string? Description, decimal MonthlyPrice, decimal AnnualPrice, string Currency, int IncludedTasks, int IncludedStorageGb, int IncludedWorkers, bool IsPublic, bool IsActive);
