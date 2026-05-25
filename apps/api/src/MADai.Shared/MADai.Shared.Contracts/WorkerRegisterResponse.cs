using System;

namespace MADai.Shared.Contracts;

public sealed record WorkerRegisterResponse(Guid WorkerId, string ApiKey, string AssignedQueue);
