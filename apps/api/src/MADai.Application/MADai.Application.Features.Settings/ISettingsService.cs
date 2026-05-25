using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MADai.Shared.Contracts;

namespace MADai.Application.Features.Settings;

public interface ISettingsService
{
	Task<IReadOnlyList<SystemSettingDto>> ListAsync(string? categoryPrefix, CancellationToken cancellationToken = default(CancellationToken));

	Task<IReadOnlyDictionary<string, string?>> UpdateBatchAsync(IReadOnlyDictionary<string, string?> updates, CancellationToken cancellationToken = default(CancellationToken));
}
