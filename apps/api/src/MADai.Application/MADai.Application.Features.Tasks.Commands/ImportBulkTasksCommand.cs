using System.Collections.Generic;
using MediatR;

namespace MADai.Application.Features.Tasks.Commands;

public sealed record ImportBulkTasksCommand(IReadOnlyList<TaskBulkImportItem> Items) : IRequest<ImportBulkTasksResult>, IBaseRequest;
