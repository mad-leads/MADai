using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MADai.Application.Features.ClaudePromptTemplates;
using MADai.Shared.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MADai.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/claude-prompt-templates")]
[ApiVersion("1.0")]
public class ClaudePromptTemplatesController : ControllerBase
{
	private readonly IClaudePromptTemplateService _service;

	public ClaudePromptTemplatesController(IClaudePromptTemplateService service)
	{
		_service = service;
	}

	[HttpGet]
	public async Task<ActionResult<ApiResult<IReadOnlyList<ClaudePromptTemplateDto>>>> List(CancellationToken cancellationToken)
	{
		return Ok(ApiResult<IReadOnlyList<ClaudePromptTemplateDto>>.Ok(await _service.ListAsync(cancellationToken)));
	}

	[HttpPost]
	[Authorize(Roles = "SystemAdmin")]
	public async Task<ActionResult<ApiResult<ClaudePromptTemplateDto>>> Create([FromBody] CreateClaudePromptTemplateRequest request, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<ClaudePromptTemplateDto>.Ok(await _service.CreateAsync(request, cancellationToken)));
	}

	[HttpPatch("{id:guid}")]
	[Authorize(Roles = "SystemAdmin")]
	public async Task<ActionResult<ApiResult<ClaudePromptTemplateDto>>> Update(Guid id, [FromBody] UpdateClaudePromptTemplateRequest request, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<ClaudePromptTemplateDto>.Ok(await _service.UpdateAsync(id, request, cancellationToken)));
	}

	[HttpDelete("{id:guid}")]
	[Authorize(Roles = "SystemAdmin")]
	public async Task<ActionResult<ApiResult>> Delete(Guid id, CancellationToken cancellationToken)
	{
		await _service.DeleteAsync(id, cancellationToken);
		return Ok(ApiResult.Ok());
	}
}
