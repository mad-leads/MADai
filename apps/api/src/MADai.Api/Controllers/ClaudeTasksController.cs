using MADai.Application.Features.ClaudeTasks.Attachments;
using MADai.Application.Features.ClaudeTasks.Commands;
using MADai.Application.Features.ClaudeTasks.Queries;
using MADai.Shared.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MADai.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/claude-tasks")]
[Asp.Versioning.ApiVersion("1.0")]
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
    [Authorize(AuthenticationSchemes = MADaiConstants.AuthSchemeWorker + "," + JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<ApiResult<PagedResult<ClaudeTaskSummaryDto>>>> List([FromQuery] ClaudeTaskQueryRequest query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetClaudeTasksQuery(query), cancellationToken);
        return Ok(ApiResult<PagedResult<ClaudeTaskSummaryDto>>.Ok(result));
    }

    // Route order matters: literal segments BEFORE /{id:guid}.
    [HttpGet("next")]
    [Authorize(AuthenticationSchemes = MADaiConstants.AuthSchemeWorker + "," + JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Next(CancellationToken cancellationToken)
    {
        var task = await _mediator.Send(new GetNextClaudeTaskQuery(), cancellationToken);
        if (task is null) return NoContent();
        return Ok(ApiResult<ClaudeTaskDetailDto>.Ok(task));
    }

    [HttpPost("import-bulk")]
    [Authorize(AuthenticationSchemes = MADaiConstants.AuthSchemeWorker + "," + JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<ApiResult<ClaudeBulkImportResult>>> ImportBulk([FromBody] ClaudeBulkImportRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ImportBulkClaudeTasksCommand(request), cancellationToken);
        return Ok(ApiResult<ClaudeBulkImportResult>.Ok(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResult<ClaudeTaskDetailDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetClaudeTaskByIdQuery(id), cancellationToken);
        return Ok(ApiResult<ClaudeTaskDetailDto>.Ok(result));
    }

    [HttpPost]
    [Authorize(Roles = "SystemAdmin")]
    public async Task<ActionResult<ApiResult<ClaudeTaskDetailDto>>> Create([FromBody] CreateClaudeTaskRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateClaudeTaskCommand(request), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id, version = "1.0" }, ApiResult<ClaudeTaskDetailDto>.Ok(result));
    }

    [HttpPatch("{id:guid}")]
    [Authorize(AuthenticationSchemes = MADaiConstants.AuthSchemeWorker + "," + JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<ApiResult<ClaudeTaskDetailDto>>> Update(
        Guid id,
        [FromBody] UpdateClaudeTaskRequest request,
        [FromQuery(Name = "override")] bool overrideTerminal = false,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new UpdateClaudeTaskCommand(id, request, overrideTerminal), cancellationToken);
        return Ok(ApiResult<ClaudeTaskDetailDto>.Ok(result));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "SystemAdmin")]
    public async Task<ActionResult<ApiResult>> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteClaudeTaskCommand(id), cancellationToken);
        return Ok(ApiResult.Ok());
    }

    [HttpPost("{id:guid}/attachments")]
    [RequestSizeLimit(100 * 1024 * 1024)]
    [Authorize(Roles = "SystemAdmin")]
    public async Task<ActionResult<ApiResult<ClaudeTaskAttachmentDto>>> UploadAttachment(Guid id, IFormFile file, CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
            return BadRequest(ApiResult<ClaudeTaskAttachmentDto>.Fail("File required."));

        await using var stream = file.OpenReadStream();
        var result = await _attachments.UploadAsync(id, file.FileName, file.ContentType ?? "application/octet-stream", file.Length, stream, cancellationToken);
        return Ok(ApiResult<ClaudeTaskAttachmentDto>.Ok(result));
    }

    [HttpDelete("{id:guid}/attachments/{filename}")]
    [Authorize(Roles = "SystemAdmin")]
    public async Task<ActionResult<ApiResult>> DeleteAttachment(Guid id, string filename, CancellationToken cancellationToken)
    {
        await _attachments.DeleteAsync(id, filename, cancellationToken);
        return Ok(ApiResult.Ok());
    }
}
