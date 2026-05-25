namespace MADai.Shared.Contracts;

public sealed record ClaudeTaskAttachmentDto(string FileName, string StoragePath, string ContentType, long SizeBytes);
