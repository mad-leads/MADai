using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MADai.Application.Features.PersistentWorkers;

public interface INativeProcessMonitor
{
	Task<IReadOnlyList<NativeProcessDto>> SnapshotAsync(CancellationToken cancellationToken = default(CancellationToken));
}
