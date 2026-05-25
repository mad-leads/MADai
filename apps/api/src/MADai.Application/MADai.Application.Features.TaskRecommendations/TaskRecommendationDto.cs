using System;

namespace MADai.Application.Features.TaskRecommendations;

public sealed record TaskRecommendationDto(Guid Id, Guid? TaskId, string Title, string Body, string Source, string Status, double Confidence, DateTime? AppliedAt, DateTime CreatedDate);
