using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Domain.Billing;
using MADai.Domain.Enums;
using MADai.Domain.Identity;
using MADai.Domain.SystemEntities;
using MADai.Domain.Tasks;
using MADai.Domain.Tenancy;
using MADai.Domain.Workers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MADai.Infrastructure.Persistence.Seed;

public static class DataSeeder
{
	private const string SuperAdminEmail = "admin@madprospects.com";
	private const string SuperAdminPassword = "P@szw0rdMP";

	public static async Task SeedAsync(IServiceProvider services)
	{
		using IServiceScope scope = services.CreateScope();
		MADaiDbContext db = scope.ServiceProvider.GetRequiredService<MADaiDbContext>();
		ILogger logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DataSeeder");
		RoleManager<ApplicationRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
		UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
		await db.Database.MigrateAsync();
		logger.LogInformation("Database migrations applied.");
		foreach (string roleName in Roles.All)
		{
			if (!(await roleManager.RoleExistsAsync(roleName)))
			{
				await roleManager.CreateAsync(new ApplicationRole
				{
					Name = roleName,
					NormalizedName = roleName.ToUpperInvariant(),
					IsSystem = true,
					Description = roleName + " role"
				});
			}
		}
		List<string> existingPermissionCodes = await db.Permissions.Select((Permission p) => p.Code).ToListAsync();
		foreach (var (code, displayName, category) in Permissions.All())
		{
			if (!existingPermissionCodes.Contains(code))
			{
				db.Permissions.Add(new Permission
				{
					Code = code,
					DisplayName = displayName,
					Category = category
				});
			}
		}
		await db.SaveChangesAsync();
		await GrantRolePermissions(db, roleManager, "SystemAdmin", (from p in Permissions.All()
			select p.Code).ToArray());
		await GrantRolePermissions(db, roleManager, "CompanyAdmin", new string[14]
		{
			"tasks.view", "tasks.create", "tasks.update", "tasks.delete", "tasks.cancel", "tasks.retry", "tasks.assign", "workers.view", "workers.manage", "companies.view",
			"users.view", "users.manage", "audit.view", "audit.manage"
		});
		await GrantRolePermissions(db, roleManager, "CompanyManager", new string[7] { "tasks.view", "tasks.create", "tasks.update", "tasks.cancel", "tasks.retry", "workers.view", "audit.view" });
		await GrantRolePermissions(db, roleManager, "User", new string[4] { "tasks.view", "tasks.create", "tasks.update", "tasks.cancel" });
		await GrantRolePermissions(db, roleManager, "ReadOnly", new string[2] { "tasks.view", "workers.view" });
		if (!(await db.Plans.AnyAsync()))
		{
			db.Plans.AddRange(new Plan
			{
				Name = "Starter",
				Code = "starter",
				MonthlyPrice = 49m,
				AnnualPrice = 490m,
				Currency = "USD",
				IncludedTasks = 200,
				IncludedStorageGb = 10,
				IncludedWorkers = 1,
				IsPublic = true,
				FeaturesJson = "{\"selfHealing\":false}"
			}, new Plan
			{
				Name = "Pro",
				Code = "pro",
				MonthlyPrice = 199m,
				AnnualPrice = 1990m,
				Currency = "USD",
				IncludedTasks = 2000,
				IncludedStorageGb = 100,
				IncludedWorkers = 5,
				IsPublic = true,
				FeaturesJson = "{\"selfHealing\":true}"
			}, new Plan
			{
				Name = "Enterprise",
				Code = "enterprise",
				MonthlyPrice = 0m,
				AnnualPrice = 0m,
				Currency = "USD",
				IncludedTasks = 100000,
				IncludedStorageGb = 2000,
				IncludedWorkers = 100,
				IsPublic = false,
				FeaturesJson = "{\"selfHealing\":true,\"prioritySupport\":true}"
			});
		}
		Company demoCompany = await db.Companies.FirstOrDefaultAsync((Company c) => c.Slug == "mad-products");
		if (demoCompany == null)
		{
			demoCompany = new Company
			{
				Name = "MAD Products",
				Slug = "mad-products",
				ContactEmail = "anton@madproducts.com",
				Country = "ZA",
				Branding = new CompanyBranding
				{
					PrimaryColor = "#7c5cff",
					AccentColor = "#22d3ee"
				},
				Settings = new CompanySettings
				{
					MaxConcurrentTasks = 100,
					MaxStorageGb = 200,
					MaxWorkers = 10
				}
			};
			db.Companies.Add(demoCompany);
		}
		await db.SaveChangesAsync();
		ApplicationUser? admin = await userManager.FindByEmailAsync(SuperAdminEmail);
		if (admin == null)
		{
			admin = new ApplicationUser
			{
				UserName = SuperAdminEmail,
				Email = SuperAdminEmail,
				EmailConfirmed = true,
				IsActive = true,
				FirstName = "MADai",
				LastName = "Superadmin",
				CompanyId = demoCompany.Id
			};
			IdentityResult result = await userManager.CreateAsync(admin, SuperAdminPassword);
			if (result.Succeeded)
			{
				await userManager.AddToRoleAsync(admin, "SystemAdmin");
				logger.LogInformation("Seeded SystemAdmin user: {Email}", SuperAdminEmail);
			}
			else
			{
				logger.LogWarning("Failed to seed admin user: {Errors}", string.Join("; ", result.Errors.Select((IdentityError e) => e.Description)));
			}
		}
		else
		{
			bool changed = false;
			if (!admin.IsActive)
			{
				admin.IsActive = true;
				changed = true;
			}
			if (!admin.EmailConfirmed)
			{
				admin.EmailConfirmed = true;
				changed = true;
			}
			if (admin.CompanyId != demoCompany.Id)
			{
				admin.CompanyId = demoCompany.Id;
				changed = true;
			}
			if (changed)
			{
				await userManager.UpdateAsync(admin);
			}
			if (!await userManager.IsInRoleAsync(admin, "SystemAdmin"))
			{
				await userManager.AddToRoleAsync(admin, "SystemAdmin");
			}
			if (!await userManager.CheckPasswordAsync(admin, SuperAdminPassword))
			{
				string resetToken = await userManager.GeneratePasswordResetTokenAsync(admin);
				IdentityResult resetResult = await userManager.ResetPasswordAsync(admin, resetToken, SuperAdminPassword);
				if (resetResult.Succeeded)
				{
					logger.LogInformation("Reset SystemAdmin password for: {Email}", SuperAdminEmail);
				}
				else
				{
					logger.LogWarning("Failed to reset admin password: {Errors}", string.Join("; ", resetResult.Errors.Select((IdentityError e) => e.Description)));
				}
			}
		}
		if (!(await db.Tasks.AnyAsync()))
		{
			db.Tasks.Add(new TaskItem
			{
				CompanyId = demoCompany.Id,
				Title = "Welcome to MADai",
				Description = "This is a sample task created during seeding. Workers will pick it up and demonstrate the lifecycle.",
				Category = TaskCategory.Documentation,
				Priority = TaskPriority.Normal,
				Status = MADai.Domain.Enums.TaskStatus.Queued,
				Origin = TaskOrigin.User,
				QueueName = "default",
				PromptPayload = "Write a 1-paragraph hello message for the MADai operator.",
				MaxRetries = 1
			});
		}
		await SeedClaudeWorkerNodeAsync(scope.ServiceProvider, db, logger);
		await SeedClaudeSettingsAsync(db);
		await db.SaveChangesAsync();
	}

