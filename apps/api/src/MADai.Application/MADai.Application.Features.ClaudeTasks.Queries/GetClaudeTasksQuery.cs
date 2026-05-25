using MADai.Shared.Contracts;
using MediatR;

namespace MADai.Application.Features.ClaudeTasks.Queries;

public sealed record GetClaudeTasksQuery(ClaudeTaskQueryRequest Request) : IRequest<PagedResult<ClaudeTaskSummaryDto>>, IBaseRequest;
