using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MADai.Application.Abstractions;
using MADai.Domain.Tasks;
using MADai.Domain.Workers;
using MADai.Shared.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MADai.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/workers/{workerId:guid}")]
[ApiVersion("1.0")]
public class WorkerLogsController : ControllerBase
{
	private readonly IDbContextAccess _db;

	private readonly ICurrentUserService _currentUser;

	public WorkerLogsController(IDbContextAccess db, ICurrentUserService currentUser)
	{
		_db = db;
		_currentUser = currentUser;
	}

	[HttpGet("heartbeats")]
	public async Task<ActionResult<ApiResult<IReadOnlyList<object>>>> Heartbeats(Guid workerId, [FromQuery] int take = 50, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Ok(ApiResult<IReadOnlyList<object>>.Ok((await (from h in (from h in _db.WorkerHeartbeats.AsNoTracking()
				where h.WorkerNodeId == workerId
				orderby h.Timestamp descending
				select h).Take(Math.Clamp(take, 1, 500))
			select new { h.Id, h.Timestamp, h.Status, h.ActiveTasks, h.CpuPercent, h.MemoryMb, h.DiskFreeGb, h.Notes }).ToListAsync(cancellationToken)).Cast<object>().ToList()));
	}

	[HttpGet("metrics")]
	public async Task<ActionResult<ApiResult<IReadOnlyList<object>>>> Metrics(Guid workerId, [FromQuery] int take = 100, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Ok(ApiResult<IReadOnlyList<object>>.Ok((await (from m in (from m in _db.WorkerMetrics.AsNoTracking()
				where m.WorkerNodeId == workerId
				orderby m.Timestamp descending
				select m).Take(Math.Clamp(take, 1, 1000))
			select new { m.Id, m.Timestamp, m.MetricName, m.Value, m.Unit, m.Tags }).ToListAsync(cancellationToken)).Cast<object>().ToList()));
	}

	[HttpGet("active-tasks")]
	public async Task<ActionResult<ApiResult<IReadOnlyList<object>>>> ActiveTasks(Guid workerId, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<IReadOnlyList<object>>.Ok((await (from t in _db.Tasks.AsNoTracking()
			where t.ClaimedByWorkerId == workerId
			orderby t.ClaimedAt descending
			select new { t.Id, t.Title, t.Status, t.Progress, t.StartedAt, t.ClaimedAt }).Take(200).ToListAsync(cancellationToken)).Cast<object>().ToList()));
	}
}
