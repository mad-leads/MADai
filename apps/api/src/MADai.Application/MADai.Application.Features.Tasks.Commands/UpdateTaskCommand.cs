using System;
using MADai.Shared.Contracts;
using MediatR;

namespace MADai.Application.Features.Tasks.Commands;

public sealed record UpdateTaskCommand(Guid TaskId, UpdateTaskRequest Request) : IRequest<TaskDetailDto>, IBaseRequest;
