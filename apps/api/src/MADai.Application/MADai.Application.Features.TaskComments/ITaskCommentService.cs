using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MADai.Application.Features.TaskComments;

public interface ITaskCommentService
{
	Task<IReadOnlyList<TaskCommentDto>> ListAsync(Guid taskId, CancellationToken cancellationToken = default(CancellationToken));

	Task<TaskCommentDto> AddAsync(Guid taskId, CreateTaskCommentRequest request, CancellationToken cancellationToken = default(CancellationToken));

	Task DeleteAsync(Guid commentId, CancellationToken cancellationToken = default(CancellationToken));
}
