using System;

namespace MADai.Shared.Contracts;

public sealed record FailureTrendPointDto(DateTime Bucket, long Failures);
