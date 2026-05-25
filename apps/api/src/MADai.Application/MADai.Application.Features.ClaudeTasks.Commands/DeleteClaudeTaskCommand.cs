using System;
using MediatR;

namespace MADai.Application.Features.ClaudeTasks.Commands;

public sealed record DeleteClaudeTaskCommand(Guid Id) : IRequest<Unit>, IBaseRequest;
