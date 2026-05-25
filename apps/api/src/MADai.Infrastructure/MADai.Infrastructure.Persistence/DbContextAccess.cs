using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Domain.Audit;
using MADai.Domain.Billing;
using MADai.Domain.Files;
using MADai.Domain.Identity;
using MADai.Domain.Notifications;
using MADai.Domain.SystemEntities;
using MADai.Domain.Tasks;
using MADai.Domain.Tenancy;
using MADai.Domain.Webhooks;
using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;

namespace MADai.Infrastructure.Persistence;

public class DbContextAccess : IDbContextAccess
{
	private readonly MADaiDbContext _db;

	public DbSet<ApplicationUser> Users => _db.Users;

	public DbSet<ApplicationRole> Roles => _db.Roles;

	public DbSet<Permission> Permissions => _db.Permissions;

	public DbSet<RolePermission> RolePermissions => _db.RolePermissions;

	public DbSet<RefreshToken> RefreshTokens => _db.RefreshTokens;

	public DbSet<LoginHistory> LoginHistory => _db.LoginHistory;

	public DbSet<ApiKey> ApiKeys => _db.ApiKeys;

	public DbSet<UserSession> UserSessions => _db.UserSessions;

	public DbSet<Company> Companies => _db.Companies;

	public DbSet<CompanyBranding> CompanyBrandings => _db.CompanyBrandings;

	public DbSet<CompanySettings> CompanySettings => _db.CompanySettings;

	public DbSet<CompanyPlan> CompanyPlans => _db.CompanyPlans;

	public DbSet<TaskItem> Tasks => _db.Tasks;

	public DbSet<TaskDependency> TaskDependencies => _db.TaskDependencies;

	public DbSet<TaskComment> TaskComments => _db.TaskComments;

	public DbSet<TaskLog> TaskLogs => _db.TaskLogs;

	public DbSet<TaskArtifact> TaskArtifacts => _db.TaskArtifacts;

	public DbSet<TaskRetry> TaskRetries => _db.TaskRetries;

	public DbSet<TaskFailure> TaskFailures => _db.TaskFailures;

	public DbSet<TaskTemplate> TaskTemplates => _db.TaskTemplates;

	public DbSet<TaskTag> TaskTags => _db.TaskTags;

	public DbSet<TaskTagLink> TaskTagLinks => _db.TaskTagLinks;

	public DbSet<TaskAssignment> TaskAssignments => _db.TaskAssignments;

	public DbSet<TaskExecution> TaskExecutions => _db.TaskExecutions;

	public DbSet<TaskRecommendation> TaskRecommendations => _db.TaskRecommendations;

	public DbSet<ClaudeTask> ClaudeTasks => _db.ClaudeTasks;

	public DbSet<ClaudePromptTemplate> ClaudePromptTemplates => _db.ClaudePromptTemplates;

	public DbSet<WorkerNode> WorkerNodes => _db.WorkerNodes;

	public DbSet<WorkerHeartbeat> WorkerHeartbeats => _db.WorkerHeartbeats;

	public DbSet<WorkerQueue> WorkerQueues => _db.WorkerQueues;

	public DbSet<WorkerCapabilityEntry> WorkerCapabilities => _db.WorkerCapabilities;

	public DbSet<WorkerMetric> WorkerMetrics => _db.WorkerMetrics;

	public DbSet<RepositoryIntelligence> RepositoryIntelligence => _db.RepositoryIntelligence;

	public DbSet<SessionCheckpoint> SessionCheckpoints => _db.SessionCheckpoints;

	public DbSet<WorkerMemory> WorkerMemory => _db.WorkerMemory;

	public DbSet<ArchitectureSummary> ArchitectureSummaries => _db.ArchitectureSummaries;

	public DbSet<DependencyGraph> DependencyGraphs => _db.DependencyGraphs;

	public DbSet<RouteMap> RouteMaps => _db.RouteMaps;

	public DbSet<EntityMap> EntityMaps => _db.EntityMaps;

	public DbSet<SessionMetric> SessionMetrics => _db.SessionMetrics;

	public DbSet<WorkerStatistic> WorkerStatistics => _db.WorkerStatistics;

	public DbSet<SessionRotation> SessionRotations => _db.SessionRotations;

	public DbSet<TokenUsage> TokenUsage => _db.TokenUsage;

	public DbSet<RepositoryCache> RepositoryCaches => _db.RepositoryCaches;

	public DbSet<ProcessHealth> ProcessHealth => _db.ProcessHealth;

	public DbSet<NativeService> NativeServices => _db.NativeServices;

	public DbSet<WorkerProcess> WorkerProcesses => _db.WorkerProcesses;

	public DbSet<ProcessRestart> ProcessRestarts => _db.ProcessRestarts;

	public DbSet<ServiceDependency> ServiceDependencies => _db.ServiceDependencies;

	public DbSet<AuditRun> AuditRuns => _db.AuditRuns;

	public DbSet<AuditFinding> AuditFindings => _db.AuditFindings;

	public DbSet<AuditRecommendation> AuditRecommendations => _db.AuditRecommendations;

	public DbSet<OptimizationSuggestion> OptimizationSuggestions => _db.OptimizationSuggestions;

	public DbSet<CleanupTask> CleanupTasks => _db.CleanupTasks;

	public DbSet<FileFolder> FileFolders => _db.FileFolders;

	public DbSet<FileItem> Files => _db.Files;

	public DbSet<FileVersion> FileVersions => _db.FileVersions;

	public DbSet<FileAccessLog> FileAccessLogs => _db.FileAccessLogs;

	public DbSet<Plan> Plans => _db.Plans;

	public DbSet<Subscription> Subscriptions => _db.Subscriptions;

	public DbSet<UsageRecord> UsageRecords => _db.UsageRecords;

	public DbSet<CreditLedger> CreditLedgers => _db.CreditLedgers;

	public DbSet<Invoice> Invoices => _db.Invoices;

	public DbSet<Payment> Payments => _db.Payments;

	public DbSet<Notification> Notifications => _db.Notifications;

	public DbSet<NotificationTemplate> NotificationTemplates => _db.NotificationTemplates;

	public DbSet<NotificationHistory> NotificationHistory => _db.NotificationHistory;

	public DbSet<NotificationPreference> NotificationPreferences => _db.NotificationPreferences;

	public DbSet<FeatureFlag> FeatureFlags => _db.FeatureFlags;

	public DbSet<SystemSetting> SystemSettings => _db.SystemSettings;

	public DbSet<AuditLog> AuditLogs => _db.AuditLogs;

	public DbSet<WebhookEndpoint> WebhookEndpoints => _db.WebhookEndpoints;

	public DbSet<WebhookEvent> WebhookEvents => _db.WebhookEvents;

	public DbContextAccess(MADaiDbContext db)
	{
		_db = db;
	}

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return _db.SaveChangesAsync(cancellationToken);
	}
}
