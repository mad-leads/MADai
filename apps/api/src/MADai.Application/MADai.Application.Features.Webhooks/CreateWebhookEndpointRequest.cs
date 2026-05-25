namespace MADai.Application.Features.Webhooks;

public sealed record CreateWebhookEndpointRequest(string Url, string? EventsCsv, bool IsActive = true);
