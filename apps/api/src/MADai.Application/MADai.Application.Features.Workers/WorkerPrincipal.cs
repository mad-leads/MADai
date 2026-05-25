using System;

namespace MADai.Application.Features.Workers;

public record WorkerPrincipal(Guid WorkerId, Guid? CompanyId, string Name);
