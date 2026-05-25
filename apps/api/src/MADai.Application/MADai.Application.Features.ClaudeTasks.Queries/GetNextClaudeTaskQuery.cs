using MADai.Shared.Contracts;
using MediatR;

namespace MADai.Application.Features.ClaudeTasks.Queries;

public sealed record GetNextClaudeTaskQuery : IRequest<ClaudeTaskDetailDto?>, IBaseRequest;
