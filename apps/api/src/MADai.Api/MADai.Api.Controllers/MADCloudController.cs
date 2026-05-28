using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Domain.SystemEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MADai.Api.Controllers;

[ApiController]
[Route("api/madcloud")]
public sealed class MADCloudController : ControllerBase
{
	private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
	{
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
	};

	private readonly IConfiguration _configuration;
	private readonly IDbContextAccess _db;
	private readonly IHostEnvironment _environment;
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly ILogger<MADCloudController> _logger;

	public MADCloudController(
		IConfiguration configuration,
		IDbContextAccess db,
		IHostEnvironment environment,
		IHttpClientFactory httpClientFactory,
		ILogger<MADCloudController> logger)
	{
		_configuration = configuration;
		_db = db;
		_environment = environment;
		_httpClientFactory = httpClientFactory;
		_logger = logger;
	}

	[HttpGet("config")]
	[AllowAnonymous]
	public ActionResult<MADCloudConfigResponse> Config()
	{
		return Ok(new MADCloudConfigResponse(
			SourceApp,
			ProductName,
			MADCloudApiUrl,
			AiRoute,
			CallbackUrl,
			"/api/madcloud/requests",
			"/api/madcloud/ai-results",
			IsConfigured));
	}

	[HttpPost("requests")]
	[HttpPost("/api/v1/madcloud/requests")]
	[Authorize(Roles = "SystemAdmin")]
	public async Task<IActionResult> Submit([FromBody] MADCloudSubmitRequest request, CancellationToken cancellationToken)
	{
		if (!IsConfigured)
		{
			return StatusCode(StatusCodes.Status503ServiceUnavailable, new
			{
				provider = "madcloud",
				configured = false,
				message = "MADCloud server-to-server credentials are not configured."
			});
		}

		if (string.IsNullOrWhiteSpace(request.Prompt))
		{
			return BadRequest(new { message = "Prompt is required." });
		}

		var path = request.RequestKind?.Equals("CodeFix", StringComparison.OrdinalIgnoreCase) == true
			? "/api/ai/code-fixes"
			: "/api/ai/requests";

		var payload = new MADCloudRequestEnvelope(
			SourceApp,
			_environment.EnvironmentName,
			request.CorrelationId,
			request.IdempotencyKey,
			CallbackUrl,
			string.IsNullOrWhiteSpace(request.RequestKind) ? "Text" : request.RequestKind!,
			request.Capability,
			request.Priority,
			string.IsNullOrWhiteSpace(request.TargetApplicationSlug) ? SourceApp : request.TargetApplicationSlug,
			request.Title,
			request.Prompt,
			request.SystemPrompt,
			request.Input,
			request.ExpectedOutputSchema,
			request.Attachments,
			request.Metadata);

		var body = JsonSerializer.Serialize(payload, JsonOptions);
		using var httpRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(MADCloudApiUrl), path))
		{
			Content = new StringContent(body, Encoding.UTF8, "application/json")
		};
		Sign(httpRequest, "POST", path, body);

		if (!string.IsNullOrWhiteSpace(request.IdempotencyKey))
		{
			httpRequest.Headers.TryAddWithoutValidation("Idempotency-Key", request.IdempotencyKey);
		}

		using var response = await _httpClientFactory.CreateClient("madcloud").SendAsync(httpRequest, cancellationToken);
		var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

		await AuditAsync("MADCloud.RequestSubmitted", request.CorrelationId, responseBody, response.IsSuccessStatusCode ? "Info" : "Warning", cancellationToken);
		return StatusCode((int)response.StatusCode, AsJsonOrText(responseBody));
	}

	[HttpPost("ai-results")]
	[HttpPost("/api/v1/madcloud/ai-results")]
	[AllowAnonymous]
	public async Task<IActionResult> ReceiveResult(CancellationToken cancellationToken)
	{
		using var reader = new StreamReader(Request.Body, Encoding.UTF8);
		var body = await reader.ReadToEndAsync(cancellationToken);

		if (!VerifyIncomingSignature(body))
		{
			await AuditAsync("MADCloud.CallbackRejected", null, "Invalid MADCloud callback signature.", "Warning", cancellationToken);
			return Unauthorized(new { message = "Invalid MADCloud callback signature." });
		}

		var payload = AsJsonOrText(body);
		var entityId = ExtractRequestId(payload);
		await AuditAsync("MADCloud.CallbackReceived", entityId, body, SeverityFromPayload(payload), cancellationToken);
		await UpdateCorrelatedTaskAsync(payload, body, cancellationToken);
		_logger.LogInformation("MADCloud callback received for {RequestId}.", entityId ?? "unknown-request");

		return Ok(new
		{
			accepted = true,
			provider = "madcloud",
			requestId = entityId,
			receivedUtc = DateTime.UtcNow
		});
	}

	private async Task AuditAsync(string action, string? entityId, string detail, string severity, CancellationToken cancellationToken)
	{
		_db.AuditLogs.Add(new AuditLog
		{
			Action = action,
			EntityType = "MADCloud",
			EntityId = Truncate(entityId, 60),
			IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
			Detail = Truncate(detail, 4000),
			Severity = severity
		});
		await _db.SaveChangesAsync(cancellationToken);
	}

	private void Sign(HttpRequestMessage request, string method, string path, string body)
	{
		var timestamp = DateTimeOffset.UtcNow.ToString("O");
		var nonce = Guid.NewGuid().ToString("N");
		var bodyHash = Sha256Hex(body);
		var payload = $"{method.ToUpperInvariant()}\n{path}\n{timestamp}\n{nonce}\n{bodyHash}";
		var signature = HmacHex(payload, MADCloudSecret);

		request.Headers.TryAddWithoutValidation("X-MADCloud-App", SourceApp);
		request.Headers.TryAddWithoutValidation("X-MADCloud-Timestamp", timestamp);
		request.Headers.TryAddWithoutValidation("X-MADCloud-Nonce", nonce);
		request.Headers.TryAddWithoutValidation("X-MADCloud-Signature", signature);
	}

	private bool VerifyIncomingSignature(string body)
	{
		if (string.IsNullOrWhiteSpace(MADCloudSecret))
		{
			return true;
		}

		var app = Header("X-MADCloud-App");
		var timestamp = Header("X-MADCloud-Timestamp");
		var nonce = Header("X-MADCloud-Nonce");
		var supplied = Header("X-MADCloud-Signature");
		if (!string.Equals(app, SourceApp, StringComparison.OrdinalIgnoreCase) ||
		    string.IsNullOrWhiteSpace(timestamp) ||
		    string.IsNullOrWhiteSpace(nonce) ||
		    string.IsNullOrWhiteSpace(supplied))
		{
			return false;
		}

		var path = Request.Path.Value ?? "/api/madcloud/ai-results";
		var payload = $"POST\n{path}\n{timestamp}\n{nonce}\n{Sha256Hex(body)}";
		var expected = HmacHex(payload, MADCloudSecret);
		return CryptographicOperations.FixedTimeEquals(
			Encoding.UTF8.GetBytes(expected),
			Encoding.UTF8.GetBytes(supplied.ToLowerInvariant()));
	}

	private object AsJsonOrText(string body)
	{
		if (string.IsNullOrWhiteSpace(body))
		{
			return new { };
		}

		try
		{
			return JsonSerializer.Deserialize<JsonElement>(body, JsonOptions);
		}
		catch
		{
			return new { text = body };
		}
	}

	private static string? ExtractRequestId(object payload)
	{
		if (payload is not JsonElement element || element.ValueKind != JsonValueKind.Object)
		{
			return null;
		}

		foreach (var name in new[] { "requestId", "RequestId", "aiRequestId", "AiRequestId" })
		{
			if (element.TryGetProperty(name, out var value))
			{
				return value.GetString();
			}
		}

		return null;
	}

	private async Task UpdateCorrelatedTaskAsync(object payload, string rawBody, CancellationToken cancellationToken)
	{
		if (payload is not JsonElement element || element.ValueKind != JsonValueKind.Object)
		{
			return;
		}

		var correlation = ExtractString(element, "correlationId", "CorrelationId", "taskId", "TaskId");
		if (!Guid.TryParse(correlation, out var taskId))
		{
			return;
		}

		var task = await _db.Tasks.FirstOrDefaultAsync(item => item.Id == taskId, cancellationToken);
		if (task is null)
		{
			await AuditAsync("MADCloud.CallbackUnmatched", correlation, "MADCloud returned a correlation id that does not match a MADai task.", "Warning", cancellationToken);
			return;
		}

		var status = ExtractString(element, "status", "Status");
		var failed = status?.Equals("Failed", StringComparison.OrdinalIgnoreCase) == true ||
		             status?.Equals("Error", StringComparison.OrdinalIgnoreCase) == true;
		task.ResultPayload = rawBody;
		task.OutputSummary = ExtractString(element, "summary", "outputSummary", "message", "result", "text") ?? task.OutputSummary;
		task.Progress = failed ? task.Progress : 100;
		task.Status = failed ? MADai.Domain.Enums.TaskStatus.Failed : MADai.Domain.Enums.TaskStatus.Completed;
		task.CompletedAt = DateTime.UtcNow;
		if (failed)
		{
			task.ErrorMessage = task.OutputSummary ?? "MADai AI returned a failed result.";
		}

		await _db.SaveChangesAsync(cancellationToken);
	}

	private static string? ExtractString(JsonElement element, params string[] names)
	{
		foreach (var name in names)
		{
			if (element.TryGetProperty(name, out var value))
			{
				return value.ValueKind == JsonValueKind.String ? value.GetString() : value.ToString();
			}
		}

		return null;
	}

	private static string SeverityFromPayload(object payload)
	{
		if (payload is JsonElement element &&
		    element.ValueKind == JsonValueKind.Object &&
		    element.TryGetProperty("status", out var status) &&
		    status.GetString()?.Equals("Failed", StringComparison.OrdinalIgnoreCase) == true)
		{
			return "Warning";
		}

		return "Info";
	}

	private string? Header(string name) => Request.Headers.TryGetValue(name, out var value) ? value.ToString() : null;
	private string SourceApp => FirstNonBlank(_configuration["MADCLOUD_APP_ID"], _configuration["MADCloud:AppId"], "madai");
	private string ProductName => FirstNonBlank(_configuration["Product:Name"], _configuration["MADCloud:ProductName"], "MADai");
	private string MADCloudApiUrl => FirstNonBlank(_configuration["MADCLOUD_API_URL"], _configuration["MADCloud:ApiUrl"], "https://madcloudapi.madprospects.com").TrimEnd('/');
	private string MADCloudSecret => FirstNonBlank(_configuration["MADCLOUD_APP_SECRET"], _configuration["MADCloud:AppSecret"]);
	private string CallbackUrl => FirstNonBlank(_configuration["MADCLOUD_CALLBACK_URL"], _configuration["MADCloud:CallbackUrl"], $"{Request.Scheme}://{Request.Host}/api/madcloud/ai-results");
	private string AiRoute => FirstNonBlank(_configuration["MADCLOUD_AI_ROUTE"], _configuration["MADCloud:AiRoute"], "/ai");
	private bool IsConfigured => !string.IsNullOrWhiteSpace(SourceApp) && !string.IsNullOrWhiteSpace(MADCloudSecret);

	private static string Sha256Hex(string value) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value))).ToLowerInvariant();
	private static string HmacHex(string value, string secret) => Convert.ToHexString(new HMACSHA256(Encoding.UTF8.GetBytes(secret)).ComputeHash(Encoding.UTF8.GetBytes(value))).ToLowerInvariant();
	private static string FirstNonBlank(params string?[] values) => values.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value)) ?? string.Empty;
	private static string? Truncate(string? value, int max) => value is null || value.Length <= max ? value : value[..max];

	public sealed record MADCloudConfigResponse(string SourceApp, string ProductName, string ApiUrl, string AiRoute, string CallbackUrl, string SubmitPath, string CallbackPath, bool Configured);
	public sealed record MADCloudSubmitRequest(string? CorrelationId, string? IdempotencyKey, string? RequestKind, string? Capability, int? Priority, string? TargetApplicationSlug, string? Title, string Prompt, string? SystemPrompt, JsonElement? Input, JsonElement? ExpectedOutputSchema, JsonElement? Attachments, JsonElement? Metadata);
	private sealed record MADCloudRequestEnvelope(string SourceApp, string? SourceEnvironment, string? CorrelationId, string? IdempotencyKey, string? CallbackUrl, string RequestKind, string? Capability, int? Priority, string? TargetApplicationSlug, string? Title, string Prompt, string? SystemPrompt, JsonElement? Input, JsonElement? ExpectedOutputSchema, JsonElement? Attachments, JsonElement? Metadata);
}
