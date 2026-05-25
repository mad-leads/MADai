using System;
using System.Collections.Generic;
using MADai.Domain.Enums;

namespace MADai.Shared.Contracts;

public sealed record WorkerRegisterRequest(string Name, string MachineName, string? AgentVersion, string? OperatingSystem, string? IpAddress, int MaxConcurrency, IReadOnlyList<WorkerCapability> Capabilities, IReadOnlyList<string>? Queues, string? Labels, string? WorkspaceRoot, Guid? CompanyId);
