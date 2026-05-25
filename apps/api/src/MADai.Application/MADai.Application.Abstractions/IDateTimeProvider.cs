using System;

namespace MADai.Application.Abstractions;

public interface IDateTimeProvider
{
	DateTime UtcNow { get; }

	DateTimeOffset NowOffset { get; }
}
