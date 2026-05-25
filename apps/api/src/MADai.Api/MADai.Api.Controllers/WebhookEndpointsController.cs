using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MADai.Application.Features.Webhooks;
using MADai.Shared.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MADai.Api.Controllers;

[ApiController]
[Authorize(Roles = "SystemAdmin,CompanyAdmin")]
[Route("api/v{version:apiVersion}/webhooks")]
[ApiVersion("1.0")]
public class WebhookEndpointsController : ControllerBase
{
	private readonly IWebhookService _svc;

	public WebhookEndpointsController(IWebhookService svc)
	{
		_svc = svc;
	}

	[HttpGet]
	public async Task<ActionResult<ApiResult<IReadOnlyList<WebhookEndpointDto>>>> List(CancellationToken cancellationToken)
	{
		return Ok(ApiResult<IReadOnlyList<WebhookEndpointDto>>.Ok(await _svc.ListAsync(cancellationToken)));
	}

	[HttpPost]
	public async Task<ActionResult<ApiResult<WebhookEndpointDto>>> Create([FromBody] CreateWebhookEndpointRequest request, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<WebhookEndpointDto>.Ok(await _svc.CreateAsync(request, cancellationToken)));
	}

	[HttpPatch("{id:guid}")]
	public async Task<ActionResult<ApiResult<WebhookEndpointDto>>> Update(Guid id, [FromBody] UpdateWebhookEndpointRequest request, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<WebhookEndpointDto>.Ok(await _svc.UpdateAsync(id, request, cancellationToken)));
	}

	[HttpDelete("{id:guid}")]
	public async Task<ActionResult<ApiResult>> Delete(Guid id, CancellationToken cancellationToken)
	{
		await _svc.DeleteAsync(id, cancellationToken);
		return Ok(ApiResult.Ok());
	}

	/// <summary>Rotate the HMAC signing secret. Returns the new secret ONCE - store it client-side.</summary>
	[HttpPost("{id:guid}/rotate-secret")]
	public async Task<ActionResult<ApiResult<object>>> RotateSecret(Guid id, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<object>.Ok(new
		{
			secret = await _svc.RotateSecretAsync(id, cancellationToken)
		}));
	}
}
