using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MADai.Application.Features.PersistentWorkers;

public class RepositoryIntelligenceService : IRepositoryIntelligenceService
{
	private sealed record RepositoryScan(List<string> ProjectFiles, List<string> RouteFiles, List<string> EndpointFiles, List<string> EntityFiles, List<string> PackageFiles, List<string> PowerShellScripts);

	private static readonly string[] InterestingExtensions = new string[8] { ".cs", ".ts", ".html", ".scss", ".json", ".csproj", ".sln", ".ps1" };

	private readonly IDbContextAccess _db;

	private readonly ILogger<RepositoryIntelligenceService> _logger;

	public RepositoryIntelligenceService(IDbContextAccess db, ILogger<RepositoryIntelligenceService> logger)
	{
		_db = db;
		_logger = logger;
	}

	public async Task<RepositoryIntelligenceDto> GetOrRefreshAsync(RepositoryRegistrationRequest request, bool forceRefresh, CancellationToken cancellationToken = default(CancellationToken))
	{
		RepositoryRegistrationRequest request2 = request;
		if (!Directory.Exists(request2.RepositoryPath))
		{
			throw new DirectoryNotFoundException("Repository path does not exist: " + request2.RepositoryPath);
		}
		string fingerprint = ComputeFingerprint(request2.RepositoryPath);
		RepositoryIntelligence existing = await _db.RepositoryIntelligence.FirstOrDefaultAsync((RepositoryIntelligence x) => x.RepositoryKey == request2.RepositoryKey, cancellationToken);
		if (!forceRefresh && existing != null && existing.CacheFingerprint == fingerprint)
		{
			return ToDto(existing);
		}
		RepositoryScan scan = ScanRepository(request2.RepositoryPath);
		string summary = BuildSummary(request2.RepositoryKey, scan);
		string routeMap = JsonSerializer.Serialize(scan.RouteFiles);
		string endpointMap = JsonSerializer.Serialize(scan.EndpointFiles);
		string entityMap = JsonSerializer.Serialize(scan.EntityFiles);
		string dependencyGraph = JsonSerializer.Serialize(scan.ProjectFiles);
		string architecture = JsonSerializer.Serialize(new { scan.ProjectFiles, scan.RouteFiles, scan.EndpointFiles, scan.EntityFiles, scan.PackageFiles, scan.PowerShellScripts });
		bool num = existing == null;
		if (existing == null)
		{
			existing = new RepositoryIntelligence
			{
				RepositoryKey = request2.RepositoryKey
			};
		}
		existing.RepositoryPath = request2.RepositoryPath;
		existing.BranchName = request2.BranchName;
		existing.Summary = summary;
		existing.ArchitectureJson = architecture;
		existing.RouteMapJson = routeMap;
		existing.EndpointMapJson = endpointMap;
		existing.EntityMapJson = entityMap;
		existing.DependencyGraphJson = dependencyGraph;
		existing.BuildInstructions = ((scan.ProjectFiles.Count > 0) ? "dotnet restore; dotnet build" : "npm install; npm run build");
		existing.ValidationInstructions = "Run backend tests/build, frontend build, then validate /health/ready and worker heartbeat.";
		existing.ScannedAt = DateTime.UtcNow;
		existing.CacheFingerprint = fingerprint;
		if (num)
		{
			_db.RepositoryIntelligence.Add(existing);
		}
		_db.ArchitectureSummaries.Add(new ArchitectureSummary
		{
			RepositoryKey = request2.RepositoryKey,
			Scope = "repository",
			Content = summary
		});
		_db.DependencyGraphs.Add(new DependencyGraph
		{
			RepositoryKey = request2.RepositoryKey,
			GraphJson = dependencyGraph
		});
		_db.RouteMaps.Add(new RouteMap
		{
			RepositoryKey = request2.RepositoryKey,
			Kind = "auto",
			MapJson = routeMap
		});
		_db.EntityMaps.Add(new EntityMap
		{
			RepositoryKey = request2.RepositoryKey,
			MapJson = entityMap
		});
		_db.RepositoryCaches.Add(new RepositoryCache
		{
			RepositoryKey = request2.RepositoryKey,
			CacheKey = "latest-intelligence",
			CacheJson = architecture,
			ExpiresAt = DateTime.UtcNow.AddHours(6.0)
		});
		await _db.SaveChangesAsync(cancellationToken);
		_logger.LogInformation("Refreshed repository intelligence for {RepositoryKey}", request2.RepositoryKey);
		return ToDto(existing);
	}

