using System;
using MADai.Shared.Contracts;
using MediatR;

namespace MADai.Application.Features.ClaudeTasks.Commands;

public sealed record UpdateClaudeTaskCommand(Guid Id, UpdateClaudeTaskRequest Request, bool Override) : IRequest<ClaudeTaskDetailDto>, IBaseRequest;
