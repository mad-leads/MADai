using System;

namespace MADai.Application.Features.Notifications;

public sealed record CreateNotificationRequest(Guid? UserId, Guid? CompanyId, string? Channel, string? Severity, string Title, string Body, string? Url, string? Tags);
