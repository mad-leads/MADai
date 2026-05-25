using System;
using System.Linq.Expressions;
using MADai.Domain.Audit;
using MADai.Domain.Billing;
using MADai.Domain.Common;
using MADai.Domain.Files;
using MADai.Domain.Identity;
using MADai.Domain.Notifications;
using MADai.Domain.SystemEntities;
using MADai.Domain.Tasks;
using MADai.Domain.Tenancy;
using MADai.Domain.Webhooks;
using MADai.Domain.Workers;
using MADai.Infrastructure.Persistence.Interceptors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MADai.Infrastructure.Persistence;

public class MADaiDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
	private readonly AuditingInterceptor _auditingInterceptor;

	public DbSet<Permission> Permissions => Set<Permission>();

	public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

	public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

	public DbSet<LoginHistory> LoginHistory => Set<LoginHistory>();

	public DbSet<ApiKey> ApiKeys => Set<ApiKey>();

	public DbSet<UserSession> UserSessions => Set<UserSession>();

	public DbSet<Company> Companies => Set<Company>();

	public DbSet<CompanyBranding> CompanyBrandings => Set<CompanyBranding>();

	public DbSet<CompanySettings> CompanySettings => Set<CompanySettings>();

	public DbSet<CompanyPlan> CompanyPlans => Set<CompanyPlan>();

	public DbSet<TaskItem> Tasks => Set<TaskItem>();

	public DbSet<TaskDependency> TaskDependencies => Set<TaskDependency>();

	public DbSet<TaskComment> TaskComments => Set<TaskComment>();

	public DbSet<TaskLog> TaskLogs => Set<TaskLog>();

	public DbSet<TaskArtifact> TaskArtifacts => Set<TaskArtifact>();

	public DbSet<TaskRetry> TaskRetries => Set<TaskRetry>();

	public DbSet<TaskFailure> TaskFailures => Set<TaskFailure>();

	public DbSet<TaskTemplate> TaskTemplates => Set<TaskTemplate>();

	public DbSet<TaskTag> TaskTags => Set<TaskTag>();

	public DbSet<TaskTagLink> TaskTagLinks => Set<TaskTagLink>();

	public DbSet<TaskAssignment> TaskAssignments => Set<TaskAssignment>();

	public DbSet<TaskExecution> TaskExecutions => Set<TaskExecution>();

	public DbSet<TaskRecommendation> TaskRecommendations => Set<TaskRecommendation>();

	public DbSet<ClaudeTask> ClaudeTasks => Set<ClaudeTask>();

	public DbSet<ClaudePromptTemplate> ClaudePromptTemplates => Set<ClaudePromptTemplate>();

	public DbSet<WorkerNode> WorkerNodes => Set<WorkerNode>();

	public DbSet<WorkerHeartbeat> WorkerHeartbeats => Set<WorkerHeartbeat>();

	public DbSet<WorkerQueue> WorkerQueues => Set<WorkerQueue>();

	public DbSet<WorkerCapabilityEntry> WorkerCapabilities => Set<WorkerCapabilityEntry>();

	public DbSet<WorkerMetric> WorkerMetrics => Set<WorkerMetric>();

	public DbSet<RepositoryIntelligence> RepositoryIntelligence => Set<RepositoryIntelligence>();

	public DbSet<SessionCheckpoint> SessionCheckpoints => Set<SessionCheckpoint>();

	public DbSet<WorkerMemory> WorkerMemory => Set<WorkerMemory>();

	public DbSet<ArchitectureSummary> ArchitectureSummaries => Set<ArchitectureSummary>();

	public DbSet<DependencyGraph> DependencyGraphs => Set<DependencyGraph>();

	public DbSet<RouteMap> RouteMaps => Set<RouteMap>();

	public DbSet<EntityMap> EntityMaps => Set<EntityMap>();

	public DbSet<SessionMetric> SessionMetrics => Set<SessionMetric>();

	public DbSet<WorkerStatistic> WorkerStatistics => Set<WorkerStatistic>();

	public DbSet<SessionRotation> SessionRotations => Set<SessionRotation>();

	public DbSet<TokenUsage> TokenUsage => Set<TokenUsage>();

	public DbSet<RepositoryCache> RepositoryCaches => Set<RepositoryCache>();

	public DbSet<ProcessHealth> ProcessHealth => Set<ProcessHealth>();

	public DbSet<NativeService> NativeServices => Set<NativeService>();

	public DbSet<WorkerProcess> WorkerProcesses => Set<WorkerProcess>();

	public DbSet<ProcessRestart> ProcessRestarts => Set<ProcessRestart>();

	public DbSet<ServiceDependency> ServiceDependencies => Set<ServiceDependency>();

	public DbSet<AuditRun> AuditRuns => Set<AuditRun>();

	public DbSet<AuditFinding> AuditFindings => Set<AuditFinding>();

	public DbSet<AuditRecommendation> AuditRecommendations => Set<AuditRecommendation>();

	public DbSet<OptimizationSuggestion> OptimizationSuggestions => Set<OptimizationSuggestion>();

	public DbSet<CleanupTask> CleanupTasks => Set<CleanupTask>();

	public DbSet<FileFolder> FileFolders => Set<FileFolder>();

	public DbSet<FileItem> Files => Set<FileItem>();

	public DbSet<FileVersion> FileVersions => Set<FileVersion>();

	public DbSet<FileAccessLog> FileAccessLogs => Set<FileAccessLog>();

	public DbSet<Plan> Plans => Set<Plan>();

	public DbSet<Subscription> Subscriptions => Set<Subscription>();

	public DbSet<UsageRecord> UsageRecords => Set<UsageRecord>();

	public DbSet<CreditLedger> CreditLedgers => Set<CreditLedger>();

	public DbSet<Invoice> Invoices => Set<Invoice>();

	public DbSet<Payment> Payments => Set<Payment>();

	public DbSet<Notification> Notifications => Set<Notification>();

	public DbSet<NotificationTemplate> NotificationTemplates => Set<NotificationTemplate>();

	public DbSet<NotificationHistory> NotificationHistory => Set<NotificationHistory>();

	public DbSet<NotificationPreference> NotificationPreferences => Set<NotificationPreference>();

	public DbSet<FeatureFlag> FeatureFlags => Set<FeatureFlag>();

	public DbSet<SystemSetting> SystemSettings => Set<SystemSetting>();

	public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

	public DbSet<WebhookEndpoint> WebhookEndpoints => Set<WebhookEndpoint>();

	public DbSet<WebhookEvent> WebhookEvents => Set<WebhookEvent>();

	public MADaiDbContext(DbContextOptions<MADaiDbContext> options, AuditingInterceptor auditingInterceptor)
		: base((DbContextOptions)options)
	{
		_auditingInterceptor = auditingInterceptor;
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);
		optionsBuilder.AddInterceptors(_auditingInterceptor);
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.ApplyConfigurationsFromAssembly(typeof(MADaiDbContext).Assembly);
		builder.Entity<ApplicationUser>().ToTable("Users");
		builder.Entity<ApplicationRole>().ToTable("Roles");
		builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
		builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
		builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
		builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");
		builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
		foreach (IMutableEntityType entityType in builder.Model.GetEntityTypes())
		{
			if (typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
			{
				builder.Entity(entityType.ClrType).HasQueryFilter(BuildSoftDeleteFilter(entityType.ClrType));
				builder.Entity(entityType.ClrType).Property<byte[]>("RowVersion").IsRowVersion()
					.IsConcurrencyToken();
			}
		}
	}

	private static LambdaExpression BuildSoftDeleteFilter(Type type)
	{
		ParameterExpression parameter = Expression.Parameter(type, "e");
		MemberExpression left = Expression.Property(parameter, "IsDeleted");
		ConstantExpression falseConst = Expression.Constant(false);
		return Expression.Lambda(Expression.Equal(left, falseConst), parameter);
	}
}
