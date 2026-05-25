using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MADai.Application.Abstractions;
using MADai.Domain.Audit;
using MADai.Shared.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MADai.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/audit")]
[ApiVersion("1.0")]
public class AuditController : ControllerBase
{
	private readonly IDbContextAccess _db;

	private readonly ICurrentUserService _currentUser;

	public AuditController(IDbContextAccess db, ICurrentUserService currentUser)
	{
		_db = db;
		_currentUser = currentUser;
	}

	[HttpGet("findings")]
	public async Task<ActionResult<ApiResult<IReadOnlyList<object>>>> Findings(CancellationToken cancellationToken)
	{
		return Ok(ApiResult<IReadOnlyList<object>>.Ok((await (from f in (from f in _db.AuditFindings.AsNoTracking()
				orderby f.CreatedDate descending
				select f).Take(500)
			select new { f.Id, f.Title, f.Severity, f.Status, f.ScanType, f.AffectedResource, f.RecommendedAction, f.CreatedDate }).ToListAsync(cancellationToken)).Cast<object>().ToList()));
	}

	[HttpGet("runs")]
	public async Task<ActionResult<ApiResult<IReadOnlyList<object>>>> Runs(CancellationToken cancellationToken)
	{
		return Ok(ApiResult<IReadOnlyList<object>>.Ok((await (from r in (from r in _db.AuditRuns.AsNoTracking()
				orderby r.StartedAt descending
				select r).Take(100)
			select new { r.Id, r.ScanType, r.StartedAt, r.CompletedAt, r.Status, r.FindingsCount, r.RecommendationsCount, r.Summary }).ToListAsync(cancellationToken)).Cast<object>().ToList()));
	}

	[HttpGet("recommendations")]
	public async Task<ActionResult<ApiResult<IReadOnlyList<object>>>> Recommendations(CancellationToken cancellationToken)
	{
		return Ok(ApiResult<IReadOnlyList<object>>.Ok((await (from r in (from r in _db.AuditRecommendations.AsNoTracking()
				orderby r.CreatedDate descending
				select r).Take(200)
			select new { r.Id, r.Title, r.Body, r.Severity, r.Status, r.CreatedDate, r.GeneratedTaskId }).ToListAsync(cancellationToken)).Cast<object>().ToList()));
	}
}
