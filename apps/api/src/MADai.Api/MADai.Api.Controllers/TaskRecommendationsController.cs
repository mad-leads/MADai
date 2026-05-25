using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MADai.Application.Features.TaskRecommendations;
using MADai.Shared.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MADai.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/task-recommendations")]
[ApiVersion("1.0")]
public class TaskRecommendationsController : ControllerBase
{
	private readonly ITaskRecommendationService _svc;

	public TaskRecommendationsController(ITaskRecommendationService svc)
	{
		_svc = svc;
	}

	[HttpGet]
	public async Task<ActionResult<ApiResult<IReadOnlyList<TaskRecommendationDto>>>> List([FromQuery] Guid? taskId, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<IReadOnlyList<TaskRecommendationDto>>.Ok(await _svc.ListAsync(taskId, cancellationToken)));
	}

	[HttpPost("{id:guid}/apply")]
	public async Task<ActionResult<ApiResult<TaskRecommendationDto>>> Apply(Guid id, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<TaskRecommendationDto>.Ok(await _svc.ApplyAsync(id, cancellationToken)));
	}

	[HttpPost("{id:guid}/dismiss")]
	public async Task<ActionResult<ApiResult<TaskRecommendationDto>>> Dismiss(Guid id, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<TaskRecommendationDto>.Ok(await _svc.DismissAsync(id, cancellationToken)));
	}
}
