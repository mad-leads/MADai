using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MADai.Application.Features.Notifications;
using MADai.Shared.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MADai.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/notifications")]
[ApiVersion("1.0")]
public class NotificationsController : ControllerBase
{
	private readonly INotificationService _svc;

	public NotificationsController(INotificationService svc)
	{
		_svc = svc;
	}

	[HttpGet]
	public async Task<ActionResult<ApiResult<IReadOnlyList<NotificationDto>>>> List([FromQuery] int take = 100, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Ok(ApiResult<IReadOnlyList<NotificationDto>>.Ok(await _svc.ListForCurrentUserAsync(take, cancellationToken)));
	}

	[HttpPost("{id:guid}/read")]
	public async Task<ActionResult<ApiResult>> Read(Guid id, CancellationToken cancellationToken)
	{
		await _svc.MarkReadAsync(id, cancellationToken);
		return Ok(ApiResult.Ok());
	}

	[HttpPost("read-all")]
	public async Task<ActionResult<ApiResult>> ReadAll(CancellationToken cancellationToken)
	{
		await _svc.MarkAllReadAsync(cancellationToken);
		return Ok(ApiResult.Ok());
	}

	[HttpPost("{id:guid}/dismiss")]
	public async Task<ActionResult<ApiResult>> Dismiss(Guid id, CancellationToken cancellationToken)
	{
		await _svc.DismissAsync(id, cancellationToken);
		return Ok(ApiResult.Ok());
	}

	[HttpPost("send")]
	[Authorize(Roles = "SystemAdmin,CompanyAdmin")]
	public async Task<ActionResult<ApiResult<NotificationDto>>> Send([FromBody] CreateNotificationRequest request, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<NotificationDto>.Ok(await _svc.SendAsync(request, cancellationToken)));
	}

	[HttpGet("preferences")]
	public async Task<ActionResult<ApiResult<IReadOnlyList<NotificationPreferenceDto>>>> GetPreferences(CancellationToken cancellationToken)
	{
		return Ok(ApiResult<IReadOnlyList<NotificationPreferenceDto>>.Ok(await _svc.GetPreferencesAsync(cancellationToken)));
	}

	[HttpPut("preferences")]
	public async Task<ActionResult<ApiResult<NotificationPreferenceDto>>> UpsertPreference([FromBody] UpsertNotificationPreferenceRequest request, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<NotificationPreferenceDto>.Ok(await _svc.UpsertPreferenceAsync(request, cancellationToken)));
	}
}
