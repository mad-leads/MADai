namespace MADai.Application.Features.Notifications;

public sealed record NotificationPreferenceDto(string Category, bool Email, bool InApp, bool Push, bool Webhook);
