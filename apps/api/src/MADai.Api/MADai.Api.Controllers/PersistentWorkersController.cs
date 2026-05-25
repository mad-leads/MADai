using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MADai.Application.Features.PersistentWorkers;
using MADai.Shared.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MADai.Api.Controllers;

[ApiController]
[Authorize(Roles = "SystemAdmin")]
[Route("api/v{version:apiVersion}/persistent-workers")]
[ApiVersion("1.0")]
public class PersistentWorkersController : ControllerBase
{
	private readonly IRepositoryIntelligenceService _intelligence;

	private readonly ISessionOrchestrator _sessions;

	private readonly INativeProcessMonitor _processes;

	public PersistentWorkersController(IRepositoryIntelligenceService intelligence, ISessionOrchestrator sessions, INativeProcessMonitor processes)
	{
		_intelligence = intelligence;
		_sessions = sessions;
		_processes = processes;
	}

	[HttpPost("repositories/intelligence")]
	public async Task<ActionResult<ApiResult<RepositoryIntelligenceDto>>> GetOrRefreshIntelligence([FromBody] RepositoryRegistrationRequest request, [FromQuery] bool forceRefresh = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Ok(ApiResult<RepositoryIntelligenceDto>.Ok(await _intelligence.GetOrRefreshAsync(request, forceRefresh, cancellationToken)));
	}

	[HttpPost("sessions/inject")]
	public async Task<ActionResult<ApiResult<InjectTaskResponse>>> InjectTask([FromBody] InjectTaskRequest request, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<InjectTaskResponse>.Ok(await _sessions.InjectAsync(request, cancellationToken)));
	}

	[HttpPost("sessions/rotate")]
	public async Task<ActionResult<ApiResult>> RotateSession([FromBody] SessionRotationRequest request, CancellationToken cancellationToken)
	{
		await _sessions.RotateAsync(request, cancellationToken);
		return Ok(ApiResult.Ok());
	}

	[HttpGet("native-processes")]
	public async Task<ActionResult<ApiResult<IReadOnlyList<NativeProcessDto>>>> NativeProcesses(CancellationToken cancellationToken)
	{
		return Ok(ApiResult<IReadOnlyList<NativeProcessDto>>.Ok(await _processes.SnapshotAsync(cancellationToken)));
	}
}
