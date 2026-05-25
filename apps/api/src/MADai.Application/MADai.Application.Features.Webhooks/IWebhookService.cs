using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MADai.Application.Features.Webhooks;

public interface IWebhookService
{
	Task<IReadOnlyList<WebhookEndpointDto>> ListAsync(CancellationToken cancellationToken = default(CancellationToken));

	Task<WebhookEndpointDto> CreateAsync(CreateWebhookEndpointRequest request, CancellationToken cancellationToken = default(CancellationToken));

	Task<WebhookEndpointDto> UpdateAsync(Guid id, UpdateWebhookEndpointRequest request, CancellationToken cancellationToken = default(CancellationToken));

	Task DeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

	Task<string> RotateSecretAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
}
