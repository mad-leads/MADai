using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MADai.Application.Features.TaskTemplates;
using MADai.Shared.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MADai.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/task-templates")]
[ApiVersion("1.0")]
public class TaskTemplatesController : ControllerBase
{
	private readonly ITaskTemplateService _svc;

	public TaskTemplatesController(ITaskTemplateService svc)
	{
		_svc = svc;
	}

	[HttpGet]
	public async Task<ActionResult<ApiResult<IReadOnlyList<TaskTemplateDto>>>> List(CancellationToken cancellationToken)
	{
		return Ok(ApiResult<IReadOnlyList<TaskTemplateDto>>.Ok(await _svc.ListAsync(cancellationToken)));
	}

	[HttpGet("{id:guid}")]
	public async Task<ActionResult<ApiResult<TaskTemplateDto>>> Get(Guid id, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<TaskTemplateDto>.Ok(await _svc.GetAsync(id, cancellationToken)));
	}

	[HttpPost]
	[Authorize(Roles = "SystemAdmin,CompanyAdmin,CompanyManager")]
	public async Task<ActionResult<ApiResult<TaskTemplateDto>>> Create([FromBody] CreateTaskTemplateRequest request, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<TaskTemplateDto>.Ok(await _svc.CreateAsync(request, cancellationToken)));
	}

	[HttpPatch("{id:guid}")]
	[Authorize(Roles = "SystemAdmin,CompanyAdmin,CompanyManager")]
	public async Task<ActionResult<ApiResult<TaskTemplateDto>>> Update(Guid id, [FromBody] UpdateTaskTemplateRequest request, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<TaskTemplateDto>.Ok(await _svc.UpdateAsync(id, request, cancellationToken)));
	}

	[HttpDelete("{id:guid}")]
	[Authorize(Roles = "SystemAdmin,CompanyAdmin")]
	public async Task<ActionResult<ApiResult>> Delete(Guid id, CancellationToken cancellationToken)
	{
		await _svc.DeleteAsync(id, cancellationToken);
		return Ok(ApiResult.Ok());
	}
}
