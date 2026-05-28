using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MADai.Application.Abstractions;
using MADai.Domain.Enums;
using MADai.Domain.Tasks;
using MADai.Shared.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MADai.Api.Controllers;

[ApiController]
[Authorize]
[ApiVersion("1.0")]
public sealed class SubscriberAiController : ControllerBase
{
	private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
	{
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
	};

	private static readonly IReadOnlyList<AiToolDto> Tools =
	[
		Tool("blog-writer", "Blog Writer", "Writing", "Turn a topic into a structured article with title ideas, outline, draft, and editing notes.", TaskCategory.BlogWriting, "Write a practical article about this topic for a business audience.", ["topic", "audience", "tone"], ["Founder thought leadership", "SEO article", "Newsletter draft"]),
		Tool("book-builder", "Book Builder", "Writing", "Create outlines, chapter drafts, back-cover copy, and publishing assets.", TaskCategory.BookGeneration, "Develop this book idea into an outline and first chapter.", ["book idea", "genre", "reader"], ["Non-fiction outline", "Chapter draft", "KDP metadata"]),
		Tool("social-studio", "Social Studio", "Writing", "Generate campaign posts, hooks, captions, and content calendars.", TaskCategory.SocialContent, "Create a social campaign for this offer.", ["offer", "platform", "voice"], ["LinkedIn launch", "Instagram captions", "30-day calendar"]),
		Tool("doc-assistant", "Documentation Assistant", "Writing", "Produce manuals, SOPs, release notes, and internal documentation.", TaskCategory.Documentation, "Create clear documentation for this process.", ["process", "audience", "format"], ["User manual", "SOP", "Release notes"]),
		Tool("report-builder", "Report Builder", "Writing", "Create executive reports, summaries, data narratives, and recommendations.", TaskCategory.Reporting, "Turn this information into a polished report.", ["data", "goal", "stakeholders"], ["Executive summary", "Board report", "Weekly status"]),
		Tool("image-brief", "Image Brief Studio", "Creative", "Create image prompts, art direction, and production-ready creative briefs.", TaskCategory.ImageGeneration, "Create an image generation brief for this concept.", ["concept", "style", "dimensions"], ["Ad visual", "Hero image", "Product mockup"]),
		Tool("video-planner", "Video Planner", "Creative", "Create video scripts, shot lists, storyboards, and production prompts.", TaskCategory.VideoGeneration, "Create a video plan for this campaign.", ["campaign", "duration", "channel"], ["Explainer video", "Short-form ad", "Storyboard"]),
		Tool("ux-ui-studio", "UX/UI Studio", "Creative", "Shape product screens, UX flows, content hierarchy, and UI polish tasks.", TaskCategory.UxUi, "Design a user experience for this product moment.", ["feature", "user", "desired action"], ["Dashboard concept", "Onboarding flow", "Conversion polish"]),
		Tool("research-hub", "Research Hub", "Intelligence", "Collect research, compare options, summarize findings, and recommend next steps.", TaskCategory.Research, "Research this topic and return a concise recommendation.", ["question", "constraints", "decision"], ["Market scan", "Vendor comparison", "Competitor research"]),
		Tool("data-analyst", "Data Analyst", "Intelligence", "Analyze pasted data, spot patterns, and produce conclusions.", TaskCategory.Analysis, "Analyze this dataset or description and explain the key findings.", ["data", "metric", "question"], ["Sales analysis", "Trend summary", "Risk review"]),
		Tool("scraping-planner", "Web Research Agent", "Intelligence", "Plan scraping and web data extraction tasks with compliance notes.", TaskCategory.Scraping, "Collect structured public information for this request.", ["source", "fields", "use case"], ["Directory extraction", "Pricing monitor", "Content audit"]),
		Tool("transcription-assistant", "Transcription Assistant", "Intelligence", "Convert audio/video notes into summaries, actions, and searchable records.", TaskCategory.Transcription, "Transcribe and summarize this recording context.", ["recording context", "speaker notes", "output format"], ["Meeting notes", "Interview summary", "Action list"]),
		Tool("automation-builder", "Automation Builder", "Automation", "Design repeatable automations, checklists, and handoff workflows.", TaskCategory.Automation, "Design an automation for this process.", ["process", "trigger", "systems"], ["Follow-up workflow", "Ops checklist", "Data cleanup"]),
		Tool("integration-architect", "Integration Architect", "Automation", "Plan API integrations, mappings, webhooks, and cross-app handoffs.", TaskCategory.IntegrationBuilding, "Design an integration for these systems.", ["source app", "target app", "data"], ["Webhook plan", "API mapping", "MAD Universe handoff"]),
		Tool("sql-helper", "SQL Helper", "Automation", "Draft SQL, data checks, migrations, and reporting queries.", TaskCategory.SqlGeneration, "Create SQL for this data task.", ["tables", "goal", "database"], ["Report query", "Data cleanup", "Migration review"]),
		Tool("dotnet-helper", ".NET Helper", "Automation", "Plan or generate .NET service, API, and backend work.", TaskCategory.DotNetGeneration, "Help build this .NET backend change.", ["feature", "constraints", "acceptance"], ["API endpoint", "Service design", "Bug investigation"]),
		Tool("angular-helper", "Angular Helper", "Automation", "Plan or generate Angular components, pages, forms, and UI polish.", TaskCategory.AngularGeneration, "Help build this Angular frontend change.", ["screen", "user flow", "style"], ["Form screen", "Dashboard panel", "Responsive polish"]),
		Tool("debugging-lab", "Debugging Lab", "Automation", "Turn symptoms, logs, and screenshots into a diagnosis and fix plan.", TaskCategory.Debugging, "Diagnose this issue and propose the fix.", ["symptom", "logs", "expected behavior"], ["Error triage", "Regression review", "Root cause"]),
		Tool("test-planner", "Test Planner", "Automation", "Create test plans, QA scenarios, and verification checklists.", TaskCategory.Testing, "Create a test plan for this change.", ["feature", "risks", "platform"], ["Acceptance tests", "Regression suite", "Browser QA"]),
		Tool("deployment-guide", "Deployment Guide", "Automation", "Prepare deploy plans, smoke checks, rollback notes, and evidence packs.", TaskCategory.Deployment, "Create a deployment plan for this release.", ["release", "environment", "risks"], ["Smoke checklist", "Rollback plan", "Evidence pack"])
	];

