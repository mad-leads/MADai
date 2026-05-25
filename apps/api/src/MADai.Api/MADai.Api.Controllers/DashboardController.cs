using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MADai.Application.Features.Dashboard;
using MADai.Shared.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MADai.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/dashboard")]
[ApiVersion("1.0")]
public class DashboardController : ControllerBase
{
	private readonly IDashboardService _dashboard;

	public DashboardController(IDashboardService dashboard)
	{
		_dashboard = dashboard;
	}

	[HttpGet("overview")]
	public async Task<ActionResult<ApiResult<SystemOverviewDto>>> Overview(CancellationToken cancellationToken)
	{
		return Ok(ApiResult<SystemOverviewDto>.Ok(await _dashboard.GetOverviewAsync(cancellationToken)));
	}

	[HttpGet("queues")]
	public async Task<ActionResult<ApiResult<IReadOnlyList<QueueHealthDto>>>> Queues(CancellationToken cancellationToken)
	{
		return Ok(ApiResult<IReadOnlyList<QueueHealthDto>>.Ok(await _dashboard.GetQueueHealthAsync(cancellationToken)));
	}

	[HttpGet("workers")]
	public async Task<ActionResult<ApiResult<IReadOnlyList<WorkerHealthDto>>>> Workers(CancellationToken cancellationToken)
	{
		return Ok(ApiResult<IReadOnlyList<WorkerHealthDto>>.Ok(await _dashboard.GetWorkerHealthAsync(cancellationToken)));
	}

	[HttpGet("trends/failures")]
	public async Task<ActionResult<ApiResult<IReadOnlyList<FailureTrendPointDto>>>> FailureTrend([FromQuery] int days = 14, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Ok(ApiResult<IReadOnlyList<FailureTrendPointDto>>.Ok(await _dashboard.GetFailureTrendAsync(days, cancellationToken)));
	}

	[HttpGet("trends/completions")]
	public async Task<ActionResult<ApiResult<IReadOnlyList<CompletionTrendPointDto>>>> CompletionTrend([FromQuery] int days = 14, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Ok(ApiResult<IReadOnlyList<CompletionTrendPointDto>>.Ok(await _dashboard.GetCompletionTrendAsync(days, cancellationToken)));
	}
}
