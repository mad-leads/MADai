using System;
using MADai.Application.Abstractions;
using MADai.Application.Features.Admin;
using MADai.Application.Features.Auth;
using MADai.Application.Features.ClaudePromptTemplates;
using MADai.Application.Features.ClaudeTasks.Attachments;
using MADai.Application.Features.Dashboard;
using MADai.Application.Features.Notifications;
using MADai.Application.Features.PersistentWorkers;
using MADai.Application.Features.Settings;
using MADai.Application.Features.TaskComments;
using MADai.Application.Features.TaskRecommendations;
using MADai.Application.Features.TaskTemplates;
using MADai.Application.Features.Webhooks;
using MADai.Application.Features.Workers;
using MADai.Domain.Identity;
using MADai.Infrastructure.Email;
using MADai.Infrastructure.Events;
using MADai.Infrastructure.Identity;
using MADai.Infrastructure.Options;
using MADai.Infrastructure.Persistence;
using MADai.Infrastructure.Persistence.Interceptors;
using MADai.Infrastructure.Storage;
using MADai.Infrastructure.Time;
using MADai.Infrastructure.Webhooks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MADai.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		IConfiguration configuration2 = configuration;
		services.Configure<JwtOptions>(configuration2.GetSection("Jwt"));
		services.Configure<StorageOptions>(configuration2.GetSection("Storage"));
		services.Configure(delegate(SmtpOptions opts)
		{
			opts.Host = configuration2["SMTP_HOST"] ?? configuration2["Smtp:Host"];
			opts.Port = (int.TryParse(configuration2["SMTP_PORT"], out var result) ? result : (int.TryParse(configuration2["Smtp:Port"], out var result2) ? result2 : 587));
			opts.Secure = !string.Equals(configuration2["SMTP_SECURE"], "false", StringComparison.OrdinalIgnoreCase) && !string.Equals(configuration2["Smtp:Secure"], "false", StringComparison.OrdinalIgnoreCase);
			opts.Username = configuration2["SMTP_USER"] ?? configuration2["Smtp:Username"];
			opts.Password = configuration2["SMTP_PASS"] ?? configuration2["Smtp:Password"];
			opts.FromName = configuration2["SMTP_FROM_NAME"] ?? configuration2["Smtp:FromName"] ?? "MADai";
			opts.FromAddress = configuration2["SMTP_FROM_ADDRESS"] ?? configuration2["Smtp:FromAddress"] ?? configuration2["SMTP_USER"];
		});
		services.AddScoped<AuditingInterceptor>();
		services.AddDbContext<MADaiDbContext>(delegate(IServiceProvider sp, DbContextOptionsBuilder options)
		{
			string? connectionString = configuration2.GetConnectionString("Default");
			if (string.IsNullOrWhiteSpace(connectionString))
			{
				connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=MADai;Trusted_Connection=True;TrustServerCertificate=True;";
			}
			options.UseSqlServer(connectionString, delegate(SqlServerDbContextOptionsBuilder sql)
			{
				sql.MigrationsAssembly(typeof(MADaiDbContext).Assembly.GetName().Name);
			});
		});
		services.AddScoped<IDbContextAccess, DbContextAccess>();
		services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
		services.AddSingleton<IWorkerApiKeyHasher, WorkerApiKeyHasher>();
		services.AddSingleton<IFileStorage>((IServiceProvider sp) => (sp.GetRequiredService<IOptions<StorageOptions>>().Value.Provider ?? "Local").ToLowerInvariant() switch
		{
			"azure" => ActivatorUtilities.CreateInstance<AzureBlobFileStorage>(sp, Array.Empty<object>()), 
			"azureblob" => ActivatorUtilities.CreateInstance<AzureBlobFileStorage>(sp, Array.Empty<object>()), 
			"s3" => ActivatorUtilities.CreateInstance<S3FileStorage>(sp, Array.Empty<object>()), 
			_ => ActivatorUtilities.CreateInstance<LocalFileStorage>(sp, Array.Empty<object>()), 
		});
		services.AddSingleton<IEmailSender, SmtpEmailSender>();
		services.AddSingleton<IEventPublisher, NullEventPublisher>();
		services.AddScoped<ICurrentUserService, CurrentUserService>();
		services.AddScoped<IJwtTokenService, JwtTokenService>();
		services.AddScoped<IAuthService, AuthService>();
		services.AddScoped<IWorkerAuthenticator, WorkerAuthenticator>();
		services.AddScoped<IWorkerQueueService, WorkerQueueService>();
		services.AddScoped<IWorkerRegistrationService, WorkerRegistrationService>();
		services.AddScoped<IRepositoryIntelligenceService, RepositoryIntelligenceService>();
		services.AddScoped<ISessionOrchestrator, SessionOrchestrator>();
		services.AddScoped<INativeProcessMonitor, NativeProcessMonitor>();
		services.AddScoped<IDashboardService, DashboardService>();
		services.AddScoped<IClaudePromptTemplateService, ClaudePromptTemplateService>();
		services.AddScoped<ISettingsService, SettingsService>();
		services.AddScoped<IClaudeTaskAttachmentService, ClaudeTaskAttachmentService>();
		services.AddScoped<ITaskTemplateService, TaskTemplateService>();
		services.AddScoped<ITaskRecommendationService, TaskRecommendationService>();
		services.AddScoped<ITaskCommentService, TaskCommentService>();
		services.AddScoped<IAdminUserService, AdminUserService>();
		services.AddScoped<IFeatureFlagAdminService, FeatureFlagAdminService>();
		services.AddScoped<IPlanAdminService, PlanAdminService>();
		services.AddScoped<ICompanyAdminService, CompanyAdminService>();
		services.AddScoped<INotificationService, NotificationService>();
		services.AddScoped<IWebhookService, WebhookService>();
		services.AddScoped<IWebhookPublisher, DbWebhookPublisher>();
		services.AddIdentityCore<ApplicationUser>(delegate(IdentityOptions opts)
		{
			opts.Password.RequireDigit = true;
			opts.Password.RequiredLength = 8;
			opts.Password.RequireNonAlphanumeric = false;
			opts.Password.RequireUppercase = true;
			opts.Password.RequireLowercase = true;
			opts.User.RequireUniqueEmail = true;
			opts.Lockout.AllowedForNewUsers = true;
			opts.Lockout.MaxFailedAccessAttempts = 6;
			opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10.0);
			opts.SignIn.RequireConfirmedEmail = false;
		}).AddRoles<ApplicationRole>().AddSignInManager<SignInManager<ApplicationUser>>()
			.AddDefaultTokenProviders()
			.AddEntityFrameworkStores<MADaiDbContext>();
		services.AddHttpContextAccessor();
		return services;
	}
}