	private static async Task SeedClaudeWorkerNodeAsync(IServiceProvider services, MADaiDbContext db, ILogger logger)
	{
		string token = services.GetRequiredService<IConfiguration>()["CLAUDE_WORKER_TOKEN"] ?? Environment.GetEnvironmentVariable("CLAUDE_WORKER_TOKEN");
		if (string.IsNullOrWhiteSpace(token))
		{
			logger.LogInformation("CLAUDE_WORKER_TOKEN not configured - skipping claude-code-bootstrap worker seed.");
			return;
		}
		IWorkerApiKeyHasher hasher = services.GetRequiredService<IWorkerApiKeyHasher>();
		string hash = hasher.Hash(token);
		WorkerNode existing = await db.WorkerNodes.FirstOrDefaultAsync((WorkerNode w) => w.MachineName == "claude-code-bootstrap");
		if (existing == null)
		{
			db.WorkerNodes.Add(new WorkerNode
			{
				Name = "Claude Code Bootstrap",
				MachineName = "claude-code-bootstrap",
				AgentVersion = "claude-code",
				OperatingSystem = "Windows",
				MaxConcurrency = 4,
				CurrentConcurrency = 0,
				Status = WorkerStatus.Idle,
				Capabilities = JsonSerializer.Serialize(new WorkerCapability[1] { WorkerCapability.Code }),
				ApiKeyHash = hash,
				IsActive = true,
				QueueName = "claude"
			});
			logger.LogInformation("Seeded WorkerNode 'claude-code-bootstrap' for /claude task system.");
		}
		else if (existing.ApiKeyHash != hash)
		{
			existing.ApiKeyHash = hash;
			existing.IsActive = true;
			logger.LogInformation("Rotated CLAUDE_WORKER_TOKEN hash on claude-code-bootstrap WorkerNode.");
		}
	}

	private static async Task SeedClaudeSettingsAsync(MADaiDbContext db)
	{
		(string Key, string Value, string Description)[] defaults = new(string, string, string)[3]
		{
			("claudeWorkerActive", "true", "Master switch for the /claude polling worker."),
			("claudeScannerActive", "true", "Master switch for the hourly /claude codebase scanner."),
			("claudeDeployNext", "false", "If true, the next worker iteration runs deploy-api.ps1 after completing its batch.")
		};
		List<string> existing = await (from s in db.SystemSettings
			where defaults.Select(((string, string, string) d) => d.Item1).Contains(s.Key)
			select s.Key).ToListAsync();
		(string, string, string)[] array = defaults;
		for (int i = 0; i < array.Length; i++)
		{
			(string, string, string) d2 = array[i];
			if (!existing.Contains(d2.Item1))
			{
				db.SystemSettings.Add(new SystemSetting
				{
					Key = d2.Item1,
					Value = d2.Item2,
					Category = "Claude",
					DataType = "bool",
					Description = d2.Item3
				});
			}
		}
	}

	private static async Task GrantRolePermissions(MADaiDbContext db, RoleManager<ApplicationRole> roleManager, string roleName, string[] codes)
	{
		string[] codes2 = codes;
		ApplicationRole role = await roleManager.FindByNameAsync(roleName);
		if (role == null)
		{
			return;
		}
		List<Guid> existing = await (from rp in db.RolePermissions
			where rp.RoleId == role.Id
			select rp.PermissionId).ToListAsync();
		foreach (Permission p2 in await db.Permissions.Where((Permission p) => codes2.Contains(p.Code)).ToListAsync())
		{
			if (!existing.Contains(p2.Id))
			{
				db.RolePermissions.Add(new RolePermission
				{
					RoleId = role.Id,
					PermissionId = p2.Id
				});
			}
		}
		await db.SaveChangesAsync();
	}
}