	private readonly IConfiguration _configuration;
	private readonly IDbContextAccess _db;
	private readonly IHostEnvironment _environment;
	private readonly IHttpClientFactory _httpClientFactory;

	public SubscriberAiController(IConfiguration configuration, IDbContextAccess db, IHostEnvironment environment, IHttpClientFactory httpClientFactory)
	{
		_configuration = configuration;
		_db = db;
		_environment = environment;
		_httpClientFactory = httpClientFactory;
	}

	[HttpGet("api/v{version:apiVersion}/ai-tools")]
	public ActionResult<ApiResult<IReadOnlyList<AiToolDto>>> ToolsCatalog()
	{
		return Ok(ApiResult<IReadOnlyList<AiToolDto>>.Ok(Tools));
	}

	[HttpPost("api/v{version:apiVersion}/ai-requests")]
	public async Task<ActionResult<ApiResult<AiRequestResponse>>> CreateAiRequest([FromBody] CreateAiRequestDto request, CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(request.Prompt))
		{
			return BadRequest(ApiResult<AiRequestResponse>.Fail("Prompt is required."));
		}

		var tool = Tools.FirstOrDefault(item => item.Key.Equals(request.ToolKey, StringComparison.OrdinalIgnoreCase)) ?? Tools.First();
		var title = string.IsNullOrWhiteSpace(request.Title) ? $"{tool.Name}: {request.Prompt.Trim()[..Math.Min(request.Prompt.Trim().Length, 80)]}" : request.Title.Trim();
		var inputPayload = JsonSerializer.Serialize(new
		{
			tool = tool.Key,
			tool.Name,
			inputs = request.Inputs,
			attachments = request.Attachments,
			templateId = request.TemplateId
		}, JsonOptions);

		var task = new TaskItem
		{
			Title = title,
			Description = BuildDescription(tool, request),
			Category = tool.TaskCategory,
			Priority = request.Priority ?? TaskPriority.Normal,
			Origin = TaskOrigin.User,
			QueueName = "madcloud",
			PromptPayload = BuildPrompt(tool, request),
			InputPayload = inputPayload,
			TimeoutSeconds = 3600,
			MaxRetries = 3,
			TemplateId = request.TemplateId,
			CompanyId = CurrentCompanyId(),
			Status = MADai.Domain.Enums.TaskStatus.Queued,
			Tags = $"madai-ai,tool:{tool.Key},category:{tool.Group.ToLowerInvariant()}"
		};
		_db.Tasks.Add(task);
		await _db.SaveChangesAsync(cancellationToken);

