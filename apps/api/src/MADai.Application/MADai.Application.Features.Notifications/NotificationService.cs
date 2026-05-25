using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Domain.Identity;
using MADai.Domain.Notifications;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.Notifications;

public class NotificationService : INotificationService
{
	private readonly IDbContextAccess _db;

	private readonly ICurrentUserService _currentUser;

	private readonly IDateTimeProvider _clock;

	private readonly IEventPublisher _publisher;

	private readonly IEmailSender _email;

	public NotificationService(IDbContextAccess db, ICurrentUserService currentUser, IDateTimeProvider clock, IEventPublisher publisher, IEmailSender email)
	{
		_db = db;
		_currentUser = currentUser;
		_clock = clock;
		_publisher = publisher;
		_email = email;
	}

	public async Task<IReadOnlyList<NotificationDto>> ListForCurrentUserAsync(int take, CancellationToken cancellationToken = default(CancellationToken))
	{
		Guid? userId = _currentUser.UserId;
		Guid? companyId = _currentUser.CompanyId;
		take = Math.Clamp(take, 1, 500);
		return await (from n in (from n in _db.Notifications.AsNoTracking()
				where n.UserId == userId || (n.UserId == null && (companyId == null || n.CompanyId == null || n.CompanyId == companyId))
				orderby n.CreatedAt descending
				select n).Take(take)
			select new NotificationDto(n.Id, n.UserId, n.CompanyId, n.Channel, n.Severity, n.Title, n.Body, n.Url, n.CreatedAt, n.ReadAt, n.DismissedAt, n.Tags)).ToListAsync(cancellationToken);
	}

	public async Task MarkReadAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		Notification i = (await _db.Notifications.FirstOrDefaultAsync((Notification x) => x.Id == id, cancellationToken)) ?? throw new NotFoundException("Notification", id);
		if (i.UserId != _currentUser.UserId && i.UserId.HasValue && !_currentUser.IsInRole("SystemAdmin"))
		{
			throw new ForbiddenException();
		}
		if (!i.ReadAt.HasValue)
		{
			i.ReadAt = _clock.UtcNow;
			await _db.SaveChangesAsync(cancellationToken);
		}
	}

	public async Task MarkAllReadAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		Guid? userId = _currentUser.UserId;
		List<Notification> unread = await _db.Notifications.Where((Notification n) => n.UserId == userId && n.ReadAt == null).ToListAsync(cancellationToken);
		foreach (Notification item in unread)
		{
			item.ReadAt = _clock.UtcNow;
		}
		if (unread.Count > 0)
		{
			await _db.SaveChangesAsync(cancellationToken);
		}
	}

	public async Task DismissAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		Notification obj = (await _db.Notifications.FirstOrDefaultAsync((Notification x) => x.Id == id, cancellationToken)) ?? throw new NotFoundException("Notification", id);
		if (obj.UserId != _currentUser.UserId && !_currentUser.IsInRole("SystemAdmin"))
		{
			throw new ForbiddenException();
		}
		obj.DismissedAt = _clock.UtcNow;
		await _db.SaveChangesAsync(cancellationToken);
	}

	public async Task<NotificationDto> SendAsync(CreateNotificationRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		Notification entity = new Notification
		{
			UserId = request.UserId,
			CompanyId = request.CompanyId,
			Channel = (request.Channel ?? "InApp"),
			Severity = (request.Severity ?? "Info"),
			Title = request.Title,
			Body = request.Body,
			Url = request.Url,
			Tags = request.Tags,
			CreatedAt = _clock.UtcNow
		};
		_db.Notifications.Add(entity);
		await _db.SaveChangesAsync(cancellationToken);
		await _publisher.PublishNotificationAsync(entity.UserId, new { entity.Id, entity.Title, entity.Body, entity.Severity, entity.CreatedAt }, cancellationToken);
		if (entity.Channel == "Email")
		{
			Guid? userId = entity.UserId;
			if (userId.HasValue)
			{
				Guid uid = userId.GetValueOrDefault();
				string email = await (from u in _db.Users
					where u.Id == uid
					select u.Email).FirstOrDefaultAsync(cancellationToken);
				if (!string.IsNullOrWhiteSpace(email))
				{
					try
					{
						await _email.SendAsync(email, entity.Title, "<p>" + entity.Body + "</p>", cancellationToken);
					}
					catch
					{
					}
				}
			}
		}
		return new NotificationDto(entity.Id, entity.UserId, entity.CompanyId, entity.Channel, entity.Severity, entity.Title, entity.Body, entity.Url, entity.CreatedAt, entity.ReadAt, entity.DismissedAt, entity.Tags);
	}

	public async Task<IReadOnlyList<NotificationPreferenceDto>> GetPreferencesAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		Guid userId = _currentUser.UserId ?? throw new ForbiddenException();
		return await (from p in _db.NotificationPreferences.AsNoTracking()
			where p.UserId == userId
			orderby p.Category
			select new NotificationPreferenceDto(p.Category, p.Email, p.InApp, p.Push, p.Webhook)).ToListAsync(cancellationToken);
	}

	public async Task<NotificationPreferenceDto> UpsertPreferenceAsync(UpsertNotificationPreferenceRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		UpsertNotificationPreferenceRequest request2 = request;
		Guid userId = _currentUser.UserId ?? throw new ForbiddenException();
		NotificationPreference existing = await _db.NotificationPreferences.FirstOrDefaultAsync((NotificationPreference p) => p.UserId == userId && p.Category == request2.Category, cancellationToken);
		if (existing == null)
		{
			existing = new NotificationPreference
			{
				UserId = userId,
				Category = request2.Category,
				Email = request2.Email,
				InApp = request2.InApp,
				Push = request2.Push,
				Webhook = request2.Webhook
			};
			_db.NotificationPreferences.Add(existing);
		}
		else
		{
			existing.Email = request2.Email;
			existing.InApp = request2.InApp;
			existing.Push = request2.Push;
			existing.Webhook = request2.Webhook;
		}
		await _db.SaveChangesAsync(cancellationToken);
		return new NotificationPreferenceDto(existing.Category, existing.Email, existing.InApp, existing.Push, existing.Webhook);
	}
}
