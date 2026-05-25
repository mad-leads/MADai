using System.Threading;
using System.Threading.Tasks;
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

namespace MADai.Application.Abstractions;

public interface IDbContextAccess
{
	DbSet<ApplicationUser> Users { get; }

	DbSet<ApplicationRole> Roles { get; }

	DbSet<Permission> Permissions { get; }

	DbSet<RolePermission> RolePermissions { get; }

	DbSet<RefreshToken> RefreshTokens { get; }

	DbSet<LoginHistory> LoginHistory { get; }

	DbSet<ApiKey> ApiKeys { get; }

	DbSet<UserSession> UserSessions { get; }

	DbSet<Company> Companies { get; }

	DbSet<CompanyBranding> CompanyBrandings { get; }

	DbSet<CompanySettings> CompanySettings { get; }

	DbSet<CompanyPlan> CompanyPlans { get; }

	DbSet<TaskItem> Tasks { get; }

	DbSet<TaskDependency> TaskDependencies { get; }

	DbSet<TaskComment> TaskComments { get; }

	DbSet<TaskLog> TaskLogs { get; }

	DbSet<TaskArtifact> TaskArtifacts { get; }

	DbSet<TaskRetry> TaskRetries { get; }

	DbSet<TaskFailure> TaskFailures { get; }

	DbSet<TaskTemplate> TaskTemplates { get; }

	DbSet<TaskTag> TaskTags { get; }

	DbSet<TaskTagLink> TaskTagLinks { get; }

	DbSet<TaskAssignment> TaskAssignments { get; }

	DbSet<TaskExecution> TaskExecutions { get; }

	DbSet<TaskRecommendation> TaskRecommendations { get; }

	DbSet<ClaudeTask> ClaudeTasks { get; }

	DbSet<ClaudePromptTemplate> ClaudePromptTemplates { get; }

	DbSet<WorkerNode> WorkerNodes { get; }

	DbSet<WorkerHeartbeat> WorkerHeartbeats { get; }

	DbSet<WorkerQueue> WorkerQueues { get; }

	DbSet<WorkerCapabilityEntry> WorkerCapabilities { get; }

	DbSet<WorkerMetric> WorkerMetrics { get; }

	DbSet<RepositoryIntelligence> RepositoryIntelligence { get; }

	DbSet<SessionCheckpoint> SessionCheckpoints { get; }

	DbSet<WorkerMemory> WorkerMemory { get; }

	DbSet<ArchitectureSummary> ArchitectureSummaries { get; }

	DbSet<DependencyGraph> DependencyGraphs { get; }

	DbSet<RouteMap> RouteMaps { get; }

	DbSet<EntityMap> EntityMaps { get; }

	DbSet<SessionMetric> SessionMetrics { get; }

	DbSet<WorkerStatistic> WorkerStatistics { get; }

	DbSet<SessionRotation> SessionRotations { get; }

	DbSet<TokenUsage> TokenUsage { get; }

	DbSet<RepositoryCache> RepositoryCaches { get; }

	DbSet<ProcessHealth> ProcessHealth { get; }

	DbSet<NativeService> NativeServices { get; }

	DbSet<WorkerProcess> WorkerProcesses { get; }

	DbSet<ProcessRestart> ProcessRestarts { get; }

	DbSet<ServiceDependency> ServiceDependencies { get; }

	DbSet<AuditRun> AuditRuns { get; }

	DbSet<AuditFinding> AuditFindings { get; }

	DbSet<AuditRecommendation> AuditRecommendations { get; }

	DbSet<OptimizationSuggestion> OptimizationSuggestions { get; }

	DbSet<CleanupTask> CleanupTasks { get; }

	DbSet<FileFolder> FileFolders { get; }

	DbSet<FileItem> Files { get; }

	DbSet<FileVersion> FileVersions { get; }

	DbSet<FileAccessLog> FileAccessLogs { get; }

	DbSet<Plan> Plans { get; }

	DbSet<Subscription> Subscriptions { get; }

	DbSet<UsageRecord> UsageRecords { get; }

	DbSet<CreditLedger> CreditLedgers { get; }

	DbSet<Invoice> Invoices { get; }

	DbSet<Payment> Payments { get; }

	DbSet<Notification> Notifications { get; }

	DbSet<NotificationTemplate> NotificationTemplates { get; }

	DbSet<NotificationHistory> NotificationHistory { get; }

	DbSet<NotificationPreference> NotificationPreferences { get; }

	DbSet<FeatureFlag> FeatureFlags { get; }

	DbSet<SystemSetting> SystemSettings { get; }

	DbSet<AuditLog> AuditLogs { get; }

	DbSet<WebhookEndpoint> WebhookEndpoints { get; }

	DbSet<WebhookEvent> WebhookEvents { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
}