		var provider = await SubmitToMadCloudAsync(task, tool, request, cancellationToken);
		return Ok(ApiResult<AiRequestResponse>.Ok(new AiRequestResponse(task.Id, task.Title, task.Status.ToString(), tool.Key, provider.Submitted, provider.Message)));
	}

	[HttpGet("api/v{version:apiVersion}/me/usage")]
	public async Task<ActionResult<ApiResult<UsageDto>>> Usage(CancellationToken cancellationToken)
	{
		var companyId = CurrentCompanyId();
		var now = DateTime.UtcNow;
		var monthStart = new DateTime(now.Year, now.Month, 1);
		var tasksThisMonth = await _db.Tasks.AsNoTracking().CountAsync(task => task.CompanyId == companyId && task.CreatedDate >= monthStart, cancellationToken);
		var completedThisMonth = await _db.Tasks.AsNoTracking().CountAsync(task => task.CompanyId == companyId && task.CreatedDate >= monthStart && task.Status == MADai.Domain.Enums.TaskStatus.Completed, cancellationToken);
		var files = await _db.Files.AsNoTracking().Where(file => file.CompanyId == companyId).SumAsync(file => (long?)file.SizeBytes, cancellationToken) ?? 0;
		return Ok(ApiResult<UsageDto>.Ok(new UsageDto("Starter", tasksThisMonth, completedThisMonth, 25, Math.Round(files / 1024d / 1024d, 2), false, "Soft launch access is active. Upgrade when your team needs more capacity.")));
	}

	[HttpGet("api/v{version:apiVersion}/outputs")]
	public async Task<ActionResult<ApiResult<IReadOnlyList<OutputDto>>>> Outputs([FromQuery] int take = 80, CancellationToken cancellationToken = default)
	{
		var companyId = CurrentCompanyId();
		var outputs = await _db.Tasks.AsNoTracking()
			.Include(task => task.Artifacts)
			.Where(task => task.CompanyId == companyId && (task.Status == MADai.Domain.Enums.TaskStatus.Completed || task.OutputSummary != null || task.ResultPayload != null || task.Artifacts.Any()))
			.OrderByDescending(task => task.CompletedAt ?? task.CreatedDate)
			.Take(Math.Clamp(take, 1, 200))
			.Select(task => new OutputDto(
				task.Id,
				task.Title,
				task.Category.ToString(),
				task.Status.ToString(),
				task.OutputSummary,
				task.ResultPayload,
				task.CompletedAt ?? task.CreatedDate,
				task.Artifacts.Select(artifact => new OutputArtifactDto(artifact.Id, artifact.FileName, artifact.ContentType, artifact.Kind, artifact.SizeBytes, artifact.PreviewUrl)).ToList()))
			.ToListAsync(cancellationToken);
		return Ok(ApiResult<IReadOnlyList<OutputDto>>.Ok(outputs));
	}

	private async Task<(bool Submitted, string Message)> SubmitToMadCloudAsync(TaskItem task, AiToolDto tool, CreateAiRequestDto request, CancellationToken cancellationToken)
	{
		if (!MadCloudConfigured)
		{
			return (false, "Your request is saved. MADai will process it when AI execution is configured.");
		}

		var path = "/api/ai/requests";
		var body = JsonSerializer.Serialize(new
		{
			sourceApp = SourceApp,
			sourceEnvironment = _environment.EnvironmentName,
			correlationId = task.Id.ToString(),
			idempotencyKey = $"madai-task-{task.Id}",
			callbackUrl = CallbackUrl,
			requestKind = "Text",
			capability = tool.Key,
			priority = (int)(request.Priority ?? TaskPriority.Normal),
			targetApplicationSlug = "madai",
			title = task.Title,
			prompt = BuildPrompt(tool, request),
			input = request.Inputs,
			attachments = request.Attachments,
			metadata = new { taskId = task.Id, tool = tool.Key, product = "MADai" }
		}, JsonOptions);

		using var httpRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(MadCloudApiUrl), path))
		{
			Content = new StringContent(body, Encoding.UTF8, "application/json")
		};
		Sign(httpRequest, "POST", path, body);
		httpRequest.Headers.TryAddWithoutValidation("Idempotency-Key", $"madai-task-{task.Id}");

		try
		{
			using var response = await _httpClientFactory.CreateClient("madcloud").SendAsync(httpRequest, cancellationToken);
			return response.IsSuccessStatusCode
				? (true, "Your request is running in MADai AI.")
				: (false, $"Your request is saved, but AI execution returned {(int)response.StatusCode}.");
		}
		catch (Exception ex)
		{
			return (false, $"Your request is saved. AI execution will need retry: {ex.Message}");
		}
	}

	private Guid CurrentCompanyId()
	{
		var claim = User.FindFirst("company_id")?.Value;
		if (Guid.TryParse(claim, out var companyId))
		{
			return companyId;
		}

		throw new UnauthorizedAccessException("Company context required.");
	}

	private void Sign(HttpRequestMessage request, string method, string path, string body)
	{
		var timestamp = DateTimeOffset.UtcNow.ToString("O");
		var nonce = Guid.NewGuid().ToString("N");
		var payload = $"{method.ToUpperInvariant()}\n{path}\n{timestamp}\n{nonce}\n{Sha256Hex(body)}";
		request.Headers.TryAddWithoutValidation("X-MADCloud-App", SourceApp);
		request.Headers.TryAddWithoutValidation("X-MADCloud-Timestamp", timestamp);
		request.Headers.TryAddWithoutValidation("X-MADCloud-Nonce", nonce);
		request.Headers.TryAddWithoutValidation("X-MADCloud-Signature", HmacHex(payload, MadCloudSecret));
	}

	private static string BuildDescription(AiToolDto tool, CreateAiRequestDto request) => $"MADai AI request created from {tool.Name}. {request.Prompt.Trim()}";
	private static string BuildPrompt(AiToolDto tool, CreateAiRequestDto request) => $"{tool.SystemPrompt}\n\nUser request:\n{request.Prompt.Trim()}\n\nInputs:\n{JsonSerializer.Serialize(request.Inputs, JsonOptions)}";
	private static AiToolDto Tool(string key, string name, string group, string description, TaskCategory taskCategory, string systemPrompt, IReadOnlyList<string> fields, IReadOnlyList<string> examples) => new(key, name, group, description, taskCategory, systemPrompt, fields, examples);
	private string SourceApp => FirstNonBlank(_configuration["MADCLOUD_APP_ID"], _configuration["MADCloud:AppId"], "madai");
	private string MadCloudApiUrl => FirstNonBlank(_configuration["MADCLOUD_API_URL"], _configuration["MADCloud:ApiUrl"], "https://madcloudapi.madprospects.com").TrimEnd('/');
	private string MadCloudSecret => FirstNonBlank(_configuration["MADCLOUD_APP_SECRET"], _configuration["MADCloud:AppSecret"]);
	private string CallbackUrl => FirstNonBlank(_configuration["MADCLOUD_CALLBACK_URL"], _configuration["MADCloud:CallbackUrl"], $"{Request.Scheme}://{Request.Host}/api/madcloud/ai-results");
	private bool MadCloudConfigured => !string.IsNullOrWhiteSpace(SourceApp) && !string.IsNullOrWhiteSpace(MadCloudSecret);
	private static string Sha256Hex(string value) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value))).ToLowerInvariant();
	private static string HmacHex(string value, string secret) => Convert.ToHexString(new HMACSHA256(Encoding.UTF8.GetBytes(secret)).ComputeHash(Encoding.UTF8.GetBytes(value))).ToLowerInvariant();
	private static string FirstNonBlank(params string?[] values) => values.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value)) ?? string.Empty;

	public sealed record AiToolDto(string Key, string Name, string Group, string Description, TaskCategory TaskCategory, string SystemPrompt, IReadOnlyList<string> Fields, IReadOnlyList<string> Examples);
	public sealed record CreateAiRequestDto(string ToolKey, string? Title, string Prompt, JsonElement? Inputs, JsonElement? Attachments, Guid? TemplateId, TaskPriority? Priority);
	public sealed record AiRequestResponse(Guid TaskId, string Title, string Status, string ToolKey, bool ProviderSubmitted, string ProviderMessage);
	public sealed record UsageDto(string PlanName, int TasksThisMonth, int CompletedThisMonth, int IncludedTasks, double StorageUsedMb, bool UpgradeRecommended, string Message);
	public sealed record OutputDto(Guid TaskId, string Title, string Category, string Status, string? Summary, string? Result, DateTime CreatedOrCompletedAt, IReadOnlyList<OutputArtifactDto> Artifacts);
	public sealed record OutputArtifactDto(Guid Id, string FileName, string ContentType, string? Kind, long SizeBytes, string? PreviewUrl);
}
