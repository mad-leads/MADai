using System;
using MADai.Shared.Contracts;
using MediatR;

namespace MADai.Application.Features.ClaudeTasks.Queries;

public sealed record GetClaudeTaskByIdQuery(Guid Id) : IRequest<ClaudeTaskDetailDto>, IBaseRequest;
