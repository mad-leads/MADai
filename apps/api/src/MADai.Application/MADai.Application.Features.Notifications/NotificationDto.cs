using System;

namespace MADai.Application.Features.Notifications;

public sealed record NotificationDto(Guid Id, Guid? UserId, Guid? CompanyId, string Channel, string Severity, string Title, string Body, string? Url, DateTime CreatedAt, DateTime? ReadAt, DateTime? DismissedAt, string? Tags);
