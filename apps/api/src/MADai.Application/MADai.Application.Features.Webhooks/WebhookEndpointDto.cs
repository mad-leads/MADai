using System;

namespace MADai.Application.Features.Webhooks;

public sealed record WebhookEndpointDto(Guid Id, string Url, string? EventsCsv, bool IsActive, DateTime? LastSuccessAt, DateTime? LastFailureAt, int ConsecutiveFailures, DateTime CreatedDate);