	private static RepositoryScan ScanRepository(string root)
	{
		string root2 = root;
		List<string> files = (from f in Directory.EnumerateFiles(root2, "*.*", SearchOption.AllDirectories)
			where !f.Contains($"{Path.DirectorySeparatorChar}.git{Path.DirectorySeparatorChar}") && !f.Contains($"{Path.DirectorySeparatorChar}node_modules{Path.DirectorySeparatorChar}") && !f.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}") && !f.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}") && InterestingExtensions.Contains<string>(Path.GetExtension(f), StringComparer.OrdinalIgnoreCase)
			select Path.GetRelativePath(root2, f)).Take(4000).ToList();
		return new RepositoryScan(files.Where((string f) => f.EndsWith(".sln", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)).ToList(), files.Where((string f) => f.EndsWith("app.routes.ts", StringComparison.OrdinalIgnoreCase) || f.Contains("routes", StringComparison.OrdinalIgnoreCase)).ToList(), files.Where((string f) => f.EndsWith("Controller.cs", StringComparison.OrdinalIgnoreCase) || f.Contains($"{Path.DirectorySeparatorChar}Controllers{Path.DirectorySeparatorChar}")).ToList(), files.Where((string f) => f.Contains($"{Path.DirectorySeparatorChar}Domain{Path.DirectorySeparatorChar}") || f.Contains($"{Path.DirectorySeparatorChar}Entities{Path.DirectorySeparatorChar}")).ToList(), files.Where((string f) => f.EndsWith("package.json", StringComparison.OrdinalIgnoreCase)).ToList(), files.Where((string f) => f.EndsWith(".ps1", StringComparison.OrdinalIgnoreCase)).ToList());
	}

	private static string BuildSummary(string repositoryKey, RepositoryScan scan)
	{
		return $"{repositoryKey}: {scan.ProjectFiles.Count} .NET project files, {scan.RouteFiles.Count} route files, {scan.EndpointFiles.Count} API endpoint files, {scan.EntityFiles.Count} entity/domain files, {scan.PackageFiles.Count} package manifests, {scan.PowerShellScripts.Count} PowerShell scripts.";
	}

	private static string ComputeFingerprint(string root)
	{
		string root2 = root;
		string joined = string.Join('|', from f in (from f in Directory.EnumerateFiles(root2, "*.*", SearchOption.AllDirectories)
				where !f.Contains($"{Path.DirectorySeparatorChar}.git{Path.DirectorySeparatorChar}") && !f.Contains($"{Path.DirectorySeparatorChar}node_modules{Path.DirectorySeparatorChar}")
				select new FileInfo(f) into f
				orderby f.FullName
				select f).Take(4000)
			select $"{Path.GetRelativePath(root2, f.FullName)}:{f.Length}:{f.LastWriteTimeUtc.Ticks}");
		return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(joined))).ToLowerInvariant();
	}

	private static RepositoryIntelligenceDto ToDto(RepositoryIntelligence item)
	{
		return new RepositoryIntelligenceDto(item.RepositoryKey, item.RepositoryPath, item.Summary, item.ArchitectureJson, item.RouteMapJson, item.EndpointMapJson, item.EntityMapJson, item.DependencyGraphJson, item.BuildInstructions, item.ValidationInstructions, item.ScannedAt, item.CacheFingerprint);
	}
}
