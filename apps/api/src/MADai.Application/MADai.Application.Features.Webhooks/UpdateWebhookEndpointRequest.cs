namespace MADai.Application.Features.Webhooks;

public sealed record UpdateWebhookEndpointRequest(string? Url, string? EventsCsv, bool? IsActive);
