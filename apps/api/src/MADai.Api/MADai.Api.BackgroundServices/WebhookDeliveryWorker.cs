using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Domain.SystemEntities;
using MADai.Domain.Webhooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MADai.Api.BackgroundServices;

/// <summary>
/// Polls the WebhookEvents outbox and delivers Pending rows to their endpoints.
/// Signs each POST with HMAC-SHA256 of the body using the endpoint's secret.
/// Retries with exponential backoff up to 5 attempts; abandons after that.
/// </summary>
public class WebhookDeliveryWorker : BackgroundService
{
	private static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(10.0);

	private const int MaxAttempts = 5;

	private const int BatchSize = 20;

	private readonly IServiceScopeFactory _scopeFactory;

	private readonly IHttpClientFactory _httpFactory;

	private readonly ILogger<WebhookDeliveryWorker> _logger;

	public WebhookDeliveryWorker(IServiceScopeFactory scopeFactory, IHttpClientFactory httpFactory, ILogger<WebhookDeliveryWorker> logger)
	{
		_scopeFactory = scopeFactory;
		_httpFactory = httpFactory;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("WebhookDeliveryWorker started (poll {Interval}, max {Max} attempts)", PollInterval, 5);
		while (!stoppingToken.IsCancellationRequested)
		{
			try
			{
				await PollOnceAsync(stoppingToken);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "WebhookDeliveryWorker iteration failed");
			}
			try
			{
				await Task.Delay(PollInterval, stoppingToken);
			}
			catch (TaskCanceledException)
			{
				break;
			}
		}
	}

	private async Task PollOnceAsync(CancellationToken cancellationToken)
	{
		using IServiceScope scope = _scopeFactory.CreateScope();
		IDbContextAccess db = scope.ServiceProvider.GetRequiredService<IDbContextAccess>();
		IDateTimeProvider clock = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();
		DateTime now = clock.UtcNow;
		List<WebhookEvent> due = await (from e in db.WebhookEvents
			where e.Status == "Pending" && (e.NextAttemptAt == null || e.NextAttemptAt <= now)
			orderby e.CreatedAt
			select e).Take(20).ToListAsync(cancellationToken);
		if (due.Count == 0)
		{
			return;
		}
		List<Guid> endpointIds = due.Select((WebhookEvent d) => d.EndpointId).Distinct().ToList();
		Dictionary<Guid, WebhookEndpoint> endpoints = await db.WebhookEndpoints.Where((WebhookEndpoint e) => endpointIds.Contains(e.Id)).ToDictionaryAsync((WebhookEndpoint e) => e.Id, cancellationToken);
		foreach (WebhookEvent ev in due)
		{
			if (!endpoints.TryGetValue(ev.EndpointId, out var endpoint) || !endpoint.IsActive)
			{
				ev.Status = "Abandoned";
				ev.LastError = "Endpoint missing or inactive at delivery time.";
				continue;
			}
			ev.AttemptCount++;
			try
			{
				using HttpClient http = _httpFactory.CreateClient("webhook");
				using HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, endpoint.Url)
				{
					Content = new StringContent(ev.PayloadJson, Encoding.UTF8, "application/json")
				};
				msg.Headers.Add("X-MADai-Event", ev.EventType);
				msg.Headers.Add("X-MADai-Event-Id", ev.Id.ToString("N"));
				msg.Headers.Add("X-MADai-Attempt", ev.AttemptCount.ToString());
				msg.Headers.Add("X-MADai-Signature", "sha256=" + Sign(ev.PayloadJson, endpoint.Secret));
				msg.Headers.UserAgent.Add(new ProductInfoHeaderValue("MADai-Webhooks", "1.0"));
				using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
				cts.CancelAfter(TimeSpan.FromSeconds(15.0));
				HttpResponseMessage resp = await http.SendAsync(msg, cts.Token);
				ev.LastResponseStatus = (int)resp.StatusCode;
				if (resp.IsSuccessStatusCode)
				{
					ev.Status = "Delivered";
					ev.DeliveredAt = now;
					endpoint.LastSuccessAt = now;
					endpoint.ConsecutiveFailures = 0;
				}
				else
				{
					ev.LastError = $"HTTP {resp.StatusCode} {resp.ReasonPhrase}";
					OnFailure(ev, endpoint, now);
				}
			}
			catch (Exception ex)
			{
				ev.LastError = ex.GetType().Name + ": " + ex.Message;
				OnFailure(ev, endpoint, now);
			}
			endpoint = null;
		}
		await db.SaveChangesAsync(cancellationToken);
	}

	private static void OnFailure(WebhookEvent ev, WebhookEndpoint endpoint, DateTime now)
	{
		endpoint.LastFailureAt = now;
		endpoint.ConsecutiveFailures++;
		if (ev.AttemptCount >= 5)
		{
			ev.Status = "Failed";
			return;
		}
		ev.NextAttemptAt = now.Add(ev.AttemptCount switch
		{
			1 => TimeSpan.FromMinutes(1.0), 
			2 => TimeSpan.FromMinutes(2.0), 
			3 => TimeSpan.FromMinutes(5.0), 
			4 => TimeSpan.FromMinutes(15.0), 
			_ => TimeSpan.FromMinutes(60.0), 
		});
	}

	private static string Sign(string body, string secret)
	{
		using HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
		byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(body));
		return Convert.ToHexString(hash).ToLowerInvariant();
	}
}
