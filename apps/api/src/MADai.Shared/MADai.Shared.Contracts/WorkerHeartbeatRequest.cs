using MADai.Domain.Enums;

namespace MADai.Shared.Contracts;

public sealed record WorkerHeartbeatRequest(WorkerStatus Status, int ActiveTasks, double CpuPercent, double MemoryMb, double DiskFreeGb, string? Notes);
