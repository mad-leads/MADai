using System;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MADai.Application.Features.Tasks.Commands;
using MADai.Application.Features.Tasks.Queries;
using MADai.Shared.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MADai.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/tasks")]
[ApiVersion("1.0")]
public class TasksController : ControllerBase
{
	public record CancelTaskRequestBody(string? Reason);

	private readonly IMediator _mediator;

	public TasksController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	public async Task<ActionResult<ApiResult<PagedResult<TaskSummaryDto>>>> List([FromQuery] TaskQueryRequest query, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<PagedResult<TaskSummaryDto>>.Ok(await _mediator.Send((IRequest<PagedResult<TaskSummaryDto>>)new GetTasksQuery(query), cancellationToken)));
	}

	[HttpGet("{id:guid}")]
	public async Task<ActionResult<ApiResult<TaskDetailDto>>> GetById(Guid id, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<TaskDetailDto>.Ok(await _mediator.Send((IRequest<TaskDetailDto>)new GetTaskByIdQuery(id), cancellationToken)));
	}

	[HttpPost]
	public async Task<ActionResult<ApiResult<TaskDetailDto>>> Create([FromBody] CreateTaskRequest request, CancellationToken cancellationToken)
	{
		TaskDetailDto result = await _mediator.Send((IRequest<TaskDetailDto>)new CreateTaskCommand(request), cancellationToken);
		return CreatedAtAction("GetById", new
		{
			id = result.Id,
			version = "1.0"
		}, ApiResult<TaskDetailDto>.Ok(result));
	}

	[HttpPut("{id:guid}")]
	public async Task<ActionResult<ApiResult<TaskDetailDto>>> Update(Guid id, [FromBody] UpdateTaskRequest request, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<TaskDetailDto>.Ok(await _mediator.Send((IRequest<TaskDetailDto>)new UpdateTaskCommand(id, request), cancellationToken)));
	}

	[HttpPost("{id:guid}/cancel")]
	public async Task<ActionResult<ApiResult>> Cancel(Guid id, [FromBody] CancelTaskRequestBody? body, CancellationToken cancellationToken)
	{
		await _mediator.Send((IRequest<Unit>)new CancelTaskCommand(id, body?.Reason), cancellationToken);
		return Ok(ApiResult.Ok());
	}

	[HttpPost("{id:guid}/retry")]
	public async Task<ActionResult<ApiResult>> Retry(Guid id, [FromQuery] bool forceFromDeadLetter = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		await _mediator.Send((IRequest<Unit>)new RetryTaskCommand(id, forceFromDeadLetter), cancellationToken);
		return Ok(ApiResult.Ok());
	}

	[HttpPost("import-bulk")]
	public async Task<ActionResult<ApiResult<ImportBulkTasksResult>>> ImportBulk([FromBody] ImportBulkTasksCommand command, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<ImportBulkTasksResult>.Ok(await _mediator.Send((IRequest<ImportBulkTasksResult>)command, cancellationToken)));
	}
}
