namespace MADai.Application.Abstractions;

public record StoredFileInfo(string StorageProvider, string StoragePath, long SizeBytes, string? Checksum);
