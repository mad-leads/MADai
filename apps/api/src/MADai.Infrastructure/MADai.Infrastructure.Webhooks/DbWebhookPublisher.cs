using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Application.Features.Webhooks;
using MADai.Domain.SystemEntities;
using MADai.Domain.Webhooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MADai.Infrastructure.Webhooks;

public class DbWebhookPublisher : IWebhookPublisher
{
	private readonly IDbContextAccess _db;

	private readonly IDateTimeProvider _clock;

	private readonly ILogger<DbWebhookPublisher> _logger;

	public DbWebhookPublisher(IDbContextAccess db, IDateTimeProvider clock, ILogger<DbWebhookPublisher> logger)
	{
		_db = db;
		_clock = clock;
		_logger = logger;
	}

	public async Task PublishAsync(Guid companyId, string eventType, object payload, CancellationToken cancellationToken = default(CancellationToken))
	{
		string eventType2 = eventType;
		var endpoints = await (from e in _db.WebhookEndpoints.AsNoTracking()
			where e.CompanyId == companyId && e.IsActive
			select new { e.Id, e.EventsCsv }).ToListAsync(cancellationToken);
		if (endpoints.Count == 0)
		{
			return;
		}
		string json = JsonSerializer.Serialize(new
		{
			type = eventType2,
			companyId = companyId,
			occurredAt = _clock.UtcNow,
			data = payload
		});
		var matching = endpoints.Where(e => string.IsNullOrWhiteSpace(e.EventsCsv) || e.EventsCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Any((string t) => string.Equals(t, eventType2, StringComparison.OrdinalIgnoreCase) || (t.EndsWith('*') && eventType2.StartsWith(t.TrimEnd('*'), StringComparison.OrdinalIgnoreCase)))).ToList();
		foreach (var e2 in matching)
		{
			_db.WebhookEvents.Add(new WebhookEvent
			{
				CompanyId = companyId,
				EndpointId = e2.Id,
				EventType = eventType2,
				PayloadJson = json,
				CreatedAt = _clock.UtcNow,
				NextAttemptAt = _clock.UtcNow,
				Status = "Pending"
			});
		}
		if (matching.Count > 0)
		{
			await _db.SaveChangesAsync(cancellationToken);
			_logger.LogDebug("Enqueued {Count} webhook events for {EventType} in company {Company}", matching.Count, eventType2, companyId);
		}
	}
}
