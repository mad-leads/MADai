namespace MADai.Application.Features.Notifications;

public sealed record UpsertNotificationPreferenceRequest(string Category, bool Email, bool InApp, bool Push, bool Webhook);
