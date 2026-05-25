using System;
using System.Collections.Generic;

namespace MADai.Application.Features.Tasks.Commands;

public sealed record ImportBulkTasksResult(int Created, int Skipped, IReadOnlyList<Guid> CreatedIds);
