using MADai.Shared.Contracts;
using MediatR;

namespace MADai.Application.Features.Tasks.Queries;

public sealed record GetTasksQuery(TaskQueryRequest Request) : IRequest<PagedResult<TaskSummaryDto>>, IBaseRequest;
