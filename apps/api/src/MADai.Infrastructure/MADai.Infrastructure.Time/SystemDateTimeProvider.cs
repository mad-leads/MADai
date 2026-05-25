using System;
using MADai.Application.Abstractions;

namespace MADai.Infrastructure.Time;

public class SystemDateTimeProvider : IDateTimeProvider
{
	public DateTime UtcNow => DateTime.UtcNow;

	public DateTimeOffset NowOffset => DateTimeOffset.UtcNow;
}
