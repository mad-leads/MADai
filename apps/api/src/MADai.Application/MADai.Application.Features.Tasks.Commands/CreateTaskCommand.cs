using MADai.Shared.Contracts;
using MediatR;

namespace MADai.Application.Features.Tasks.Commands;

public sealed record CreateTaskCommand(CreateTaskRequest Request) : IRequest<TaskDetailDto>, IBaseRequest;
