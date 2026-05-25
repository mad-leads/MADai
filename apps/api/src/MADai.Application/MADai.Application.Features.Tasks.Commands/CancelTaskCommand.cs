using System;
using MediatR;

namespace MADai.Application.Features.Tasks.Commands;

public sealed record CancelTaskCommand(Guid TaskId, string? Reason) : IRequest<Unit>, IBaseRequest;
