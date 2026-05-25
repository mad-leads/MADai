using System;
using MADai.Shared.Contracts;
using MediatR;

namespace MADai.Application.Features.Tasks.Queries;

public sealed record GetTaskByIdQuery(Guid Id) : IRequest<TaskDetailDto>, IBaseRequest;
