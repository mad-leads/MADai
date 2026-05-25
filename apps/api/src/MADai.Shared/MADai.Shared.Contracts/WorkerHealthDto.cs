using System;

namespace MADai.Shared.Contracts;

public sealed record WorkerHealthDto(Guid Id, string Name, string Status, int ActiveTasks, int MaxConcurrency, DateTime? LastHeartbeatAt, double CpuPercent, double MemoryMb, double DiskFreeGb);
