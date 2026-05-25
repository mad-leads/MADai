using System;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MADai.Infrastructure.Persistence.Interceptors;

public class AuditingInterceptor : SaveChangesInterceptor
{
	private readonly ICurrentUserService _currentUserService;

	public AuditingInterceptor(ICurrentUserService currentUserService)
	{
		_currentUserService = currentUserService;
	}

	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		Apply(eventData.Context);
		return base.SavingChanges(eventData, result);
	}

	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default(CancellationToken))
	{
		Apply(eventData.Context);
		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	private void Apply(DbContext? context)
	{
		if (context == null)
		{
			return;
		}
		DateTime now = DateTime.UtcNow;
		Guid? userId = _currentUserService.UserId;
		foreach (EntityEntry<AuditableEntity> entry in context.ChangeTracker.Entries<AuditableEntity>())
		{
			switch (entry.State)
			{
			case EntityState.Added:
				entry.Entity.CreatedDate = ((entry.Entity.CreatedDate == default(DateTime)) ? now : entry.Entity.CreatedDate);
				entry.Entity.CreatedByUserId = entry.Entity.CreatedByUserId ?? userId;
				break;
			case EntityState.Modified:
				entry.Entity.ModifiedDate = now;
				entry.Entity.ModifiedByUserId = userId;
				break;
			case EntityState.Deleted:
				entry.State = EntityState.Modified;
				entry.Entity.IsDeleted = true;
				entry.Entity.DeletedDate = now;
				entry.Entity.DeletedByUserId = userId;
				break;
			}
		}
	}
}
