using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MADai.Application.Features.TaskRecommendations;

public interface ITaskRecommendationService
{
	Task<IReadOnlyList<TaskRecommendationDto>> ListAsync(Guid? taskId, CancellationToken cancellationToken = default(CancellationToken));

	Task<TaskRecommendationDto> ApplyAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

	Task<TaskRecommendationDto> DismissAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
}
