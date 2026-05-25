using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MADai.Application.Features.ClaudeTasks.Attachments;
using MADai.Application.Features.ClaudeTasks.Commands;
using MADai.Application.Features.ClaudeTasks.Queries;
using MADai.Shared.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MADai.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/claude-tasks")]
[ApiVersion("1.0")]
public class ClaudeTasksController : ControllerBase
{
	private readonly IMediator _mediator;

	private readonly IClaudeTaskAttachmentService _attachments;

	public ClaudeTasksController(IMediator mediator, IClaudeTaskAttachmentService attachments)
	{
		_mediator = mediator;
		_attachments = attachments;
	}

	[HttpGet]
	[Authorize(AuthenticationSchemes = "Worker,Bearer")]
	public async Task<ActionResult<ApiResult<PagedResult<ClaudeTaskSummaryDto>>>> List([FromQuery] ClaudeTaskQueryRequest query, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<PagedResult<ClaudeTaskSummaryDto>>.Ok(await _mediator.Send((IRequest<PagedResult<ClaudeTaskSummaryDto>>)new GetClaudeTasksQuery(query), cancellationToken)));
	}

	[HttpGet("next")]
	[Authorize(AuthenticationSchemes = "Worker,Bearer")]
	public async Task<IActionResult> Next(CancellationToken cancellationToken)
	{
		ClaudeTaskDetailDto task = await _mediator.Send((IRequest<ClaudeTaskDetailDto>)new GetNextClaudeTaskQuery(), cancellationToken);
		if ((object)task == null)
		{
			return NoContent();
		}
		return Ok(ApiResult<ClaudeTaskDetailDto>.Ok(task));
	}

	[HttpPost("import-bulk")]
	[Authorize(AuthenticationSchemes = "Worker,Bearer")]
	public async Task<ActionResult<ApiResult<ClaudeBulkImportResult>>> ImportBulk([FromBody] ClaudeBulkImportRequest request, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<ClaudeBulkImportResult>.Ok(await _mediator.Send((IRequest<ClaudeBulkImportResult>)new ImportBulkClaudeTasksCommand(request), cancellationToken)));
	}

	[HttpGet("{id:guid}")]
	public async Task<ActionResult<ApiResult<ClaudeTaskDetailDto>>> GetById(Guid id, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<ClaudeTaskDetailDto>.Ok(await _mediator.Send((IRequest<ClaudeTaskDetailDto>)new GetClaudeTaskByIdQuery(id), cancellationToken)));
	}

	[HttpPost]
	[Authorize(Roles = "SystemAdmin")]
	public async Task<ActionResult<ApiResult<ClaudeTaskDetailDto>>> Create([FromBody] CreateClaudeTaskRequest request, CancellationToken cancellationToken)
	{
		ClaudeTaskDetailDto result = await _mediator.Send((IRequest<ClaudeTaskDetailDto>)new CreateClaudeTaskCommand(request), cancellationToken);
		return CreatedAtAction("GetById", new
		{
			id = result.Id,
			version = "1.0"
		}, ApiResult<ClaudeTaskDetailDto>.Ok(result));
	}

	[HttpPatch("{id:guid}")]
	[Authorize(AuthenticationSchemes = "Worker,Bearer")]
	public async Task<ActionResult<ApiResult<ClaudeTaskDetailDto>>> Update(Guid id, [FromBody] UpdateClaudeTaskRequest request, [FromQuery(Name = "override")] bool overrideTerminal = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Ok(ApiResult<ClaudeTaskDetailDto>.Ok(await _mediator.Send((IRequest<ClaudeTaskDetailDto>)new UpdateClaudeTaskCommand(id, request, overrideTerminal), cancellationToken)));
	}

	[HttpDelete("{id:guid}")]
	[Authorize(Roles = "SystemAdmin")]
	public async Task<ActionResult<ApiResult>> Delete(Guid id, CancellationToken cancellationToken)
	{
		await _mediator.Send((IRequest<Unit>)new DeleteClaudeTaskCommand(id), cancellationToken);
		return Ok(ApiResult.Ok());
	}

	[HttpPost("{id:guid}/attachments")]
	[RequestSizeLimit(104857600L)]
	[Authorize(Roles = "SystemAdmin")]
	public async Task<ActionResult<ApiResult<ClaudeTaskAttachmentDto>>> UploadAttachment(Guid id, IFormFile file, CancellationToken cancellationToken)
	{
		if (file == null || file.Length == 0L)
		{
			return BadRequest(ApiResult<ClaudeTaskAttachmentDto>.Fail("File required."));
		}
		Stream stream = file.OpenReadStream();
		ActionResult<ApiResult<ClaudeTaskAttachmentDto>> result;
		try
		{
			result = Ok(ApiResult<ClaudeTaskAttachmentDto>.Ok(await _attachments.UploadAsync(id, file.FileName, file.ContentType ?? "application/octet-stream", file.Length, stream, cancellationToken)));
		}
		finally
		{
			if (stream != null)
			{
				await stream.DisposeAsync();
			}
		}
		return result;
	}

	[HttpDelete("{id:guid}/attachments/{filename}")]
	[Authorize(Roles = "SystemAdmin")]
	public async Task<ActionResult<ApiResult>> DeleteAttachment(Guid id, string filename, CancellationToken cancellationToken)
	{
		await _attachments.DeleteAsync(id, filename, cancellationToken);
		return Ok(ApiResult.Ok());
	}
}
