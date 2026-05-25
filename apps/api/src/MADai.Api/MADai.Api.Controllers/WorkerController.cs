using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MADai.Application.Features.Workers;
using MADai.Shared.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MADai.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/worker")]
[ApiVersion("1.0")]
public class WorkerController : ControllerBase
{
	private readonly IWorkerRegistrationService _registration;

	private readonly IWorkerQueueService _queue;

	public WorkerController(IWorkerRegistrationService registration, IWorkerQueueService queue)
	{
		_registration = registration;
		_queue = queue;
	}

	[HttpPost("register")]
	[AllowAnonymous]
	public async Task<ActionResult<ApiResult<WorkerRegisterResponse>>> Register([FromBody] WorkerRegisterRequest request, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<WorkerRegisterResponse>.Ok(await _registration.RegisterAsync(request, cancellationToken)));
	}

	[HttpPost("heartbeat")]
	[Authorize(AuthenticationSchemes = "Worker", Policy = "WorkerOnly")]
	public async Task<ActionResult<ApiResult>> Heartbeat([FromBody] WorkerHeartbeatRequest request, CancellationToken cancellationToken)
	{
		Guid workerId = GetWorkerId();
		await _queue.HeartbeatAsync(workerId, request, cancellationToken);
		return Ok(ApiResult.Ok());
	}

	[HttpPost("claim")]
	[Authorize(AuthenticationSchemes = "Worker", Policy = "WorkerOnly")]
	public async Task<ActionResult<ApiResult<IReadOnlyList<TaskClaimResponseItem>>>> Claim([FromBody] TaskClaimRequest request, CancellationToken cancellationToken)
	{
		Guid workerId = GetWorkerId();
		return Ok(ApiResult<IReadOnlyList<TaskClaimResponseItem>>.Ok(await _queue.ClaimAsync(workerId, request, cancellationToken)));
	}

	[HttpPost("tasks/{taskId:guid}/progress")]
	[Authorize(AuthenticationSchemes = "Worker", Policy = "WorkerOnly")]
	public async Task<ActionResult<ApiResult>> Progress(Guid taskId, [FromHeader(Name = "X-Claim-Token")] string claimToken, [FromBody] TaskProgressReport report, CancellationToken cancellationToken)
	{
		await _queue.ReportProgressAsync(GetWorkerId(), taskId, claimToken, report, cancellationToken);
		return Ok(ApiResult.Ok());
	}

	[HttpPost("tasks/{taskId:guid}/log")]
	[Authorize(AuthenticationSchemes = "Worker", Policy = "WorkerOnly")]
	public async Task<ActionResult<ApiResult>> Log(Guid taskId, [FromHeader(Name = "X-Claim-Token")] string claimToken, [FromBody] TaskLogEntry entry, CancellationToken cancellationToken)
	{
		await _queue.ReportLogAsync(GetWorkerId(), taskId, claimToken, entry, cancellationToken);
		return Ok(ApiResult.Ok());
	}

	[HttpPost("tasks/{taskId:guid}/complete")]
	[Authorize(AuthenticationSchemes = "Worker", Policy = "WorkerOnly")]
	public async Task<ActionResult<ApiResult>> Complete(Guid taskId, [FromHeader(Name = "X-Claim-Token")] string claimToken, [FromBody] TaskCompletionReport report, CancellationToken cancellationToken)
	{
		await _queue.CompleteAsync(GetWorkerId(), taskId, claimToken, report, cancellationToken);
		return Ok(ApiResult.Ok());
	}

	[HttpPost("tasks/{taskId:guid}/fail")]
	[Authorize(AuthenticationSchemes = "Worker", Policy = "WorkerOnly")]
	public async Task<ActionResult<ApiResult>> Fail(Guid taskId, [FromHeader(Name = "X-Claim-Token")] string claimToken, [FromBody] TaskFailureReport report, CancellationToken cancellationToken)
	{
		await _queue.FailAsync(GetWorkerId(), taskId, claimToken, report, cancellationToken);
		return Ok(ApiResult.Ok());
	}

	private Guid GetWorkerId()
	{
		string id = base.User.FindFirstValue("worker_id");
		return Guid.Parse(id);
	}
}
