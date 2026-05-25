using System;

namespace MADai.Shared.Contracts;

public sealed record TaskArtifactDto(Guid Id, string FileName, string ContentType, long SizeBytes, string StorageProvider, string? PreviewUrl, bool IsFinal, DateTime CreatedDate, int Version, string? Kind);
