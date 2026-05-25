using System;
using System.Collections.Generic;
using System.Text.Json;
using MADai.Shared.Contracts;

namespace MADai.Application.Features.ClaudeTasks.Queries;

internal static class ClaudeTaskAttachmentsParser
{
	public static IReadOnlyList<ClaudeTaskAttachmentDto> Parse(string? json)
	{
		if (string.IsNullOrWhiteSpace(json))
		{
			return Array.Empty<ClaudeTaskAttachmentDto>();
		}
		try
		{
			return JsonSerializer.Deserialize<List<ClaudeTaskAttachmentDto>>(json, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			}) ?? new List<ClaudeTaskAttachmentDto>();
		}
		catch
		{
			return Array.Empty<ClaudeTaskAttachmentDto>();
		}
	}

	public static string Serialize(IEnumerable<ClaudeTaskAttachmentDto> items)
	{
		return JsonSerializer.Serialize(items, new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		});
	}
}
