using System;

namespace MADai.Shared.Contracts;

public sealed record TaskLogEntry(DateTime Timestamp, string Level, string Message, string? Source);
