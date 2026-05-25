using MADai.Shared.Contracts;
using MediatR;

namespace MADai.Application.Features.ClaudeTasks.Commands;

public sealed record ImportBulkClaudeTasksCommand(ClaudeBulkImportRequest Request) : IRequest<ClaudeBulkImportResult>, IBaseRequest;
