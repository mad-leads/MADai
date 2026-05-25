using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MADai.Shared.Contracts;

namespace MADai.Application.Features.ClaudePromptTemplates;

public interface IClaudePromptTemplateService
{
	Task<IReadOnlyList<ClaudePromptTemplateDto>> ListAsync(CancellationToken cancellationToken = default(CancellationToken));

	Task<ClaudePromptTemplateDto> CreateAsync(CreateClaudePromptTemplateRequest request, CancellationToken cancellationToken = default(CancellationToken));

	Task<ClaudePromptTemplateDto> UpdateAsync(Guid id, UpdateClaudePromptTemplateRequest request, CancellationToken cancellationToken = default(CancellationToken));

	Task DeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
}
