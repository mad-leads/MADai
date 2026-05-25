using System.Collections.Generic;

namespace MADai.Shared.Contracts;

public sealed record ClaudeBulkImportRequest(IReadOnlyList<ClaudeBulkImportItem> Items);
