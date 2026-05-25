using System.Collections.Generic;

namespace MADai.Shared.Contracts;

public sealed record ClaudeBulkImportResult(int Created, int Skipped, IReadOnlyList<ClaudeTaskSummaryDto> Items);
