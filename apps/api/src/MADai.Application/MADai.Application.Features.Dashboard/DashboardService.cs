using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Domain.Tasks;
using MADai.Domain.Workers;
using MADai.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.Dashboard;

public class DashboardService : IDashboardService
{
	private readonly IDbContextAccess _db;

	private readonly ICurrentUserService _currentUser;

	private readonly IDateTimeProvider _clock;

	public DashboardService(IDbContextAccess db, ICurrentUserService currentUser, IDateTimeProvider clock)
	{
		_db = db;
		_currentUser = currentUser;
		_clock = clock;
	}

	public async Task<SystemOverviewDto> GetOverviewAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		IQueryable<TaskItem> tasks = _db.Tasks.AsNoTracking();
		Guid? companyId = _currentUser.CompanyId;
		if (companyId.HasValue)
		{
			Guid cid = companyId.GetValueOrDefault();
			tasks = tasks.Where((TaskItem t) => t.CompanyId == cid);
		}
		DateTime today = _clock.UtcNow.Date;
		long total = await tasks.LongCountAsync(cancellationToken);
		long queued = await tasks.LongCountAsync((TaskItem t) => (int)t.Status == 10, cancellationToken);
		long running = await tasks.LongCountAsync((TaskItem t) => (int)t.Status == 30, cancellationToken);
		long completedToday = await tasks.LongCountAsync((TaskItem t) => t.CompletedAt != null && t.CompletedAt >= today && (int)t.Status == 50, cancellationToken);
		long failedToday = await tasks.LongCountAsync((TaskItem t) => (int)t.Status == 60 && t.CompletedAt != null && t.CompletedAt >= today, cancellationToken);
		long dlq = await tasks.LongCountAsync((TaskItem t) => t.IsDeadLetter, cancellationToken);
		IQueryable<WorkerNode> workers = _db.WorkerNodes.AsNoTracking();
		companyId = _currentUser.CompanyId;
		if (companyId.HasValue)
		{
			Guid cid2 = companyId.GetValueOrDefault();
			workers = workers.Where((WorkerNode w) => w.CompanyId == null || w.CompanyId == cid2);
		}
		int active = await workers.CountAsync((WorkerNode w) => (int)w.Status == 30 && w.IsActive, cancellationToken);
		int idle = await workers.CountAsync((WorkerNode w) => (int)w.Status == 20 && w.IsActive, cancellationToken);
		int offline = await workers.CountAsync((WorkerNode w) => (int)w.Status == 0 || !w.IsActive, cancellationToken);
		DateTime since = today.AddDays(-7.0);
		var completedRecords = await (from t in tasks
			where (int)t.Status == 50 && t.CompletedAt != null && t.StartedAt != null && t.CompletedAt >= since
			select new { t.StartedAt, t.CompletedAt }).ToListAsync(cancellationToken);
		double avgDuration = ((completedRecords.Count == 0) ? 0.0 : completedRecords.Average(r => (r.CompletedAt.Value - r.StartedAt.Value).TotalSeconds));
		long doneOrFailed = await tasks.LongCountAsync((TaskItem t) => (int)t.Status == 50 || (int)t.Status == 60, cancellationToken);
		long doneOnly = await tasks.LongCountAsync((TaskItem t) => (int)t.Status == 50, cancellationToken);
		double successRate = ((doneOrFailed == 0L) ? 0.0 : Math.Round((double)doneOnly / (double)doneOrFailed * 100.0, 2));
		return new SystemOverviewDto(total, queued, running, completedToday, failedToday, dlq, active, idle, offline, avgDuration, successRate);
	}

	public async Task<IReadOnlyList<QueueHealthDto>> GetQueueHealthAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		IQueryable<TaskItem> tasks = _db.Tasks.AsNoTracking();
		Guid? companyId = _currentUser.CompanyId;
		if (companyId.HasValue)
		{
			Guid cid = companyId.GetValueOrDefault();
			tasks = tasks.Where((TaskItem t) => t.CompanyId == cid);
		}
		return await (from t in tasks
			group t by t.QueueName ?? "default" into g
			select new QueueHealthDto(g.Key, g.Count((TaskItem t) => (int)t.Status == 10), g.Count((TaskItem t) => (int)t.Status == 30), g.Count((TaskItem t) => (int)t.Status == 60 || t.IsDeadLetter), 0.0)).ToListAsync(cancellationToken);
	}

	public async Task<IReadOnlyList<WorkerHealthDto>> GetWorkerHealthAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		IQueryable<WorkerNode> query = _db.WorkerNodes.AsNoTracking().AsQueryable();
		Guid? companyId = _currentUser.CompanyId;
		if (companyId.HasValue)
		{
			Guid cid = companyId.GetValueOrDefault();
			query = query.Where((WorkerNode w) => w.CompanyId == null || w.CompanyId == cid);
		}
		return await query.Select((WorkerNode w) => new WorkerHealthDto(w.Id, w.Name, w.Status.ToString(), w.CurrentConcurrency, w.MaxConcurrency, w.LastHeartbeatAt, (from h in w.Heartbeats
			orderby h.Timestamp descending
			select h.CpuPercent).FirstOrDefault(), (from h in w.Heartbeats
			orderby h.Timestamp descending
			select h.MemoryMb).FirstOrDefault(), (from h in w.Heartbeats
			orderby h.Timestamp descending
			select h.DiskFreeGb).FirstOrDefault())).ToListAsync(cancellationToken);
	}

	public async Task<IReadOnlyList<FailureTrendPointDto>> GetFailureTrendAsync(int days, CancellationToken cancellationToken = default(CancellationToken))
	{
		days = Math.Clamp(days, 1, 90);
		DateTime since = _clock.UtcNow.Date.AddDays(-days);
		IQueryable<TaskItem> tasks = from t in _db.Tasks.AsNoTracking()
			where t.CompletedAt != null && t.CompletedAt >= since && (int)t.Status == 60
			select t;
		Guid? companyId = _currentUser.CompanyId;
		if (companyId.HasValue)
		{
			Guid cid = companyId.GetValueOrDefault();
			tasks = tasks.Where((TaskItem t) => t.CompanyId == cid);
		}
		return await (from t in tasks
			group t by t.CompletedAt.Value.Date into g
			select new FailureTrendPointDto(g.Key, g.LongCount()) into p
			orderby p.Bucket
			select p).ToListAsync(cancellationToken);
	}

	public async Task<IReadOnlyList<CompletionTrendPointDto>> GetCompletionTrendAsync(int days, CancellationToken cancellationToken = default(CancellationToken))
	{
		days = Math.Clamp(days, 1, 90);
		DateTime since = _clock.UtcNow.Date.AddDays(-days);
		IQueryable<TaskItem> tasks = from t in _db.Tasks.AsNoTracking()
			where t.CompletedAt != null && t.CompletedAt >= since && (int)t.Status == 50
			select t;
		Guid? companyId = _currentUser.CompanyId;
		if (companyId.HasValue)
		{
			Guid cid = companyId.GetValueOrDefault();
			tasks = tasks.Where((TaskItem t) => t.CompanyId == cid);
		}
		return await (from t in tasks
			group t by t.CompletedAt.Value.Date into g
			select new CompletionTrendPointDto(g.Key, g.LongCount()) into p
			orderby p.Bucket
			select p).ToListAsync(cancellationToken);
	}
}
