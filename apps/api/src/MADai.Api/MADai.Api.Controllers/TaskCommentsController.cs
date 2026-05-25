using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MADai.Application.Features.TaskComments;
using MADai.Shared.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MADai.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/tasks/{taskId:guid}/comments")]
[ApiVersion("1.0")]
public class TaskCommentsController : ControllerBase
{
	private readonly ITaskCommentService _svc;

	public TaskCommentsController(ITaskCommentService svc)
	{
		_svc = svc;
	}

	[HttpGet]
	public async Task<ActionResult<ApiResult<IReadOnlyList<TaskCommentDto>>>> List(Guid taskId, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<IReadOnlyList<TaskCommentDto>>.Ok(await _svc.ListAsync(taskId, cancellationToken)));
	}

	[HttpPost]
	public async Task<ActionResult<ApiResult<TaskCommentDto>>> Add(Guid taskId, [FromBody] CreateTaskCommentRequest request, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<TaskCommentDto>.Ok(await _svc.AddAsync(taskId, request, cancellationToken)));
	}

	[HttpDelete("{commentId:guid}")]
	public async Task<ActionResult<ApiResult>> Delete(Guid taskId, Guid commentId, CancellationToken cancellationToken)
	{
		await _svc.DeleteAsync(commentId, cancellationToken);
		return Ok(ApiResult.Ok());
	}
}
