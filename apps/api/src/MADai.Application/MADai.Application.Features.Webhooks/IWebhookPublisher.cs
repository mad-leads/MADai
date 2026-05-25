using System;
using System.Threading;
using System.Threading.Tasks;

namespace MADai.Application.Features.Webhooks;

public interface IWebhookPublisher
{
	Task PublishAsync(Guid companyId, string eventType, object payload, CancellationToken cancellationToken = default(CancellationToken));
}
