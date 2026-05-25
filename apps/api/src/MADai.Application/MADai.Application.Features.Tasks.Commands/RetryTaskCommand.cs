using System;
using MediatR;

namespace MADai.Application.Features.Tasks.Commands;

public sealed record RetryTaskCommand(Guid TaskId, bool ForceFromDeadLetter) : IRequest<Unit>, IBaseRequest;
