using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MADai.Application.Features.TaskTemplates;

public interface ITaskTemplateService
{
	Task<IReadOnlyList<TaskTemplateDto>> ListAsync(CancellationToken cancellationToken = default(CancellationToken));

	Task<TaskTemplateDto> GetAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

	Task<TaskTemplateDto> CreateAsync(CreateTaskTemplateRequest request, CancellationToken cancellationToken = default(CancellationToken));

	Task<TaskTemplateDto> UpdateAsync(Guid id, UpdateTaskTemplateRequest request, CancellationToken cancellationToken = default(CancellationToken));

	Task DeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
}
