using System.Collections.Generic;
using MADai.Domain.Enums;

namespace MADai.Shared.Contracts;

public sealed record TaskClaimRequest(int MaxTasks, IReadOnlyList<string> Queues, IReadOnlyList<WorkerCapability> Capabilities);
