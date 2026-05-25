using MADai.Shared.Contracts;
using MediatR;

namespace MADai.Application.Features.ClaudeTasks.Commands;

public sealed record CreateClaudeTaskCommand(CreateClaudeTaskRequest Request) : IRequest<ClaudeTaskDetailDto>, IBaseRequest;
