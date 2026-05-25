using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MADai.Application.Features.Notifications;

public interface INotificationService
{
	Task<IReadOnlyList<NotificationDto>> ListForCurrentUserAsync(int take, CancellationToken cancellationToken = default(CancellationToken));

	Task MarkReadAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

	Task MarkAllReadAsync(CancellationToken cancellationToken = default(CancellationToken));

	Task DismissAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

	Task<NotificationDto> SendAsync(CreateNotificationRequest request, CancellationToken cancellationToken = default(CancellationToken));

	Task<IReadOnlyList<NotificationPreferenceDto>> GetPreferencesAsync(CancellationToken cancellationToken = default(CancellationToken));

	Task<NotificationPreferenceDto> UpsertPreferenceAsync(UpsertNotificationPreferenceRequest request, CancellationToken cancellationToken = default(CancellationToken));
}
