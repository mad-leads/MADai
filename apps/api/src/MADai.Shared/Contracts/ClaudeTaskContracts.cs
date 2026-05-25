using MADai.Domain.Enums;

namespace MADai.Shared.Contracts;

public sealed record ClaudeTaskSummaryDto(
    Guid Id,
    string Title,
    ClaudeTaskStatus Status,
    ClaudeTaskPriority Priority,
    int AttachmentCount,
    DateTime CreatedDate,
    DateTime? ModifiedDate);

public sealed record ClaudeTaskAttachmentDto(
    string FileName,
    string StoragePath,
    string ContentType,
    long SizeBytes);

public sealed record ClaudeTaskDetailDto(
    Guid Id,
    string Title,
    string? Description,
    string? Notes,
    ClaudeTaskStatus Status,
    ClaudeTaskPriority Priority,
    IReadOnlyList<ClaudeTaskAttachmentDto> Attachments,
    DateTime CreatedDate,
    DateTime? ModifiedDate);

public sealed record CreateClaudeTaskRequest(
    string Title,
    string? Description,
    string? Notes,
    ClaudeTaskPriority? Priority);

public sealed record UpdateClaudeTaskRequest(
    string? Title,
    string? Description,
    string? Notes,
    ClaudeTaskStatus? Status,
    ClaudeTaskPriority? Priority);

public sealed record ClaudeTaskQueryRequest : PageQuery
{
    public ClaudeTaskStatus[]? Statuses { get; init; }
    public ClaudeTaskPriority? MinPriority { get; init; }
    public bool? IncludeTerminal { get; init; }

    public new ClaudeTaskQueryRequest Normalize(int maxPageSize = 200) => this with
    {
        Page = Page < 1 ? 1 : Page,
        PageSize = PageSize switch
        {
            < 1 => 25,
            _ when PageSize > maxPageSize => maxPageSize,
            _ => PageSize
        }
    };
}

public sealed record ClaudePromptTemplateDto(
    Guid Id,
    string Name,
    string? Description,
    string Content,
    DateTime CreatedDate,
    DateTime? ModifiedDate);

public sealed record CreateClaudePromptTemplateRequest(string Name, string? Description, string Content);
public sealed record UpdateClaudePromptTemplateRequest(string? Name, string? Description, string? Content);

public sealed record ClaudeBulkImportItem(string Title, string? Description, string? Notes, ClaudeTaskPriority? Priority);
public sealed record ClaudeBulkImportRequest(IReadOnlyList<ClaudeBulkImportItem> Items);
public sealed record ClaudeBulkImportResult(int Created, int Skipped, IReadOnlyList<ClaudeTaskSummaryDto> Items);

public sealed record SystemSettingDto(string Key, string Category, string? Value, string? DataType, string? Description);
