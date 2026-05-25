using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MADai.Application.Abstractions;
using MADai.Application.Features.Workers;
using MADai.Domain.Workers;
using MADai.Shared.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MADai.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/workers")]
[ApiVersion("1.0")]
public class WorkersAdminController : ControllerBase
{
	private readonly IDbContextAccess _db;

	private readonly IWorkerRegistrationService _registration;

	private readonly ICurrentUserService _currentUser;

	public WorkersAdminController(IDbContextAccess db, IWorkerRegistrationService registration, ICurrentUserService currentUser)
	{
		_db = db;
		_registration = registration;
		_currentUser = currentUser;
	}

	[HttpGet]
	public async Task<ActionResult<ApiResult<IReadOnlyList<object>>>> List(CancellationToken cancellationToken)
	{
		IQueryable<WorkerNode> query = _db.WorkerNodes.AsNoTracking().AsQueryable();
		Guid? companyId = _currentUser.CompanyId;
		if (companyId.HasValue)
		{
			Guid cid = companyId.GetValueOrDefault();
			query = query.Where((WorkerNode w) => w.CompanyId == null || w.CompanyId == cid);
		}
		return Ok(ApiResult<IReadOnlyList<object>>.Ok((await (from w in query
			orderby w.LastHeartbeatAt descending
			select new
			{
				w.Id, w.Name, w.MachineName, w.AgentVersion, w.Status, w.QueueName, w.MaxConcurrency, w.CurrentConcurrency, w.LastHeartbeatAt, w.IsActive,
				w.CompanyId
			}).ToListAsync(cancellationToken)).Cast<object>().ToList()));
	}

	[HttpPost("{id:guid}/drain")]
	public async Task<ActionResult<ApiResult>> Drain(Guid id, CancellationToken cancellationToken)
	{
		await _registration.DrainAsync(id, cancellationToken);
		return Ok(ApiResult.Ok());
	}

	[HttpPost("{id:guid}/deactivate")]
	public async Task<ActionResult<ApiResult>> Deactivate(Guid id, CancellationToken cancellationToken)
	{
		await _registration.DeactivateAsync(id, cancellationToken);
		return Ok(ApiResult.Ok());
	}
}
