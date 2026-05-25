using System;

namespace MADai.Application.Features.PersistentWorkers;

public sealed record NativeProcessDto(string ProcessKey, string ProcessName, int? ProcessId, string Status, double MemoryMb, DateTime CheckedAt, string? Details);
