using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Domain.Enums;
using MADai.Domain.Workers;
using MADai.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.Workers;

public class WorkerRegistrationService : IWorkerRegistrationService
{
	private readonly IDbContextAccess _db;

	private readonly IWorkerApiKeyHasher _hasher;

	private readonly IDateTimeProvider _clock;

	public WorkerRegistrationService(IDbContextAccess db, IWorkerApiKeyHasher hasher, IDateTimeProvider clock)
	{
		_db = db;
		_hasher = hasher;
		_clock = clock;
	}

	public async Task<WorkerRegisterResponse> RegisterAsync(WorkerRegisterRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		WorkerRegisterRequest request2 = request;
		string apiKey = _hasher.Generate();
		string queue = request2.Queues?.FirstOrDefault() ?? "default";
		WorkerNode existing = await _db.WorkerNodes.FirstOrDefaultAsync((WorkerNode w) => w.MachineName == request2.MachineName && w.Name == request2.Name && w.CompanyId == request2.CompanyId, cancellationToken);
		string capabilities = JsonSerializer.Serialize(request2.Capabilities ?? Array.Empty<WorkerCapability>());
		if (existing == null)
		{
			existing = new WorkerNode
			{
				Name = request2.Name,
				MachineName = request2.MachineName,
				AgentVersion = request2.AgentVersion,
				OperatingSystem = request2.OperatingSystem,
				IpAddress = request2.IpAddress,
				MaxConcurrency = Math.Max(1, request2.MaxConcurrency),
				CompanyId = request2.CompanyId,
				QueueName = queue,
				Capabilities = capabilities,
				Labels = request2.Labels,
				Status = WorkerStatus.Starting,
				StartedAt = _clock.UtcNow,
				LastHeartbeatAt = _clock.UtcNow,
				ApiKeyHash = _hasher.Hash(apiKey),
				IsActive = true,
				WorkspaceRoot = request2.WorkspaceRoot
			};
			_db.WorkerNodes.Add(existing);
		}
		else
		{
			existing.AgentVersion = request2.AgentVersion ?? existing.AgentVersion;
			existing.OperatingSystem = request2.OperatingSystem ?? existing.OperatingSystem;
			existing.IpAddress = request2.IpAddress ?? existing.IpAddress;
			existing.MaxConcurrency = Math.Max(1, request2.MaxConcurrency);
			existing.QueueName = queue;
			existing.Capabilities = capabilities;
			existing.Labels = request2.Labels ?? existing.Labels;
			existing.Status = WorkerStatus.Starting;
			existing.StartedAt = _clock.UtcNow;
			existing.LastHeartbeatAt = _clock.UtcNow;
			existing.ApiKeyHash = _hasher.Hash(apiKey);
			existing.IsActive = true;
			existing.WorkspaceRoot = request2.WorkspaceRoot ?? existing.WorkspaceRoot;
		}
		foreach (WorkerCapability cap in request2.Capabilities ?? Array.Empty<WorkerCapability>())
		{
			if (!(await _db.WorkerCapabilities.AnyAsync((WorkerCapabilityEntry c) => c.WorkerNodeId == existing.Id && (int)c.Capability == (int)cap, cancellationToken)))
			{
				_db.WorkerCapabilities.Add(new WorkerCapabilityEntry
				{
					WorkerNodeId = existing.Id,
					Capability = cap
				});
			}
		}
		await _db.SaveChangesAsync(cancellationToken);
		return new WorkerRegisterResponse(existing.Id, apiKey, queue);
	}

	public async Task DeactivateAsync(Guid workerId, CancellationToken cancellationToken = default(CancellationToken))
	{
		WorkerNode w = await _db.WorkerNodes.FirstOrDefaultAsync((WorkerNode x) => x.Id == workerId, cancellationToken);
		if (w != null)
		{
			w.IsActive = false;
			w.Status = WorkerStatus.Offline;
			await _db.SaveChangesAsync(cancellationToken);
		}
	}

	public async Task DrainAsync(Guid workerId, CancellationToken cancellationToken = default(CancellationToken))
	{
		WorkerNode w = await _db.WorkerNodes.FirstOrDefaultAsync((WorkerNode x) => x.Id == workerId, cancellationToken);
		if (w != null)
		{
			w.Status = WorkerStatus.Draining;
			await _db.SaveChangesAsync(cancellationToken);
		}
	}
}
