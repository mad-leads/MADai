using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Domain.Workers;

namespace MADai.Application.Features.PersistentWorkers;

public class NativeProcessMonitor : INativeProcessMonitor
{
	private readonly IDbContextAccess _db;

	public NativeProcessMonitor(IDbContextAccess db)
	{
		_db = db;
	}

	public async Task<IReadOnlyList<NativeProcessDto>> SnapshotAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		string[] names = new string[7] { "dotnet", "node", "redis-server", "ffmpeg", "magick", "powershell", "pwsh" };
		List<NativeProcessDto> rows = (from p in Process.GetProcesses()
			where names.Contains<string>(p.ProcessName, StringComparer.OrdinalIgnoreCase)
			select CreateDto(p) into p
			orderby p.ProcessName, p.ProcessId
			select p).ToList();
		foreach (NativeProcessDto row in rows)
		{
			_db.ProcessHealth.Add(new ProcessHealth
			{
				ProcessKey = row.ProcessKey,
				ProcessName = row.ProcessName,
				ProcessId = row.ProcessId,
				Status = row.Status,
				MemoryMb = row.MemoryMb,
				CheckedAt = row.CheckedAt,
				Details = row.Details
			});
		}
		await _db.SaveChangesAsync(cancellationToken);
		return rows;
	}

	private static NativeProcessDto CreateDto(Process process)
	{
		try
		{
			return new NativeProcessDto($"{process.ProcessName}:{process.Id}", process.ProcessName, process.Id, process.HasExited ? "Exited" : "Running", Math.Round((double)process.WorkingSet64 / 1024.0 / 1024.0, 2), DateTime.UtcNow, process.MainWindowTitle);
		}
		catch
		{
			return new NativeProcessDto($"{process.ProcessName}:{process.Id}", process.ProcessName, process.Id, "Unknown", 0.0, DateTime.UtcNow, null);
		}
	}
}
