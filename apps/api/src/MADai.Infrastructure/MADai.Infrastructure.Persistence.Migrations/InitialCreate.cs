using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace MADai.Infrastructure.Persistence.Migrations;

[DbContext(typeof(MADaiDbContext))]
[Migration("20260520164655_InitialCreate")]
public class InitialCreate : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable("AuditLogs", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id50 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> timestamp4 = table.Column<DateTime>("datetime2");
			OperationBuilder<AddColumnOperation> actorUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> companyId8 = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true);
			int? maxLength48 = 120;
			OperationBuilder<AddColumnOperation> action2 = table.Column<string>("nvarchar(120)", null, maxLength48);
			maxLength48 = 120;
			OperationBuilder<AddColumnOperation> entityType = table.Column<string>("nvarchar(120)", null, maxLength48);
			maxLength48 = 60;
			OperationBuilder<AddColumnOperation> entityId = table.Column<string>("nvarchar(60)", null, maxLength48, rowVersion: false, null, nullable: true);
			maxLength48 = 64;
			OperationBuilder<AddColumnOperation> ipAddress4 = table.Column<string>("nvarchar(64)", null, maxLength48, rowVersion: false, null, nullable: true);
			maxLength48 = 4000;
			OperationBuilder<AddColumnOperation> detail = table.Column<string>("nvarchar(4000)", null, maxLength48, rowVersion: false, null, nullable: true);
			maxLength48 = 20;
			return new
			{
				Id = id50,
				Timestamp = timestamp4,
				ActorUserId = actorUserId,
				CompanyId = companyId8,
				Action = action2,
				EntityType = entityType,
				EntityId = entityId,
				IpAddress = ipAddress4,
				Detail = detail,
				Severity = table.Column<string>("nvarchar(20)", null, maxLength48)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_AuditLogs", x => x.Id);
		});
		migrationBuilder.CreateTable("AuditRuns", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id49 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> startedAt4 = table.Column<DateTime>("datetime2");
			OperationBuilder<AddColumnOperation> completedAt3 = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> scanType2 = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> findingsCount = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> recommendationsCount = table.Column<int>("int");
			int? maxLength47 = 2000;
			OperationBuilder<AddColumnOperation> summary = table.Column<string>("nvarchar(2000)", null, maxLength47, rowVersion: false, null, nullable: true);
			maxLength47 = 40;
			return new
			{
				Id = id49,
				StartedAt = startedAt4,
				CompletedAt = completedAt3,
				ScanType = scanType2,
				FindingsCount = findingsCount,
				RecommendationsCount = recommendationsCount,
				Summary = summary,
				Status = table.Column<string>("nvarchar(40)", null, maxLength47),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_AuditRuns", x => x.Id);
		});
		migrationBuilder.CreateTable("CleanupTasks", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id48 = table.Column<Guid>("uniqueidentifier");
			int? maxLength46 = 200;
			OperationBuilder<AddColumnOperation> target = table.Column<string>("nvarchar(200)", null, maxLength46);
			OperationBuilder<AddColumnOperation> reclaimableBytes = table.Column<long>("bigint");
			OperationBuilder<AddColumnOperation> itemCount = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> status9 = table.Column<string>("nvarchar(max)");
			OperationBuilder<AddColumnOperation> executedAt = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true);
			maxLength46 = 2000;
			return new
			{
				Id = id48,
				Target = target,
				ReclaimableBytes = reclaimableBytes,
				ItemCount = itemCount,
				Status = status9,
				ExecutedAt = executedAt,
				Result = table.Column<string>("nvarchar(2000)", null, maxLength46, rowVersion: false, null, nullable: true),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_CleanupTasks", x => x.Id);
		});
		migrationBuilder.CreateTable("Companies", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id47 = table.Column<Guid>("uniqueidentifier");
			int? maxLength45 = 200;
			OperationBuilder<AddColumnOperation> name12 = table.Column<string>("nvarchar(200)", null, maxLength45);
			maxLength45 = 80;
			OperationBuilder<AddColumnOperation> slug = table.Column<string>("nvarchar(80)", null, maxLength45);
			maxLength45 = 200;
			OperationBuilder<AddColumnOperation> legalName = table.Column<string>("nvarchar(200)", null, maxLength45, rowVersion: false, null, nullable: true);
			maxLength45 = 254;
			OperationBuilder<AddColumnOperation> contactEmail = table.Column<string>("nvarchar(254)", null, maxLength45, rowVersion: false, null, nullable: true);
			maxLength45 = 40;
			OperationBuilder<AddColumnOperation> contactPhone = table.Column<string>("nvarchar(40)", null, maxLength45, rowVersion: false, null, nullable: true);
			maxLength45 = 200;
			OperationBuilder<AddColumnOperation> website = table.Column<string>("nvarchar(200)", null, maxLength45, rowVersion: false, null, nullable: true);
			maxLength45 = 80;
			OperationBuilder<AddColumnOperation> country2 = table.Column<string>("nvarchar(80)", null, maxLength45, rowVersion: false, null, nullable: true);
			maxLength45 = 80;
			return new
			{
				Id = id47,
				Name = name12,
				Slug = slug,
				LegalName = legalName,
				ContactEmail = contactEmail,
				ContactPhone = contactPhone,
				Website = website,
				Country = country2,
				Timezone = table.Column<string>("nvarchar(80)", null, maxLength45, rowVersion: false, null, nullable: true),
				IsActive = table.Column<bool>("bit"),
				PlanId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_Companies", x => x.Id);
		});
		migrationBuilder.CreateTable("CompanyPlans", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id46 = table.Column<Guid>("uniqueidentifier");
			int? maxLength44 = 120;
			OperationBuilder<AddColumnOperation> name11 = table.Column<string>("nvarchar(120)", null, maxLength44);
			maxLength44 = 40;
			OperationBuilder<AddColumnOperation> code4 = table.Column<string>("nvarchar(40)", null, maxLength44);
			maxLength44 = 18;
			int? scale5 = 2;
			OperationBuilder<AddColumnOperation> monthlyPrice2 = table.Column<decimal>("decimal(18,2)", null, null, rowVersion: false, null, nullable: false, null, null, null, null, null, null, maxLength44, scale5);
			scale5 = 18;
			maxLength44 = 2;
			return new
			{
				Id = id46,
				Name = name11,
				Code = code4,
				MonthlyPrice = monthlyPrice2,
				AnnualPrice = table.Column<decimal>("decimal(18,2)", null, null, rowVersion: false, null, nullable: false, null, null, null, null, null, null, scale5, maxLength44),
				MaxConcurrentTasks = table.Column<int>("int"),
				MaxStorageGb = table.Column<int>("int"),
				MaxWorkers = table.Column<int>("int"),
				MaxUsers = table.Column<int>("int"),
				IncludesSelfHealing = table.Column<bool>("bit"),
				IncludesPrioritySupport = table.Column<bool>("bit"),
				Features = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true),
				IsActive = table.Column<bool>("bit"),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_CompanyPlans", x => x.Id);
		});
		migrationBuilder.CreateTable("CreditLedgers", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id45 = table.Column<Guid>("uniqueidentifier");
			int? precision2 = 18;
			int? scale4 = 2;
			OperationBuilder<AddColumnOperation> amount2 = table.Column<decimal>("decimal(18,2)", null, null, rowVersion: false, null, nullable: false, null, null, null, null, null, null, precision2, scale4);
			scale4 = 10;
			OperationBuilder<AddColumnOperation> currency2 = table.Column<string>("nvarchar(10)", null, scale4);
			scale4 = 200;
			OperationBuilder<AddColumnOperation> reason2 = table.Column<string>("nvarchar(200)", null, scale4);
			scale4 = 200;
			return new
			{
				Id = id45,
				Amount = amount2,
				Currency = currency2,
				Reason = reason2,
				Reference = table.Column<string>("nvarchar(200)", null, scale4, rowVersion: false, null, nullable: true),
				OccurredAt = table.Column<DateTime>("datetime2"),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true),
				CompanyId = table.Column<Guid>("uniqueidentifier")
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_CreditLedgers", x => x.Id);
		});
		migrationBuilder.CreateTable("FeatureFlags", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id44 = table.Column<Guid>("uniqueidentifier");
			int? maxLength43 = 120;
			OperationBuilder<AddColumnOperation> key2 = table.Column<string>("nvarchar(120)", null, maxLength43);
			maxLength43 = 200;
			OperationBuilder<AddColumnOperation> name10 = table.Column<string>("nvarchar(200)", null, maxLength43);
			maxLength43 = 1000;
			OperationBuilder<AddColumnOperation> description6 = table.Column<string>("nvarchar(1000)", null, maxLength43, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> isEnabled = table.Column<bool>("bit");
			maxLength43 = 500;
			return new
			{
				Id = id44,
				Key = key2,
				Name = name10,
				Description = description6,
				IsEnabled = isEnabled,
				Audience = table.Column<string>("nvarchar(500)", null, maxLength43, rowVersion: false, null, nullable: true),
				Configuration = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_FeatureFlags", x => x.Id);
		});
		migrationBuilder.CreateTable("FileFolders", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id43 = table.Column<Guid>("uniqueidentifier");
			int? maxLength42 = 200;
			OperationBuilder<AddColumnOperation> name9 = table.Column<string>("nvarchar(200)", null, maxLength42);
			OperationBuilder<AddColumnOperation> parentFolderId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true);
			maxLength42 = 1000;
			return new
			{
				Id = id43,
				Name = name9,
				ParentFolderId = parentFolderId,
				Path = table.Column<string>("nvarchar(1000)", null, maxLength42, rowVersion: false, null, nullable: true),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true),
				CompanyId = table.Column<Guid>("uniqueidentifier")
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_FileFolders", x => x.Id);
			table.ForeignKey("FK_FileFolders_FileFolders_ParentFolderId", x => x.ParentFolderId, "FileFolders", "Id");
		});
		migrationBuilder.CreateTable("NotificationHistory", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id42 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> userId8 = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> companyId7 = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true);
			int? maxLength41 = 80;
			OperationBuilder<AddColumnOperation> templateCode = table.Column<string>("nvarchar(80)", null, maxLength41);
			maxLength41 = 40;
			OperationBuilder<AddColumnOperation> channel3 = table.Column<string>("nvarchar(40)", null, maxLength41);
			OperationBuilder<AddColumnOperation> sentAt = table.Column<DateTime>("datetime2");
			maxLength41 = 254;
			OperationBuilder<AddColumnOperation> recipient = table.Column<string>("nvarchar(254)", null, maxLength41);
			maxLength41 = 40;
			OperationBuilder<AddColumnOperation> status8 = table.Column<string>("nvarchar(40)", null, maxLength41);
			maxLength41 = 2000;
			return new
			{
				Id = id42,
				UserId = userId8,
				CompanyId = companyId7,
				TemplateCode = templateCode,
				Channel = channel3,
				SentAt = sentAt,
				Recipient = recipient,
				Status = status8,
				ErrorMessage = table.Column<string>("nvarchar(2000)", null, maxLength41, rowVersion: false, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_NotificationHistory", x => x.Id);
		});
		migrationBuilder.CreateTable("NotificationTemplates", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id41 = table.Column<Guid>("uniqueidentifier");
			int? maxLength40 = 80;
			OperationBuilder<AddColumnOperation> code3 = table.Column<string>("nvarchar(80)", null, maxLength40);
			maxLength40 = 200;
			OperationBuilder<AddColumnOperation> name8 = table.Column<string>("nvarchar(200)", null, maxLength40);
			maxLength40 = 40;
			OperationBuilder<AddColumnOperation> channel2 = table.Column<string>("nvarchar(40)", null, maxLength40);
			maxLength40 = 200;
			return new
			{
				Id = id41,
				Code = code3,
				Name = name8,
				Channel = channel2,
				Subject = table.Column<string>("nvarchar(200)", null, maxLength40),
				Body = table.Column<string>("nvarchar(max)"),
				IsActive = table.Column<bool>("bit"),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_NotificationTemplates", x => x.Id);
		});
		migrationBuilder.CreateTable("OptimizationSuggestions", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id40 = table.Column<Guid>("uniqueidentifier");
			int? maxLength39 = 80;
			OperationBuilder<AddColumnOperation> area = table.Column<string>("nvarchar(80)", null, maxLength39);
			maxLength39 = 200;
			OperationBuilder<AddColumnOperation> title6 = table.Column<string>("nvarchar(200)", null, maxLength39);
			maxLength39 = 4000;
			return new
			{
				Id = id40,
				Area = area,
				Title = title6,
				Description = table.Column<string>("nvarchar(4000)", null, maxLength39),
				EstimatedImpact = table.Column<double>("float"),
				Status = table.Column<string>("nvarchar(max)"),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_OptimizationSuggestions", x => x.Id);
		});
		migrationBuilder.CreateTable("Permissions", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id39 = table.Column<Guid>("uniqueidentifier");
			int? maxLength38 = 120;
			OperationBuilder<AddColumnOperation> code2 = table.Column<string>("nvarchar(120)", null, maxLength38);
			maxLength38 = 200;
			OperationBuilder<AddColumnOperation> displayName = table.Column<string>("nvarchar(200)", null, maxLength38);
			maxLength38 = 500;
			OperationBuilder<AddColumnOperation> description5 = table.Column<string>("nvarchar(500)", null, maxLength38, rowVersion: false, null, nullable: true);
			maxLength38 = 80;
			return new
			{
				Id = id39,
				Code = code2,
				DisplayName = displayName,
				Description = description5,
				Category = table.Column<string>("nvarchar(80)", null, maxLength38)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_Permissions", x => x.Id);
		});
		migrationBuilder.CreateTable("Plans", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id38 = table.Column<Guid>("uniqueidentifier");
			int? maxLength37 = 120;
			OperationBuilder<AddColumnOperation> name7 = table.Column<string>("nvarchar(120)", null, maxLength37);
			maxLength37 = 40;
			OperationBuilder<AddColumnOperation> code = table.Column<string>("nvarchar(40)", null, maxLength37);
			OperationBuilder<AddColumnOperation> description4 = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true);
			maxLength37 = 18;
			int? scale3 = 2;
			OperationBuilder<AddColumnOperation> monthlyPrice = table.Column<decimal>("decimal(18,2)", null, null, rowVersion: false, null, nullable: false, null, null, null, null, null, null, maxLength37, scale3);
			scale3 = 18;
			maxLength37 = 2;
			OperationBuilder<AddColumnOperation> annualPrice = table.Column<decimal>("decimal(18,2)", null, null, rowVersion: false, null, nullable: false, null, null, null, null, null, null, scale3, maxLength37);
			maxLength37 = 10;
			return new
			{
				Id = id38,
				Name = name7,
				Code = code,
				Description = description4,
				MonthlyPrice = monthlyPrice,
				AnnualPrice = annualPrice,
				Currency = table.Column<string>("nvarchar(10)", null, maxLength37),
				IncludedTasks = table.Column<int>("int"),
				IncludedStorageGb = table.Column<int>("int"),
				IncludedWorkers = table.Column<int>("int"),
				IsPublic = table.Column<bool>("bit"),
				IsActive = table.Column<bool>("bit"),
				FeaturesJson = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_Plans", x => x.Id);
		});
		migrationBuilder.CreateTable("Roles", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id37 = table.Column<Guid>("uniqueidentifier");
			int? maxLength36 = 400;
			OperationBuilder<AddColumnOperation> description3 = table.Column<string>("nvarchar(400)", null, maxLength36, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> isSystem = table.Column<bool>("bit");
			maxLength36 = 256;
			OperationBuilder<AddColumnOperation> name6 = table.Column<string>("nvarchar(256)", null, maxLength36, rowVersion: false, null, nullable: true);
			maxLength36 = 256;
			return new
			{
				Id = id37,
				Description = description3,
				IsSystem = isSystem,
				Name = name6,
				NormalizedName = table.Column<string>("nvarchar(256)", null, maxLength36, rowVersion: false, null, nullable: true),
				ConcurrencyStamp = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_Roles", x => x.Id);
		});
		migrationBuilder.CreateTable("SystemSettings", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id36 = table.Column<Guid>("uniqueidentifier");
			int? maxLength35 = 120;
			OperationBuilder<AddColumnOperation> key = table.Column<string>("nvarchar(120)", null, maxLength35);
			maxLength35 = 80;
			OperationBuilder<AddColumnOperation> category3 = table.Column<string>("nvarchar(80)", null, maxLength35);
			OperationBuilder<AddColumnOperation> value2 = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true);
			maxLength35 = 40;
			OperationBuilder<AddColumnOperation> dataType = table.Column<string>("nvarchar(40)", null, maxLength35, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> isSecret = table.Column<bool>("bit");
			maxLength35 = 1000;
			return new
			{
				Id = id36,
				Key = key,
				Category = category3,
				Value = value2,
				DataType = dataType,
				IsSecret = isSecret,
				Description = table.Column<string>("nvarchar(1000)", null, maxLength35, rowVersion: false, null, nullable: true),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_SystemSettings", x => x.Id);
		});
		migrationBuilder.CreateTable("TaskTags", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id35 = table.Column<Guid>("uniqueidentifier");
			int? maxLength34 = 80;
			OperationBuilder<AddColumnOperation> name5 = table.Column<string>("nvarchar(80)", null, maxLength34);
			maxLength34 = 20;
			return new
			{
				Id = id35,
				Name = name5,
				Color = table.Column<string>("nvarchar(20)", null, maxLength34),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true),
				CompanyId = table.Column<Guid>("uniqueidentifier")
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_TaskTags", x => x.Id);
		});
		migrationBuilder.CreateTable("TaskTemplates", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id34 = table.Column<Guid>("uniqueidentifier");
			int? maxLength33 = 120;
			OperationBuilder<AddColumnOperation> name4 = table.Column<string>("nvarchar(120)", null, maxLength33);
			maxLength33 = 2000;
			OperationBuilder<AddColumnOperation> description2 = table.Column<string>("nvarchar(2000)", null, maxLength33, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> category2 = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> defaultPriority = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> promptTemplate = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> defaultInputJson = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true);
			maxLength33 = 80;
			OperationBuilder<AddColumnOperation> queueName3 = table.Column<string>("nvarchar(80)", null, maxLength33, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> defaultTimeoutSeconds = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> defaultMaxRetries = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> isPublic = table.Column<bool>("bit");
			maxLength33 = 500;
			return new
			{
				Id = id34,
				Name = name4,
				Description = description2,
				Category = category2,
				DefaultPriority = defaultPriority,
				PromptTemplate = promptTemplate,
				DefaultInputJson = defaultInputJson,
				QueueName = queueName3,
				DefaultTimeoutSeconds = defaultTimeoutSeconds,
				DefaultMaxRetries = defaultMaxRetries,
				IsPublic = isPublic,
				RequiredCapabilities = table.Column<string>("nvarchar(500)", null, maxLength33, rowVersion: false, null, nullable: true),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true),
				CompanyId = table.Column<Guid>("uniqueidentifier")
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_TaskTemplates", x => x.Id);
		});
		migrationBuilder.CreateTable("UsageRecords", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id33 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> periodStart = table.Column<DateTime>("datetime2");
			OperationBuilder<AddColumnOperation> periodEnd = table.Column<DateTime>("datetime2");
			int? maxLength32 = 80;
			OperationBuilder<AddColumnOperation> metricName2 = table.Column<string>("nvarchar(80)", null, maxLength32);
			OperationBuilder<AddColumnOperation> quantity = table.Column<double>("float");
			maxLength32 = 500;
			return new
			{
				Id = id33,
				PeriodStart = periodStart,
				PeriodEnd = periodEnd,
				MetricName = metricName2,
				Quantity = quantity,
				Notes = table.Column<string>("nvarchar(500)", null, maxLength32, rowVersion: false, null, nullable: true),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true),
				CompanyId = table.Column<Guid>("uniqueidentifier")
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_UsageRecords", x => x.Id);
		});
		migrationBuilder.CreateTable("WebhookEndpoints", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id32 = table.Column<Guid>("uniqueidentifier");
			int? maxLength31 = 500;
			OperationBuilder<AddColumnOperation> url2 = table.Column<string>("nvarchar(500)", null, maxLength31);
			maxLength31 = 128;
			OperationBuilder<AddColumnOperation> secret = table.Column<string>("nvarchar(128)", null, maxLength31);
			maxLength31 = 1000;
			return new
			{
				Id = id32,
				Url = url2,
				Secret = secret,
				EventsCsv = table.Column<string>("nvarchar(1000)", null, maxLength31, rowVersion: false, null, nullable: true),
				IsActive = table.Column<bool>("bit"),
				LastSuccessAt = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				LastFailureAt = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				ConsecutiveFailures = table.Column<int>("int"),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true),
				CompanyId = table.Column<Guid>("uniqueidentifier")
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_WebhookEndpoints", x => x.Id);
		});
		migrationBuilder.CreateTable("WorkerNodes", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id31 = table.Column<Guid>("uniqueidentifier");
			int? maxLength30 = 120;
			OperationBuilder<AddColumnOperation> name3 = table.Column<string>("nvarchar(120)", null, maxLength30);
			maxLength30 = 200;
			OperationBuilder<AddColumnOperation> machineName = table.Column<string>("nvarchar(200)", null, maxLength30);
			maxLength30 = 40;
			OperationBuilder<AddColumnOperation> agentVersion = table.Column<string>("nvarchar(40)", null, maxLength30, rowVersion: false, null, nullable: true);
			maxLength30 = 200;
			OperationBuilder<AddColumnOperation> operatingSystem = table.Column<string>("nvarchar(200)", null, maxLength30, rowVersion: false, null, nullable: true);
			maxLength30 = 64;
			OperationBuilder<AddColumnOperation> ipAddress3 = table.Column<string>("nvarchar(64)", null, maxLength30, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> status7 = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> companyId6 = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true);
			maxLength30 = 80;
			OperationBuilder<AddColumnOperation> queueName2 = table.Column<string>("nvarchar(80)", null, maxLength30, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> maxConcurrency = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> currentConcurrency = table.Column<int>("int");
			maxLength30 = 2000;
			OperationBuilder<AddColumnOperation> capabilities = table.Column<string>("nvarchar(2000)", null, maxLength30, rowVersion: false, null, nullable: true);
			maxLength30 = 2000;
			OperationBuilder<AddColumnOperation> labels = table.Column<string>("nvarchar(2000)", null, maxLength30, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> lastHeartbeatAt = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> startedAt3 = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true);
			maxLength30 = 200;
			OperationBuilder<AddColumnOperation> apiKeyHash = table.Column<string>("nvarchar(200)", null, maxLength30);
			OperationBuilder<AddColumnOperation> isActive2 = table.Column<bool>("bit");
			maxLength30 = 500;
			return new
			{
				Id = id31,
				Name = name3,
				MachineName = machineName,
				AgentVersion = agentVersion,
				OperatingSystem = operatingSystem,
				IpAddress = ipAddress3,
				Status = status7,
				CompanyId = companyId6,
				QueueName = queueName2,
				MaxConcurrency = maxConcurrency,
				CurrentConcurrency = currentConcurrency,
				Capabilities = capabilities,
				Labels = labels,
				LastHeartbeatAt = lastHeartbeatAt,
				StartedAt = startedAt3,
				ApiKeyHash = apiKeyHash,
				IsActive = isActive2,
				WorkspaceRoot = table.Column<string>("nvarchar(500)", null, maxLength30, rowVersion: false, null, nullable: true),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_WorkerNodes", x => x.Id);
		});
		migrationBuilder.CreateTable("WorkerQueues", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id30 = table.Column<Guid>("uniqueidentifier");
			int? maxLength29 = 80;
			return new
			{
				Id = id30,
				Name = table.Column<string>("nvarchar(80)", null, maxLength29),
				Description = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true),
				CompanyId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				Priority = table.Column<int>("int"),
				IsActive = table.Column<bool>("bit"),
				MaxParallelism = table.Column<int>("int"),
				RequiredCapabilities = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_WorkerQueues", x => x.Id);
		});
		migrationBuilder.CreateTable("AuditRecommendations", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id29 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> auditRunId2 = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true);
			int? maxLength28 = 200;
			OperationBuilder<AddColumnOperation> title5 = table.Column<string>("nvarchar(200)", null, maxLength28);
			maxLength28 = 4000;
			return new
			{
				Id = id29,
				AuditRunId = auditRunId2,
				Title = title5,
				Body = table.Column<string>("nvarchar(4000)", null, maxLength28),
				Severity = table.Column<int>("int"),
				Status = table.Column<string>("nvarchar(max)"),
				GeneratedTaskId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_AuditRecommendations", x => x.Id);
			table.ForeignKey("FK_AuditRecommendations_AuditRuns_AuditRunId", x => x.AuditRunId, "AuditRuns", "Id");
		});
		migrationBuilder.CreateTable("CompanyBranding", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id28 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> companyId5 = table.Column<Guid>("uniqueidentifier");
			int? maxLength27 = 500;
			OperationBuilder<AddColumnOperation> logoUrl = table.Column<string>("nvarchar(500)", null, maxLength27, rowVersion: false, null, nullable: true);
			maxLength27 = 500;
			OperationBuilder<AddColumnOperation> faviconUrl = table.Column<string>("nvarchar(500)", null, maxLength27, rowVersion: false, null, nullable: true);
			maxLength27 = 20;
			OperationBuilder<AddColumnOperation> primaryColor = table.Column<string>("nvarchar(20)", null, maxLength27);
			maxLength27 = 20;
			OperationBuilder<AddColumnOperation> accentColor = table.Column<string>("nvarchar(20)", null, maxLength27);
			maxLength27 = 20;
			OperationBuilder<AddColumnOperation> backgroundColor = table.Column<string>("nvarchar(20)", null, maxLength27);
			OperationBuilder<AddColumnOperation> customCss = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true);
			maxLength27 = 20;
			OperationBuilder<AddColumnOperation> themeMode = table.Column<string>("nvarchar(20)", null, maxLength27);
			maxLength27 = 120;
			OperationBuilder<AddColumnOperation> emailFromName = table.Column<string>("nvarchar(120)", null, maxLength27, rowVersion: false, null, nullable: true);
			maxLength27 = 254;
			return new
			{
				Id = id28,
				CompanyId = companyId5,
				LogoUrl = logoUrl,
				FaviconUrl = faviconUrl,
				PrimaryColor = primaryColor,
				AccentColor = accentColor,
				BackgroundColor = backgroundColor,
				CustomCss = customCss,
				ThemeMode = themeMode,
				EmailFromName = emailFromName,
				EmailFromAddress = table.Column<string>("nvarchar(254)", null, maxLength27, rowVersion: false, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_CompanyBranding", x => x.Id);
			table.ForeignKey("FK_CompanyBranding_Companies_CompanyId", x => x.CompanyId, "Companies", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("CompanySettings", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id27 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> companyId4 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> maxConcurrentTasks = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> maxStorageGb = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> maxWorkers = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> taskRetentionDays = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> enableSelfHealing = table.Column<bool>("bit");
			OperationBuilder<AddColumnOperation> enableAutoRetry = table.Column<bool>("bit");
			OperationBuilder<AddColumnOperation> defaultTaskTimeoutMinutes = table.Column<int>("int");
			int? maxLength26 = 128;
			OperationBuilder<AddColumnOperation> webhookSecret = table.Column<string>("nvarchar(128)", null, maxLength26, rowVersion: false, null, nullable: true);
			maxLength26 = 200;
			return new
			{
				Id = id27,
				CompanyId = companyId4,
				MaxConcurrentTasks = maxConcurrentTasks,
				MaxStorageGb = maxStorageGb,
				MaxWorkers = maxWorkers,
				TaskRetentionDays = taskRetentionDays,
				EnableSelfHealing = enableSelfHealing,
				EnableAutoRetry = enableAutoRetry,
				DefaultTaskTimeoutMinutes = defaultTaskTimeoutMinutes,
				WebhookSecret = webhookSecret,
				CustomDomain = table.Column<string>("nvarchar(200)", null, maxLength26, rowVersion: false, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_CompanySettings", x => x.Id);
			table.ForeignKey("FK_CompanySettings_Companies_CompanyId", x => x.CompanyId, "Companies", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("Users", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id26 = table.Column<Guid>("uniqueidentifier");
			int? maxLength25 = 80;
			OperationBuilder<AddColumnOperation> firstName = table.Column<string>("nvarchar(80)", null, maxLength25, rowVersion: false, null, nullable: true);
			maxLength25 = 80;
			OperationBuilder<AddColumnOperation> lastName = table.Column<string>("nvarchar(80)", null, maxLength25, rowVersion: false, null, nullable: true);
			maxLength25 = 500;
			OperationBuilder<AddColumnOperation> avatarUrl = table.Column<string>("nvarchar(500)", null, maxLength25, rowVersion: false, null, nullable: true);
			maxLength25 = 80;
			OperationBuilder<AddColumnOperation> timeZone = table.Column<string>("nvarchar(80)", null, maxLength25, rowVersion: false, null, nullable: true);
			maxLength25 = 20;
			OperationBuilder<AddColumnOperation> locale = table.Column<string>("nvarchar(20)", null, maxLength25, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> isActive = table.Column<bool>("bit");
			OperationBuilder<AddColumnOperation> isMfaEnrolled = table.Column<bool>("bit");
			OperationBuilder<AddColumnOperation> createdDate = table.Column<DateTime>("datetime2");
			OperationBuilder<AddColumnOperation> lastLoginAt = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true);
			maxLength25 = 64;
			OperationBuilder<AddColumnOperation> lastLoginIp = table.Column<string>("nvarchar(64)", null, maxLength25, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> companyId3 = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> isDeleted = table.Column<bool>("bit");
			OperationBuilder<AddColumnOperation> deletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true);
			maxLength25 = 256;
			OperationBuilder<AddColumnOperation> userName = table.Column<string>("nvarchar(256)", null, maxLength25, rowVersion: false, null, nullable: true);
			maxLength25 = 256;
			OperationBuilder<AddColumnOperation> normalizedUserName = table.Column<string>("nvarchar(256)", null, maxLength25, rowVersion: false, null, nullable: true);
			maxLength25 = 256;
			OperationBuilder<AddColumnOperation> email = table.Column<string>("nvarchar(256)", null, maxLength25, rowVersion: false, null, nullable: true);
			maxLength25 = 256;
			return new
			{
				Id = id26,
				FirstName = firstName,
				LastName = lastName,
				AvatarUrl = avatarUrl,
				TimeZone = timeZone,
				Locale = locale,
				IsActive = isActive,
				IsMfaEnrolled = isMfaEnrolled,
				CreatedDate = createdDate,
				LastLoginAt = lastLoginAt,
				LastLoginIp = lastLoginIp,
				CompanyId = companyId3,
				IsDeleted = isDeleted,
				DeletedDate = deletedDate,
				UserName = userName,
				NormalizedUserName = normalizedUserName,
				Email = email,
				NormalizedEmail = table.Column<string>("nvarchar(256)", null, maxLength25, rowVersion: false, null, nullable: true),
				EmailConfirmed = table.Column<bool>("bit"),
				PasswordHash = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true),
				SecurityStamp = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true),
				ConcurrencyStamp = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true),
				PhoneNumber = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true),
				PhoneNumberConfirmed = table.Column<bool>("bit"),
				TwoFactorEnabled = table.Column<bool>("bit"),
				LockoutEnd = table.Column<DateTimeOffset>("datetimeoffset", null, null, rowVersion: false, null, nullable: true),
				LockoutEnabled = table.Column<bool>("bit"),
				AccessFailedCount = table.Column<int>("int")
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_Users", x => x.Id);
			table.ForeignKey("FK_Users_Companies_CompanyId", x => x.CompanyId, "Companies", "Id");
		});
		migrationBuilder.CreateTable("Files", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id25 = table.Column<Guid>("uniqueidentifier");
			int? maxLength24 = 255;
			OperationBuilder<AddColumnOperation> name2 = table.Column<string>("nvarchar(255)", null, maxLength24);
			maxLength24 = 120;
			OperationBuilder<AddColumnOperation> contentType2 = table.Column<string>("nvarchar(120)", null, maxLength24);
			OperationBuilder<AddColumnOperation> sizeBytes3 = table.Column<long>("bigint");
			maxLength24 = 500;
			OperationBuilder<AddColumnOperation> storagePath3 = table.Column<string>("nvarchar(500)", null, maxLength24);
			maxLength24 = 40;
			OperationBuilder<AddColumnOperation> storageProvider2 = table.Column<string>("nvarchar(40)", null, maxLength24);
			maxLength24 = 128;
			OperationBuilder<AddColumnOperation> checksum2 = table.Column<string>("nvarchar(128)", null, maxLength24, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> folderId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true);
			maxLength24 = 500;
			OperationBuilder<AddColumnOperation> thumbnailPath2 = table.Column<string>("nvarchar(500)", null, maxLength24, rowVersion: false, null, nullable: true);
			maxLength24 = 500;
			return new
			{
				Id = id25,
				Name = name2,
				ContentType = contentType2,
				SizeBytes = sizeBytes3,
				StoragePath = storagePath3,
				StorageProvider = storageProvider2,
				Checksum = checksum2,
				FolderId = folderId,
				ThumbnailPath = thumbnailPath2,
				PreviewPath = table.Column<string>("nvarchar(500)", null, maxLength24, rowVersion: false, null, nullable: true),
				Version = table.Column<int>("int"),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true),
				CompanyId = table.Column<Guid>("uniqueidentifier")
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_Files", x => x.Id);
			table.ForeignKey("FK_Files_FileFolders_FolderId", x => x.FolderId, "FileFolders", "Id");
		});
		migrationBuilder.CreateTable("Subscriptions", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id24 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> planId = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> status6 = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> startDate = table.Column<DateTime>("datetime2");
			OperationBuilder<AddColumnOperation> currentPeriodEnd = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> cancelledAt2 = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true);
			int? maxLength23 = 20;
			OperationBuilder<AddColumnOperation> billingCycle = table.Column<string>("nvarchar(20)", null, maxLength23);
			maxLength23 = 200;
			return new
			{
				Id = id24,
				PlanId = planId,
				Status = status6,
				StartDate = startDate,
				CurrentPeriodEnd = currentPeriodEnd,
				CancelledAt = cancelledAt2,
				BillingCycle = billingCycle,
				ExternalReference = table.Column<string>("nvarchar(200)", null, maxLength23, rowVersion: false, null, nullable: true),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true),
				CompanyId = table.Column<Guid>("uniqueidentifier")
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_Subscriptions", x => x.Id);
			table.ForeignKey("FK_Subscriptions_Plans_PlanId", x => x.PlanId, "Plans", "Id");
		});
		migrationBuilder.CreateTable("RoleClaims", (ColumnsBuilder table) => new
		{
			Id = table.Column<int>("int").Annotation("SqlServer:Identity", "1, 1"),
			RoleId = table.Column<Guid>("uniqueidentifier"),
			ClaimType = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true),
			ClaimValue = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true)
		}, null, table =>
		{
			table.PrimaryKey("PK_RoleClaims", x => x.Id);
			table.ForeignKey("FK_RoleClaims_Roles_RoleId", x => x.RoleId, "Roles", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("RolePermissions", (ColumnsBuilder table) => new
		{
			Id = table.Column<Guid>("uniqueidentifier"),
			RoleId = table.Column<Guid>("uniqueidentifier"),
			PermissionId = table.Column<Guid>("uniqueidentifier")
		}, null, table =>
		{
			table.PrimaryKey("PK_RolePermissions", x => x.Id);
			table.ForeignKey("FK_RolePermissions_Permissions_PermissionId", x => x.PermissionId, "Permissions", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
			table.ForeignKey("FK_RolePermissions_Roles_RoleId", x => x.RoleId, "Roles", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("Tasks", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id23 = table.Column<Guid>("uniqueidentifier");
			int? maxLength22 = 200;
			OperationBuilder<AddColumnOperation> title4 = table.Column<string>("nvarchar(200)", null, maxLength22);
			maxLength22 = 8000;
			OperationBuilder<AddColumnOperation> description = table.Column<string>("nvarchar(max)", null, maxLength22, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> category = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> priority = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> status5 = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> origin = table.Column<int>("int");
			maxLength22 = 80;
			OperationBuilder<AddColumnOperation> queueName = table.Column<string>("nvarchar(80)", null, maxLength22, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> parentTaskId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> promptPayload = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> inputPayload = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true);
			maxLength22 = 4000;
			OperationBuilder<AddColumnOperation> outputSummary2 = table.Column<string>("nvarchar(4000)", null, maxLength22, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> resultPayload = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true);
			maxLength22 = 2000;
			OperationBuilder<AddColumnOperation> errorMessage = table.Column<string>("nvarchar(2000)", null, maxLength22, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> errorStack = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> progress = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> retryCount = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> maxRetries = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> timeoutSeconds = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> scheduledAt = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> claimedAt = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> startedAt2 = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> completedAt2 = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> cancelledAt = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> nextRetryAt = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> estimatedDuration = table.Column<TimeSpan>("time", null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> claimedByWorkerId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true);
			maxLength22 = 64;
			OperationBuilder<AddColumnOperation> claimToken = table.Column<string>("nvarchar(64)", null, maxLength22, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> templateId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true);
			maxLength22 = 2000;
			OperationBuilder<AddColumnOperation> tags = table.Column<string>("nvarchar(2000)", null, maxLength22, rowVersion: false, null, nullable: true);
			maxLength22 = 500;
			OperationBuilder<AddColumnOperation> workspacePath2 = table.Column<string>("nvarchar(500)", null, maxLength22, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> isRecurring = table.Column<bool>("bit");
			maxLength22 = 100;
			return new
			{
				Id = id23,
				Title = title4,
				Description = description,
				Category = category,
				Priority = priority,
				Status = status5,
				Origin = origin,
				QueueName = queueName,
				ParentTaskId = parentTaskId,
				PromptPayload = promptPayload,
				InputPayload = inputPayload,
				OutputSummary = outputSummary2,
				ResultPayload = resultPayload,
				ErrorMessage = errorMessage,
				ErrorStack = errorStack,
				Progress = progress,
				RetryCount = retryCount,
				MaxRetries = maxRetries,
				TimeoutSeconds = timeoutSeconds,
				ScheduledAt = scheduledAt,
				ClaimedAt = claimedAt,
				StartedAt = startedAt2,
				CompletedAt = completedAt2,
				CancelledAt = cancelledAt,
				NextRetryAt = nextRetryAt,
				EstimatedDuration = estimatedDuration,
				ClaimedByWorkerId = claimedByWorkerId,
				ClaimToken = claimToken,
				TemplateId = templateId,
				Tags = tags,
				WorkspacePath = workspacePath2,
				IsRecurring = isRecurring,
				CronExpression = table.Column<string>("nvarchar(100)", null, maxLength22, rowVersion: false, null, nullable: true),
				LastRunAt = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				IsDeadLetter = table.Column<bool>("bit"),
				ValidationReport = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true),
				CompanyId = table.Column<Guid>("uniqueidentifier")
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_Tasks", x => x.Id);
			table.ForeignKey("FK_Tasks_TaskTemplates_TemplateId", x => x.TemplateId, "TaskTemplates", "Id", null, ReferentialAction.NoAction, ReferentialAction.SetNull);
			table.ForeignKey("FK_Tasks_Tasks_ParentTaskId", x => x.ParentTaskId, "Tasks", "Id");
			table.ForeignKey("FK_Tasks_WorkerNodes_ClaimedByWorkerId", x => x.ClaimedByWorkerId, "WorkerNodes", "Id", null, ReferentialAction.NoAction, ReferentialAction.SetNull);
		});
		migrationBuilder.CreateTable("WorkerCapabilities", (ColumnsBuilder table) => new
		{
			Id = table.Column<Guid>("uniqueidentifier"),
			WorkerNodeId = table.Column<Guid>("uniqueidentifier"),
			Capability = table.Column<int>("int"),
			Detail = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true)
		}, null, table =>
		{
			table.PrimaryKey("PK_WorkerCapabilities", x => x.Id);
			table.ForeignKey("FK_WorkerCapabilities_WorkerNodes_WorkerNodeId", x => x.WorkerNodeId, "WorkerNodes", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("WorkerHeartbeats", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id22 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> workerNodeId3 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> timestamp3 = table.Column<DateTime>("datetime2");
			OperationBuilder<AddColumnOperation> status4 = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> activeTasks = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> cpuPercent = table.Column<double>("float");
			OperationBuilder<AddColumnOperation> memoryMb = table.Column<double>("float");
			OperationBuilder<AddColumnOperation> diskFreeGb = table.Column<double>("float");
			int? maxLength21 = 500;
			return new
			{
				Id = id22,
				WorkerNodeId = workerNodeId3,
				Timestamp = timestamp3,
				Status = status4,
				ActiveTasks = activeTasks,
				CpuPercent = cpuPercent,
				MemoryMb = memoryMb,
				DiskFreeGb = diskFreeGb,
				Notes = table.Column<string>("nvarchar(500)", null, maxLength21, rowVersion: false, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_WorkerHeartbeats", x => x.Id);
			table.ForeignKey("FK_WorkerHeartbeats_WorkerNodes_WorkerNodeId", x => x.WorkerNodeId, "WorkerNodes", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("WorkerMetrics", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id21 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> workerNodeId2 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> timestamp2 = table.Column<DateTime>("datetime2");
			int? maxLength20 = 80;
			OperationBuilder<AddColumnOperation> metricName = table.Column<string>("nvarchar(80)", null, maxLength20);
			OperationBuilder<AddColumnOperation> value = table.Column<double>("float");
			maxLength20 = 20;
			OperationBuilder<AddColumnOperation> unit = table.Column<string>("nvarchar(20)", null, maxLength20, rowVersion: false, null, nullable: true);
			maxLength20 = 500;
			return new
			{
				Id = id21,
				WorkerNodeId = workerNodeId2,
				Timestamp = timestamp2,
				MetricName = metricName,
				Value = value,
				Unit = unit,
				Tags = table.Column<string>("nvarchar(500)", null, maxLength20, rowVersion: false, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_WorkerMetrics", x => x.Id);
			table.ForeignKey("FK_WorkerMetrics_WorkerNodes_WorkerNodeId", x => x.WorkerNodeId, "WorkerNodes", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("ApiKeys", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id20 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> userId7 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> companyId2 = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true);
			int? maxLength19 = 120;
			OperationBuilder<AddColumnOperation> name = table.Column<string>("nvarchar(120)", null, maxLength19);
			maxLength19 = 20;
			OperationBuilder<AddColumnOperation> keyPrefix = table.Column<string>("nvarchar(20)", null, maxLength19);
			maxLength19 = 200;
			OperationBuilder<AddColumnOperation> keyHash = table.Column<string>("nvarchar(200)", null, maxLength19);
			OperationBuilder<AddColumnOperation> expiresAt3 = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> lastUsedAt = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true);
			maxLength19 = 1000;
			return new
			{
				Id = id20,
				UserId = userId7,
				CompanyId = companyId2,
				Name = name,
				KeyPrefix = keyPrefix,
				KeyHash = keyHash,
				ExpiresAt = expiresAt3,
				LastUsedAt = lastUsedAt,
				Scopes = table.Column<string>("nvarchar(1000)", null, maxLength19, rowVersion: false, null, nullable: true),
				IsRevoked = table.Column<bool>("bit"),
				RevokedAt = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_ApiKeys", x => x.Id);
			table.ForeignKey("FK_ApiKeys_Users_UserId", x => x.UserId, "Users", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("LoginHistory", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id19 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> userId6 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> loginAt = table.Column<DateTime>("datetime2");
			int? maxLength18 = 64;
			OperationBuilder<AddColumnOperation> ipAddress2 = table.Column<string>("nvarchar(64)", null, maxLength18, rowVersion: false, null, nullable: true);
			maxLength18 = 500;
			OperationBuilder<AddColumnOperation> userAgent2 = table.Column<string>("nvarchar(500)", null, maxLength18, rowVersion: false, null, nullable: true);
			maxLength18 = 200;
			OperationBuilder<AddColumnOperation> deviceFingerprint = table.Column<string>("nvarchar(200)", null, maxLength18, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> success = table.Column<bool>("bit");
			maxLength18 = 200;
			OperationBuilder<AddColumnOperation> failureReason2 = table.Column<string>("nvarchar(200)", null, maxLength18, rowVersion: false, null, nullable: true);
			maxLength18 = 80;
			OperationBuilder<AddColumnOperation> country = table.Column<string>("nvarchar(80)", null, maxLength18, rowVersion: false, null, nullable: true);
			maxLength18 = 120;
			return new
			{
				Id = id19,
				UserId = userId6,
				LoginAt = loginAt,
				IpAddress = ipAddress2,
				UserAgent = userAgent2,
				DeviceFingerprint = deviceFingerprint,
				Success = success,
				FailureReason = failureReason2,
				Country = country,
				City = table.Column<string>("nvarchar(120)", null, maxLength18, rowVersion: false, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_LoginHistory", x => x.Id);
			table.ForeignKey("FK_LoginHistory_Users_UserId", x => x.UserId, "Users", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("NotificationPreferences", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id18 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> userId5 = table.Column<Guid>("uniqueidentifier");
			int? maxLength17 = 80;
			return new
			{
				Id = id18,
				UserId = userId5,
				Category = table.Column<string>("nvarchar(80)", null, maxLength17),
				Email = table.Column<bool>("bit"),
				InApp = table.Column<bool>("bit"),
				Push = table.Column<bool>("bit"),
				Webhook = table.Column<bool>("bit")
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_NotificationPreferences", x => x.Id);
			table.ForeignKey("FK_NotificationPreferences_Users_UserId", x => x.UserId, "Users", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("Notifications", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id17 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> userId4 = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> companyId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true);
			int? maxLength16 = 40;
			OperationBuilder<AddColumnOperation> channel = table.Column<string>("nvarchar(40)", null, maxLength16);
			maxLength16 = 20;
			OperationBuilder<AddColumnOperation> severity2 = table.Column<string>("nvarchar(20)", null, maxLength16);
			maxLength16 = 200;
			OperationBuilder<AddColumnOperation> title3 = table.Column<string>("nvarchar(200)", null, maxLength16);
			maxLength16 = 2000;
			OperationBuilder<AddColumnOperation> body2 = table.Column<string>("nvarchar(2000)", null, maxLength16);
			maxLength16 = 500;
			OperationBuilder<AddColumnOperation> url = table.Column<string>("nvarchar(500)", null, maxLength16, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> createdAt3 = table.Column<DateTime>("datetime2");
			OperationBuilder<AddColumnOperation> readAt = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> dismissedAt = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true);
			maxLength16 = 500;
			return new
			{
				Id = id17,
				UserId = userId4,
				CompanyId = companyId,
				Channel = channel,
				Severity = severity2,
				Title = title3,
				Body = body2,
				Url = url,
				CreatedAt = createdAt3,
				ReadAt = readAt,
				DismissedAt = dismissedAt,
				Tags = table.Column<string>("nvarchar(500)", null, maxLength16, rowVersion: false, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_Notifications", x => x.Id);
			table.ForeignKey("FK_Notifications_Users_UserId", x => x.UserId, "Users", "Id");
		});
		migrationBuilder.CreateTable("RefreshTokens", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id16 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> userId3 = table.Column<Guid>("uniqueidentifier");
			int? maxLength15 = 128;
			OperationBuilder<AddColumnOperation> token = table.Column<string>("nvarchar(128)", null, maxLength15);
			OperationBuilder<AddColumnOperation> expiresAt2 = table.Column<DateTime>("datetime2");
			OperationBuilder<AddColumnOperation> createdAt2 = table.Column<DateTime>("datetime2");
			maxLength15 = 64;
			OperationBuilder<AddColumnOperation> createdByIp = table.Column<string>("nvarchar(64)", null, maxLength15, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> revokedAt = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true);
			maxLength15 = 64;
			OperationBuilder<AddColumnOperation> revokedByIp = table.Column<string>("nvarchar(64)", null, maxLength15, rowVersion: false, null, nullable: true);
			maxLength15 = 128;
			OperationBuilder<AddColumnOperation> replacedByToken = table.Column<string>("nvarchar(128)", null, maxLength15, rowVersion: false, null, nullable: true);
			maxLength15 = 200;
			return new
			{
				Id = id16,
				UserId = userId3,
				Token = token,
				ExpiresAt = expiresAt2,
				CreatedAt = createdAt2,
				CreatedByIp = createdByIp,
				RevokedAt = revokedAt,
				RevokedByIp = revokedByIp,
				ReplacedByToken = replacedByToken,
				ReasonRevoked = table.Column<string>("nvarchar(200)", null, maxLength15, rowVersion: false, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_RefreshTokens", x => x.Id);
			table.ForeignKey("FK_RefreshTokens_Users_UserId", x => x.UserId, "Users", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("UserClaims", (ColumnsBuilder table) => new
		{
			Id = table.Column<int>("int").Annotation("SqlServer:Identity", "1, 1"),
			UserId = table.Column<Guid>("uniqueidentifier"),
			ClaimType = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true),
			ClaimValue = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true)
		}, null, table =>
		{
			table.PrimaryKey("PK_UserClaims", x => x.Id);
			table.ForeignKey("FK_UserClaims_Users_UserId", x => x.UserId, "Users", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("UserLogins", (ColumnsBuilder table) => new
		{
			LoginProvider = table.Column<string>("nvarchar(450)"),
			ProviderKey = table.Column<string>("nvarchar(450)"),
			ProviderDisplayName = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true),
			UserId = table.Column<Guid>("uniqueidentifier")
		}, null, table =>
		{
			table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
			table.ForeignKey("FK_UserLogins_Users_UserId", x => x.UserId, "Users", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("UserRoles", (ColumnsBuilder table) => new
		{
			UserId = table.Column<Guid>("uniqueidentifier"),
			RoleId = table.Column<Guid>("uniqueidentifier")
		}, null, table =>
		{
			table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
			table.ForeignKey("FK_UserRoles_Roles_RoleId", x => x.RoleId, "Roles", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
			table.ForeignKey("FK_UserRoles_Users_UserId", x => x.UserId, "Users", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("UserSessions", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id15 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> userId2 = table.Column<Guid>("uniqueidentifier");
			int? maxLength14 = 128;
			OperationBuilder<AddColumnOperation> sessionToken = table.Column<string>("nvarchar(128)", null, maxLength14);
			OperationBuilder<AddColumnOperation> createdAt = table.Column<DateTime>("datetime2");
			OperationBuilder<AddColumnOperation> lastSeenAt = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> expiresAt = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true);
			maxLength14 = 64;
			OperationBuilder<AddColumnOperation> ipAddress = table.Column<string>("nvarchar(64)", null, maxLength14, rowVersion: false, null, nullable: true);
			maxLength14 = 500;
			OperationBuilder<AddColumnOperation> userAgent = table.Column<string>("nvarchar(500)", null, maxLength14, rowVersion: false, null, nullable: true);
			maxLength14 = 200;
			return new
			{
				Id = id15,
				UserId = userId2,
				SessionToken = sessionToken,
				CreatedAt = createdAt,
				LastSeenAt = lastSeenAt,
				ExpiresAt = expiresAt,
				IpAddress = ipAddress,
				UserAgent = userAgent,
				DeviceFingerprint = table.Column<string>("nvarchar(200)", null, maxLength14, rowVersion: false, null, nullable: true),
				IsActive = table.Column<bool>("bit")
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_UserSessions", x => x.Id);
			table.ForeignKey("FK_UserSessions_Users_UserId", x => x.UserId, "Users", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("UserTokens", (ColumnsBuilder table) => new
		{
			UserId = table.Column<Guid>("uniqueidentifier"),
			LoginProvider = table.Column<string>("nvarchar(450)"),
			Name = table.Column<string>("nvarchar(450)"),
			Value = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true)
		}, null, table =>
		{
			table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
			table.ForeignKey("FK_UserTokens_Users_UserId", x => x.UserId, "Users", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("FileAccessLogs", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id14 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> fileId2 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> userId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> accessedAt = table.Column<DateTime>("datetime2");
			int? maxLength13 = 40;
			OperationBuilder<AddColumnOperation> action = table.Column<string>("nvarchar(40)", null, maxLength13);
			maxLength13 = 64;
			return new
			{
				Id = id14,
				FileId = fileId2,
				UserId = userId,
				AccessedAt = accessedAt,
				Action = action,
				IpAddress = table.Column<string>("nvarchar(64)", null, maxLength13, rowVersion: false, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_FileAccessLogs", x => x.Id);
			table.ForeignKey("FK_FileAccessLogs_Files_FileId", x => x.FileId, "Files", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
			table.ForeignKey("FK_FileAccessLogs_Users_UserId", x => x.UserId, "Users", "Id");
		});
		migrationBuilder.CreateTable("FileVersions", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id13 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> fileId = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> version = table.Column<int>("int");
			int? maxLength12 = 500;
			OperationBuilder<AddColumnOperation> storagePath2 = table.Column<string>("nvarchar(500)", null, maxLength12);
			OperationBuilder<AddColumnOperation> sizeBytes2 = table.Column<long>("bigint");
			maxLength12 = 128;
			return new
			{
				Id = id13,
				FileId = fileId,
				Version = version,
				StoragePath = storagePath2,
				SizeBytes = sizeBytes2,
				Checksum = table.Column<string>("nvarchar(128)", null, maxLength12, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				CreatedDate = table.Column<DateTime>("datetime2")
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_FileVersions", x => x.Id);
			table.ForeignKey("FK_FileVersions_Files_FileId", x => x.FileId, "Files", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("Invoices", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id12 = table.Column<Guid>("uniqueidentifier");
			int? maxLength11 = 40;
			OperationBuilder<AddColumnOperation> number = table.Column<string>("nvarchar(40)", null, maxLength11);
			OperationBuilder<AddColumnOperation> status3 = table.Column<int>("int");
			maxLength11 = 18;
			int? scale2 = 2;
			OperationBuilder<AddColumnOperation> subtotal = table.Column<decimal>("decimal(18,2)", null, null, rowVersion: false, null, nullable: false, null, null, null, null, null, null, maxLength11, scale2);
			scale2 = 18;
			maxLength11 = 2;
			OperationBuilder<AddColumnOperation> tax = table.Column<decimal>("decimal(18,2)", null, null, rowVersion: false, null, nullable: false, null, null, null, null, null, null, scale2, maxLength11);
			maxLength11 = 18;
			scale2 = 2;
			OperationBuilder<AddColumnOperation> total = table.Column<decimal>("decimal(18,2)", null, null, rowVersion: false, null, nullable: false, null, null, null, null, null, null, maxLength11, scale2);
			scale2 = 10;
			return new
			{
				Id = id12,
				Number = number,
				Status = status3,
				Subtotal = subtotal,
				Tax = tax,
				Total = total,
				Currency = table.Column<string>("nvarchar(10)", null, scale2),
				IssuedAt = table.Column<DateTime>("datetime2"),
				DueAt = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				PaidAt = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				LineItemsJson = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true),
				SubscriptionId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true),
				CompanyId = table.Column<Guid>("uniqueidentifier")
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_Invoices", x => x.Id);
			table.ForeignKey("FK_Invoices_Subscriptions_SubscriptionId", x => x.SubscriptionId, "Subscriptions", "Id");
		});
		migrationBuilder.CreateTable("AuditFindings", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id11 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> auditRunId = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> scanType = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> severity = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> status2 = table.Column<int>("int");
			int? maxLength10 = 200;
			OperationBuilder<AddColumnOperation> title2 = table.Column<string>("nvarchar(200)", null, maxLength10);
			maxLength10 = 4000;
			OperationBuilder<AddColumnOperation> details = table.Column<string>("nvarchar(4000)", null, maxLength10, rowVersion: false, null, nullable: true);
			maxLength10 = 500;
			OperationBuilder<AddColumnOperation> affectedResource = table.Column<string>("nvarchar(500)", null, maxLength10, rowVersion: false, null, nullable: true);
			maxLength10 = 1000;
			return new
			{
				Id = id11,
				AuditRunId = auditRunId,
				ScanType = scanType,
				Severity = severity,
				Status = status2,
				Title = title2,
				Details = details,
				AffectedResource = affectedResource,
				RecommendedAction = table.Column<string>("nvarchar(1000)", null, maxLength10, rowVersion: false, null, nullable: true),
				RelatedTaskId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				GeneratedTaskId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_AuditFindings", x => x.Id);
			table.ForeignKey("FK_AuditFindings_AuditRuns_AuditRunId", x => x.AuditRunId, "AuditRuns", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
			table.ForeignKey("FK_AuditFindings_Tasks_GeneratedTaskId", x => x.GeneratedTaskId, "Tasks", "Id");
			table.ForeignKey("FK_AuditFindings_Tasks_RelatedTaskId", x => x.RelatedTaskId, "Tasks", "Id");
		});
		migrationBuilder.CreateTable("TaskArtifacts", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id10 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> taskId9 = table.Column<Guid>("uniqueidentifier");
			int? maxLength9 = 255;
			OperationBuilder<AddColumnOperation> fileName = table.Column<string>("nvarchar(255)", null, maxLength9);
			maxLength9 = 120;
			OperationBuilder<AddColumnOperation> contentType = table.Column<string>("nvarchar(120)", null, maxLength9);
			OperationBuilder<AddColumnOperation> sizeBytes = table.Column<long>("bigint");
			maxLength9 = 500;
			OperationBuilder<AddColumnOperation> storagePath = table.Column<string>("nvarchar(500)", null, maxLength9);
			maxLength9 = 40;
			OperationBuilder<AddColumnOperation> storageProvider = table.Column<string>("nvarchar(40)", null, maxLength9);
			maxLength9 = 128;
			OperationBuilder<AddColumnOperation> checksum = table.Column<string>("nvarchar(128)", null, maxLength9, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> isFinal = table.Column<bool>("bit");
			OperationBuilder<AddColumnOperation> thumbnailPath = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true);
			maxLength9 = 500;
			OperationBuilder<AddColumnOperation> previewUrl = table.Column<string>("nvarchar(500)", null, maxLength9, rowVersion: false, null, nullable: true);
			maxLength9 = 40;
			return new
			{
				Id = id10,
				TaskId = taskId9,
				FileName = fileName,
				ContentType = contentType,
				SizeBytes = sizeBytes,
				StoragePath = storagePath,
				StorageProvider = storageProvider,
				Checksum = checksum,
				IsFinal = isFinal,
				ThumbnailPath = thumbnailPath,
				PreviewUrl = previewUrl,
				Kind = table.Column<string>("nvarchar(40)", null, maxLength9, rowVersion: false, null, nullable: true),
				Version = table.Column<int>("int"),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_TaskArtifacts", x => x.Id);
			table.ForeignKey("FK_TaskArtifacts_Tasks_TaskId", x => x.TaskId, "Tasks", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("TaskAssignments", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id9 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> taskId8 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> assignedUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> assignedWorkerId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> assignedAt = table.Column<DateTime>("datetime2");
			int? maxLength8 = 20;
			OperationBuilder<AddColumnOperation> assignmentType = table.Column<string>("nvarchar(20)", null, maxLength8);
			maxLength8 = 1000;
			return new
			{
				Id = id9,
				TaskId = taskId8,
				AssignedUserId = assignedUserId,
				AssignedWorkerId = assignedWorkerId,
				AssignedAt = assignedAt,
				AssignmentType = assignmentType,
				Notes = table.Column<string>("nvarchar(1000)", null, maxLength8, rowVersion: false, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_TaskAssignments", x => x.Id);
			table.ForeignKey("FK_TaskAssignments_Tasks_TaskId", x => x.TaskId, "Tasks", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
			table.ForeignKey("FK_TaskAssignments_WorkerNodes_AssignedWorkerId", x => x.AssignedWorkerId, "WorkerNodes", "Id");
		});
		migrationBuilder.CreateTable("TaskComments", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id8 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> taskId7 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> authorUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true);
			int? maxLength7 = 8000;
			return new
			{
				Id = id8,
				TaskId = taskId7,
				AuthorUserId = authorUserId,
				Body = table.Column<string>("nvarchar(max)", null, maxLength7),
				IsSystem = table.Column<bool>("bit"),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_TaskComments", x => x.Id);
			table.ForeignKey("FK_TaskComments_Tasks_TaskId", x => x.TaskId, "Tasks", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("TaskDependencies", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id7 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> taskId6 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> dependsOnTaskId = table.Column<Guid>("uniqueidentifier");
			int? maxLength6 = 40;
			return new
			{
				Id = id7,
				TaskId = taskId6,
				DependsOnTaskId = dependsOnTaskId,
				DependencyType = table.Column<string>("nvarchar(40)", null, maxLength6),
				IsHardDependency = table.Column<bool>("bit")
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_TaskDependencies", x => x.Id);
			table.ForeignKey("FK_TaskDependencies_Tasks_DependsOnTaskId", x => x.DependsOnTaskId, "Tasks", "Id");
			table.ForeignKey("FK_TaskDependencies_Tasks_TaskId", x => x.TaskId, "Tasks", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("TaskExecutions", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id6 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> taskId5 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> workerNodeId = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> attemptNumber2 = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> startedAt = table.Column<DateTime>("datetime2");
			OperationBuilder<AddColumnOperation> completedAt = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> finalStatus = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> checkpointJson = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true);
			int? maxLength5 = 500;
			OperationBuilder<AddColumnOperation> workspacePath = table.Column<string>("nvarchar(500)", null, maxLength5, rowVersion: false, null, nullable: true);
			maxLength5 = 4000;
			OperationBuilder<AddColumnOperation> outputSummary = table.Column<string>("nvarchar(4000)", null, maxLength5, rowVersion: false, null, nullable: true);
			maxLength5 = 2000;
			return new
			{
				Id = id6,
				TaskId = taskId5,
				WorkerNodeId = workerNodeId,
				AttemptNumber = attemptNumber2,
				StartedAt = startedAt,
				CompletedAt = completedAt,
				FinalStatus = finalStatus,
				CheckpointJson = checkpointJson,
				WorkspacePath = workspacePath,
				OutputSummary = outputSummary,
				ErrorMessage = table.Column<string>("nvarchar(2000)", null, maxLength5, rowVersion: false, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_TaskExecutions", x => x.Id);
			table.ForeignKey("FK_TaskExecutions_Tasks_TaskId", x => x.TaskId, "Tasks", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
			table.ForeignKey("FK_TaskExecutions_WorkerNodes_WorkerNodeId", x => x.WorkerNodeId, "WorkerNodes", "Id");
		});
		migrationBuilder.CreateTable("TaskFailures", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id5 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> taskId4 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> failedAt = table.Column<DateTime>("datetime2");
			int? maxLength4 = 2000;
			OperationBuilder<AddColumnOperation> reason = table.Column<string>("nvarchar(2000)", null, maxLength4);
			OperationBuilder<AddColumnOperation> stackTrace = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true);
			maxLength4 = 40;
			return new
			{
				Id = id5,
				TaskId = taskId4,
				FailedAt = failedAt,
				Reason = reason,
				StackTrace = stackTrace,
				Category = table.Column<string>("nvarchar(40)", null, maxLength4, rowVersion: false, null, nullable: true),
				IsTransient = table.Column<bool>("bit"),
				WorkerNodeId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_TaskFailures", x => x.Id);
			table.ForeignKey("FK_TaskFailures_Tasks_TaskId", x => x.TaskId, "Tasks", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("TaskLogs", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id4 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> taskId3 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> timestamp = table.Column<DateTime>("datetime2");
			int? maxLength3 = 20;
			OperationBuilder<AddColumnOperation> level = table.Column<string>("nvarchar(20)", null, maxLength3);
			maxLength3 = 4000;
			OperationBuilder<AddColumnOperation> message = table.Column<string>("nvarchar(4000)", null, maxLength3);
			maxLength3 = 120;
			return new
			{
				Id = id4,
				TaskId = taskId3,
				Timestamp = timestamp,
				Level = level,
				Message = message,
				Source = table.Column<string>("nvarchar(120)", null, maxLength3, rowVersion: false, null, nullable: true),
				ContextJson = table.Column<string>("nvarchar(max)", null, null, rowVersion: false, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_TaskLogs", x => x.Id);
			table.ForeignKey("FK_TaskLogs_Tasks_TaskId", x => x.TaskId, "Tasks", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("TaskRecommendations", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id3 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> taskId2 = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true);
			int? maxLength2 = 200;
			OperationBuilder<AddColumnOperation> title = table.Column<string>("nvarchar(200)", null, maxLength2);
			maxLength2 = 4000;
			OperationBuilder<AddColumnOperation> body = table.Column<string>("nvarchar(4000)", null, maxLength2);
			maxLength2 = 40;
			OperationBuilder<AddColumnOperation> source = table.Column<string>("nvarchar(40)", null, maxLength2);
			maxLength2 = 40;
			return new
			{
				Id = id3,
				TaskId = taskId2,
				Title = title,
				Body = body,
				Source = source,
				Status = table.Column<string>("nvarchar(40)", null, maxLength2),
				Confidence = table.Column<double>("float"),
				AppliedAt = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true),
				CompanyId = table.Column<Guid>("uniqueidentifier")
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_TaskRecommendations", x => x.Id);
			table.ForeignKey("FK_TaskRecommendations_Tasks_TaskId", x => x.TaskId, "Tasks", "Id");
		});
		migrationBuilder.CreateTable("TaskRetries", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id2 = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> taskId = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> attemptNumber = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> attemptedAt = table.Column<DateTime>("datetime2");
			int? maxLength = 2000;
			OperationBuilder<AddColumnOperation> failureReason = table.Column<string>("nvarchar(2000)", null, maxLength, rowVersion: false, null, nullable: true);
			maxLength = 40;
			return new
			{
				Id = id2,
				TaskId = taskId,
				AttemptNumber = attemptNumber,
				AttemptedAt = attemptedAt,
				FailureReason = failureReason,
				Strategy = table.Column<string>("nvarchar(40)", null, maxLength, rowVersion: false, null, nullable: true),
				WorkerNodeId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_TaskRetries", x => x.Id);
			table.ForeignKey("FK_TaskRetries_Tasks_TaskId", x => x.TaskId, "Tasks", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("TaskTagLinks", (ColumnsBuilder table) => new
		{
			Id = table.Column<Guid>("uniqueidentifier"),
			TaskId = table.Column<Guid>("uniqueidentifier"),
			TagId = table.Column<Guid>("uniqueidentifier")
		}, null, table =>
		{
			table.PrimaryKey("PK_TaskTagLinks", x => x.Id);
			table.ForeignKey("FK_TaskTagLinks_TaskTags_TagId", x => x.TagId, "TaskTags", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
			table.ForeignKey("FK_TaskTagLinks_Tasks_TaskId", x => x.TaskId, "Tasks", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("Payments", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<Guid>("uniqueidentifier");
			OperationBuilder<AddColumnOperation> invoiceId = table.Column<Guid>("uniqueidentifier");
			int? precision = 18;
			int? scale = 2;
			OperationBuilder<AddColumnOperation> amount = table.Column<decimal>("decimal(18,2)", null, null, rowVersion: false, null, nullable: false, null, null, null, null, null, null, precision, scale);
			scale = 10;
			OperationBuilder<AddColumnOperation> currency = table.Column<string>("nvarchar(10)", null, scale);
			OperationBuilder<AddColumnOperation> status = table.Column<int>("int");
			scale = 40;
			OperationBuilder<AddColumnOperation> provider = table.Column<string>("nvarchar(40)", null, scale);
			scale = 200;
			return new
			{
				Id = id,
				InvoiceId = invoiceId,
				Amount = amount,
				Currency = currency,
				Status = status,
				Provider = provider,
				ProviderReference = table.Column<string>("nvarchar(200)", null, scale, rowVersion: false, null, nullable: true),
				ProcessedAt = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedDate = table.Column<DateTime>("datetime2"),
				ModifiedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				DeletedDate = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				CreatedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				ModifiedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				DeletedByUserId = table.Column<Guid>("uniqueidentifier", null, null, rowVersion: false, null, nullable: true),
				IsDeleted = table.Column<bool>("bit"),
				RowVersion = table.Column<byte[]>("rowversion", null, null, rowVersion: true, null, nullable: true),
				CompanyId = table.Column<Guid>("uniqueidentifier")
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_Payments", x => x.Id);
			table.ForeignKey("FK_Payments_Invoices_InvoiceId", x => x.InvoiceId, "Invoices", "Id");
		});
		migrationBuilder.CreateIndex("IX_ApiKeys_KeyPrefix", "ApiKeys", "KeyPrefix");
		migrationBuilder.CreateIndex("IX_ApiKeys_UserId", "ApiKeys", "UserId");
		migrationBuilder.CreateIndex("IX_AuditFindings_AuditRunId", "AuditFindings", "AuditRunId");
		migrationBuilder.CreateIndex("IX_AuditFindings_GeneratedTaskId", "AuditFindings", "GeneratedTaskId");
		migrationBuilder.CreateIndex("IX_AuditFindings_RelatedTaskId", "AuditFindings", "RelatedTaskId");
		migrationBuilder.CreateIndex("IX_AuditFindings_Status_Severity", "AuditFindings", new string[2] { "Status", "Severity" });
		migrationBuilder.CreateIndex("IX_AuditLogs_EntityType_EntityId", "AuditLogs", new string[2] { "EntityType", "EntityId" });
		migrationBuilder.CreateIndex("IX_AuditRecommendations_AuditRunId", "AuditRecommendations", "AuditRunId");
		migrationBuilder.CreateIndex("IX_Companies_Slug", "Companies", "Slug", null, unique: true);
		migrationBuilder.CreateIndex("IX_CompanyBranding_CompanyId", "CompanyBranding", "CompanyId", null, unique: true);
		migrationBuilder.CreateIndex("IX_CompanyPlans_Code", "CompanyPlans", "Code", null, unique: true);
		migrationBuilder.CreateIndex("IX_CompanySettings_CompanyId", "CompanySettings", "CompanyId", null, unique: true);
		migrationBuilder.CreateIndex("IX_FeatureFlags_Key", "FeatureFlags", "Key", null, unique: true);
		migrationBuilder.CreateIndex("IX_FileAccessLogs_FileId", "FileAccessLogs", "FileId");
		migrationBuilder.CreateIndex("IX_FileAccessLogs_UserId", "FileAccessLogs", "UserId");
		migrationBuilder.CreateIndex("IX_FileFolders_ParentFolderId", "FileFolders", "ParentFolderId");
		migrationBuilder.CreateIndex("IX_Files_FolderId", "Files", "FolderId");
		migrationBuilder.CreateIndex("IX_FileVersions_FileId", "FileVersions", "FileId");
		migrationBuilder.CreateIndex("IX_Invoices_Number", "Invoices", "Number", null, unique: true);
		migrationBuilder.CreateIndex("IX_Invoices_SubscriptionId", "Invoices", "SubscriptionId");
		migrationBuilder.CreateIndex("IX_LoginHistory_UserId_LoginAt", "LoginHistory", new string[2] { "UserId", "LoginAt" });
		migrationBuilder.CreateIndex("IX_NotificationPreferences_UserId_Category", "NotificationPreferences", new string[2] { "UserId", "Category" }, null, unique: true);
		migrationBuilder.CreateIndex("IX_Notifications_UserId_CreatedAt", "Notifications", new string[2] { "UserId", "CreatedAt" });
		migrationBuilder.CreateIndex("IX_NotificationTemplates_Code", "NotificationTemplates", "Code", null, unique: true);
		migrationBuilder.CreateIndex("IX_Payments_InvoiceId", "Payments", "InvoiceId");
		migrationBuilder.CreateIndex("IX_Permissions_Code", "Permissions", "Code", null, unique: true);
		migrationBuilder.CreateIndex("IX_Plans_Code", "Plans", "Code", null, unique: true);
		migrationBuilder.CreateIndex("IX_RefreshTokens_Token", "RefreshTokens", "Token", null, unique: true);
		migrationBuilder.CreateIndex("IX_RefreshTokens_UserId", "RefreshTokens", "UserId");
		migrationBuilder.CreateIndex("IX_RoleClaims_RoleId", "RoleClaims", "RoleId");
		migrationBuilder.CreateIndex("IX_RolePermissions_PermissionId", "RolePermissions", "PermissionId");
		migrationBuilder.CreateIndex("IX_RolePermissions_RoleId_PermissionId", "RolePermissions", new string[2] { "RoleId", "PermissionId" }, null, unique: true);
		migrationBuilder.CreateIndex("RoleNameIndex", "Roles", "NormalizedName", null, unique: true, "[NormalizedName] IS NOT NULL");
		migrationBuilder.CreateIndex("IX_Subscriptions_PlanId", "Subscriptions", "PlanId");
		migrationBuilder.CreateIndex("IX_SystemSettings_Key", "SystemSettings", "Key", null, unique: true);
		migrationBuilder.CreateIndex("IX_TaskArtifacts_TaskId", "TaskArtifacts", "TaskId");
		migrationBuilder.CreateIndex("IX_TaskAssignments_AssignedWorkerId", "TaskAssignments", "AssignedWorkerId");
		migrationBuilder.CreateIndex("IX_TaskAssignments_TaskId", "TaskAssignments", "TaskId");
		migrationBuilder.CreateIndex("IX_TaskComments_TaskId", "TaskComments", "TaskId");
		migrationBuilder.CreateIndex("IX_TaskDependencies_DependsOnTaskId", "TaskDependencies", "DependsOnTaskId");
		migrationBuilder.CreateIndex("IX_TaskDependencies_TaskId_DependsOnTaskId", "TaskDependencies", new string[2] { "TaskId", "DependsOnTaskId" }, null, unique: true);
		migrationBuilder.CreateIndex("IX_TaskExecutions_TaskId_AttemptNumber", "TaskExecutions", new string[2] { "TaskId", "AttemptNumber" });
		migrationBuilder.CreateIndex("IX_TaskExecutions_WorkerNodeId", "TaskExecutions", "WorkerNodeId");
		migrationBuilder.CreateIndex("IX_TaskFailures_TaskId_FailedAt", "TaskFailures", new string[2] { "TaskId", "FailedAt" });
		migrationBuilder.CreateIndex("IX_TaskLogs_TaskId_Timestamp", "TaskLogs", new string[2] { "TaskId", "Timestamp" });
		migrationBuilder.CreateIndex("IX_TaskRecommendations_TaskId", "TaskRecommendations", "TaskId");
		migrationBuilder.CreateIndex("IX_TaskRetries_TaskId_AttemptNumber", "TaskRetries", new string[2] { "TaskId", "AttemptNumber" });
		migrationBuilder.CreateIndex("IX_Tasks_ClaimedByWorkerId", "Tasks", "ClaimedByWorkerId");
		migrationBuilder.CreateIndex("IX_Tasks_CompanyId_Status", "Tasks", new string[2] { "CompanyId", "Status" });
		migrationBuilder.CreateIndex("IX_Tasks_CreatedDate", "Tasks", "CreatedDate");
		migrationBuilder.CreateIndex("IX_Tasks_ParentTaskId", "Tasks", "ParentTaskId");
		migrationBuilder.CreateIndex("IX_Tasks_Priority", "Tasks", "Priority");
		migrationBuilder.CreateIndex("IX_Tasks_QueueName_Status", "Tasks", new string[2] { "QueueName", "Status" });
		migrationBuilder.CreateIndex("IX_Tasks_Status", "Tasks", "Status");
		migrationBuilder.CreateIndex("IX_Tasks_TemplateId", "Tasks", "TemplateId");
		migrationBuilder.CreateIndex("IX_TaskTagLinks_TagId", "TaskTagLinks", "TagId");
		migrationBuilder.CreateIndex("IX_TaskTagLinks_TaskId_TagId", "TaskTagLinks", new string[2] { "TaskId", "TagId" }, null, unique: true);
		migrationBuilder.CreateIndex("IX_TaskTags_CompanyId_Name", "TaskTags", new string[2] { "CompanyId", "Name" }, null, unique: true);
		migrationBuilder.CreateIndex("IX_TaskTemplates_CompanyId_Name", "TaskTemplates", new string[2] { "CompanyId", "Name" });
		migrationBuilder.CreateIndex("IX_UserClaims_UserId", "UserClaims", "UserId");
		migrationBuilder.CreateIndex("IX_UserLogins_UserId", "UserLogins", "UserId");
		migrationBuilder.CreateIndex("IX_UserRoles_RoleId", "UserRoles", "RoleId");
		migrationBuilder.CreateIndex("EmailIndex", "Users", "NormalizedEmail");
		migrationBuilder.CreateIndex("IX_Users_CompanyId", "Users", "CompanyId");
		migrationBuilder.CreateIndex("UserNameIndex", "Users", "NormalizedUserName", null, unique: true, "[NormalizedUserName] IS NOT NULL");
		migrationBuilder.CreateIndex("IX_UserSessions_SessionToken", "UserSessions", "SessionToken", null, unique: true);
		migrationBuilder.CreateIndex("IX_UserSessions_UserId", "UserSessions", "UserId");
		migrationBuilder.CreateIndex("IX_WorkerCapabilities_WorkerNodeId_Capability", "WorkerCapabilities", new string[2] { "WorkerNodeId", "Capability" }, null, unique: true);
		migrationBuilder.CreateIndex("IX_WorkerHeartbeats_WorkerNodeId_Timestamp", "WorkerHeartbeats", new string[2] { "WorkerNodeId", "Timestamp" });
		migrationBuilder.CreateIndex("IX_WorkerMetrics_WorkerNodeId_MetricName_Timestamp", "WorkerMetrics", new string[3] { "WorkerNodeId", "MetricName", "Timestamp" });
		migrationBuilder.CreateIndex("IX_WorkerNodes_MachineName_Name_CompanyId", "WorkerNodes", new string[3] { "MachineName", "Name", "CompanyId" });
		migrationBuilder.CreateIndex("IX_WorkerNodes_Status", "WorkerNodes", "Status");
		migrationBuilder.CreateIndex("IX_WorkerQueues_Name_CompanyId", "WorkerQueues", new string[2] { "Name", "CompanyId" }, null, unique: true, "[CompanyId] IS NOT NULL");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable("ApiKeys");
		migrationBuilder.DropTable("AuditFindings");
		migrationBuilder.DropTable("AuditLogs");
		migrationBuilder.DropTable("AuditRecommendations");
		migrationBuilder.DropTable("CleanupTasks");
		migrationBuilder.DropTable("CompanyBranding");
		migrationBuilder.DropTable("CompanyPlans");
		migrationBuilder.DropTable("CompanySettings");
		migrationBuilder.DropTable("CreditLedgers");
		migrationBuilder.DropTable("FeatureFlags");
		migrationBuilder.DropTable("FileAccessLogs");
		migrationBuilder.DropTable("FileVersions");
		migrationBuilder.DropTable("LoginHistory");
		migrationBuilder.DropTable("NotificationHistory");
		migrationBuilder.DropTable("NotificationPreferences");
		migrationBuilder.DropTable("Notifications");
		migrationBuilder.DropTable("NotificationTemplates");
		migrationBuilder.DropTable("OptimizationSuggestions");
		migrationBuilder.DropTable("Payments");
		migrationBuilder.DropTable("RefreshTokens");
		migrationBuilder.DropTable("RoleClaims");
		migrationBuilder.DropTable("RolePermissions");
		migrationBuilder.DropTable("SystemSettings");
		migrationBuilder.DropTable("TaskArtifacts");
		migrationBuilder.DropTable("TaskAssignments");
		migrationBuilder.DropTable("TaskComments");
		migrationBuilder.DropTable("TaskDependencies");
		migrationBuilder.DropTable("TaskExecutions");
		migrationBuilder.DropTable("TaskFailures");
		migrationBuilder.DropTable("TaskLogs");
		migrationBuilder.DropTable("TaskRecommendations");
		migrationBuilder.DropTable("TaskRetries");
		migrationBuilder.DropTable("TaskTagLinks");
		migrationBuilder.DropTable("UsageRecords");
		migrationBuilder.DropTable("UserClaims");
		migrationBuilder.DropTable("UserLogins");
		migrationBuilder.DropTable("UserRoles");
		migrationBuilder.DropTable("UserSessions");
		migrationBuilder.DropTable("UserTokens");
		migrationBuilder.DropTable("WebhookEndpoints");
		migrationBuilder.DropTable("WorkerCapabilities");
		migrationBuilder.DropTable("WorkerHeartbeats");
		migrationBuilder.DropTable("WorkerMetrics");
		migrationBuilder.DropTable("WorkerQueues");
		migrationBuilder.DropTable("AuditRuns");
		migrationBuilder.DropTable("Files");
		migrationBuilder.DropTable("Invoices");
		migrationBuilder.DropTable("Permissions");
		migrationBuilder.DropTable("TaskTags");
		migrationBuilder.DropTable("Tasks");
		migrationBuilder.DropTable("Roles");
		migrationBuilder.DropTable("Users");
		migrationBuilder.DropTable("FileFolders");
		migrationBuilder.DropTable("Subscriptions");
		migrationBuilder.DropTable("TaskTemplates");
		migrationBuilder.DropTable("WorkerNodes");
		migrationBuilder.DropTable("Companies");
		migrationBuilder.DropTable("Plans");
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		modelBuilder.HasAnnotation("ProductVersion", "8.0.10").HasAnnotation("Relational:MaxIdentifierLength", 128);
		modelBuilder.UseIdentityColumns(1L);
		modelBuilder.Entity("MADai.Domain.Audit.AuditFinding", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("AffectedResource").HasMaxLength(500).HasColumnType("nvarchar(500)");
			b.Property<Guid>("AuditRunId").HasColumnType("uniqueidentifier");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<string>("Details").HasMaxLength(4000).HasColumnType("nvarchar(4000)");
			b.Property<Guid?>("GeneratedTaskId").HasColumnType("uniqueidentifier");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("RecommendedAction").HasMaxLength(1000).HasColumnType("nvarchar(1000)");
			b.Property<Guid?>("RelatedTaskId").HasColumnType("uniqueidentifier");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<int>("ScanType").HasColumnType("int");
			b.Property<int>("Severity").HasColumnType("int");
			b.Property<int>("Status").HasColumnType("int");
			b.Property<string>("Title").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.HasKey("Id");
			b.HasIndex("AuditRunId");
			b.HasIndex("GeneratedTaskId");
			b.HasIndex("RelatedTaskId");
			b.HasIndex("Status", "Severity");
			b.ToTable("AuditFindings", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Audit.AuditRecommendation", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<Guid?>("AuditRunId").HasColumnType("uniqueidentifier");
			b.Property<string>("Body").IsRequired().HasMaxLength(4000)
				.HasColumnType("nvarchar(4000)");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<Guid?>("GeneratedTaskId").HasColumnType("uniqueidentifier");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<int>("Severity").HasColumnType("int");
			b.Property<string>("Status").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("Title").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.HasKey("Id");
			b.HasIndex("AuditRunId");
			b.ToTable("AuditRecommendations", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Audit.AuditRun", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("CompletedAt").HasColumnType("datetime2");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<int>("FindingsCount").HasColumnType("int");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<int>("RecommendationsCount").HasColumnType("int");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<int>("ScanType").HasColumnType("int");
			b.Property<DateTime>("StartedAt").HasColumnType("datetime2");
			b.Property<string>("Status").IsRequired().HasMaxLength(40)
				.HasColumnType("nvarchar(40)");
			b.Property<string>("Summary").HasMaxLength(2000).HasColumnType("nvarchar(2000)");
			b.HasKey("Id");
			b.ToTable("AuditRuns", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Audit.CleanupTask", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<DateTime?>("ExecutedAt").HasColumnType("datetime2");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<int>("ItemCount").HasColumnType("int");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<long>("ReclaimableBytes").HasColumnType("bigint");
			b.Property<string>("Result").HasMaxLength(2000).HasColumnType("nvarchar(2000)");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<string>("Status").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("Target").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.HasKey("Id");
			b.ToTable("CleanupTasks", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Audit.OptimizationSuggestion", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Area").IsRequired().HasMaxLength(80)
				.HasColumnType("nvarchar(80)");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<string>("Description").IsRequired().HasMaxLength(4000)
				.HasColumnType("nvarchar(4000)");
			b.Property<double>("EstimatedImpact").HasColumnType("float");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<string>("Status").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("Title").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.HasKey("Id");
			b.ToTable("OptimizationSuggestions", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Billing.CreditLedger", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<decimal>("Amount").HasPrecision(18, 2).HasColumnType("decimal(18,2)");
			b.Property<Guid>("CompanyId").HasColumnType("uniqueidentifier");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<string>("Currency").IsRequired().HasMaxLength(10)
				.HasColumnType("nvarchar(10)");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<DateTime>("OccurredAt").HasColumnType("datetime2");
			b.Property<string>("Reason").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.Property<string>("Reference").HasMaxLength(200).HasColumnType("nvarchar(200)");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.HasKey("Id");
			b.ToTable("CreditLedgers", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Billing.Invoice", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<Guid>("CompanyId").HasColumnType("uniqueidentifier");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<string>("Currency").IsRequired().HasMaxLength(10)
				.HasColumnType("nvarchar(10)");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<DateTime?>("DueAt").HasColumnType("datetime2");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<DateTime>("IssuedAt").HasColumnType("datetime2");
			b.Property<string>("LineItemsJson").HasColumnType("nvarchar(max)");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("Number").IsRequired().HasMaxLength(40)
				.HasColumnType("nvarchar(40)");
			b.Property<DateTime?>("PaidAt").HasColumnType("datetime2");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<int>("Status").HasColumnType("int");
			b.Property<Guid?>("SubscriptionId").HasColumnType("uniqueidentifier");
			b.Property<decimal>("Subtotal").HasPrecision(18, 2).HasColumnType("decimal(18,2)");
			b.Property<decimal>("Tax").HasPrecision(18, 2).HasColumnType("decimal(18,2)");
			b.Property<decimal>("Total").HasPrecision(18, 2).HasColumnType("decimal(18,2)");
			b.HasKey("Id");
			b.HasIndex("Number").IsUnique();
			b.HasIndex("SubscriptionId");
			b.ToTable("Invoices", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Billing.Payment", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<decimal>("Amount").HasPrecision(18, 2).HasColumnType("decimal(18,2)");
			b.Property<Guid>("CompanyId").HasColumnType("uniqueidentifier");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<string>("Currency").IsRequired().HasMaxLength(10)
				.HasColumnType("nvarchar(10)");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<Guid>("InvoiceId").HasColumnType("uniqueidentifier");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<DateTime?>("ProcessedAt").HasColumnType("datetime2");
			b.Property<string>("Provider").IsRequired().HasMaxLength(40)
				.HasColumnType("nvarchar(40)");
			b.Property<string>("ProviderReference").HasMaxLength(200).HasColumnType("nvarchar(200)");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<int>("Status").HasColumnType("int");
			b.HasKey("Id");
			b.HasIndex("InvoiceId");
			b.ToTable("Payments", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Billing.Plan", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<decimal>("AnnualPrice").HasPrecision(18, 2).HasColumnType("decimal(18,2)");
			b.Property<string>("Code").IsRequired().HasMaxLength(40)
				.HasColumnType("nvarchar(40)");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<string>("Currency").IsRequired().HasMaxLength(10)
				.HasColumnType("nvarchar(10)");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<string>("Description").HasColumnType("nvarchar(max)");
			b.Property<string>("FeaturesJson").HasColumnType("nvarchar(max)");
			b.Property<int>("IncludedStorageGb").HasColumnType("int");
			b.Property<int>("IncludedTasks").HasColumnType("int");
			b.Property<int>("IncludedWorkers").HasColumnType("int");
			b.Property<bool>("IsActive").HasColumnType("bit");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<bool>("IsPublic").HasColumnType("bit");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<decimal>("MonthlyPrice").HasPrecision(18, 2).HasColumnType("decimal(18,2)");
			b.Property<string>("Name").IsRequired().HasMaxLength(120)
				.HasColumnType("nvarchar(120)");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.HasKey("Id");
			b.HasIndex("Code").IsUnique();
			b.ToTable("Plans", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Billing.Subscription", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("BillingCycle").IsRequired().HasMaxLength(20)
				.HasColumnType("nvarchar(20)");
			b.Property<DateTime?>("CancelledAt").HasColumnType("datetime2");
			b.Property<Guid>("CompanyId").HasColumnType("uniqueidentifier");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<DateTime?>("CurrentPeriodEnd").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<string>("ExternalReference").HasMaxLength(200).HasColumnType("nvarchar(200)");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<Guid>("PlanId").HasColumnType("uniqueidentifier");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<DateTime>("StartDate").HasColumnType("datetime2");
			b.Property<int>("Status").HasColumnType("int");
			b.HasKey("Id");
			b.HasIndex("PlanId");
			b.ToTable("Subscriptions", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Billing.UsageRecord", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<Guid>("CompanyId").HasColumnType("uniqueidentifier");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<string>("MetricName").IsRequired().HasMaxLength(80)
				.HasColumnType("nvarchar(80)");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("Notes").HasMaxLength(500).HasColumnType("nvarchar(500)");
			b.Property<DateTime>("PeriodEnd").HasColumnType("datetime2");
			b.Property<DateTime>("PeriodStart").HasColumnType("datetime2");
			b.Property<double>("Quantity").HasColumnType("float");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.HasKey("Id");
			b.ToTable("UsageRecords", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Files.FileAccessLog", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<DateTime>("AccessedAt").HasColumnType("datetime2");
			b.Property<string>("Action").IsRequired().HasMaxLength(40)
				.HasColumnType("nvarchar(40)");
			b.Property<Guid>("FileId").HasColumnType("uniqueidentifier");
			b.Property<string>("IpAddress").HasMaxLength(64).HasColumnType("nvarchar(64)");
			b.Property<Guid?>("UserId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("FileId");
			b.HasIndex("UserId");
			b.ToTable("FileAccessLogs", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Files.FileFolder", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<Guid>("CompanyId").HasColumnType("uniqueidentifier");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("Name").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.Property<Guid?>("ParentFolderId").HasColumnType("uniqueidentifier");
			b.Property<string>("Path").HasMaxLength(1000).HasColumnType("nvarchar(1000)");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.HasKey("Id");
			b.HasIndex("ParentFolderId");
			b.ToTable("FileFolders", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Files.FileItem", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Checksum").HasMaxLength(128).HasColumnType("nvarchar(128)");
			b.Property<Guid>("CompanyId").HasColumnType("uniqueidentifier");
			b.Property<string>("ContentType").IsRequired().HasMaxLength(120)
				.HasColumnType("nvarchar(120)");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<Guid?>("FolderId").HasColumnType("uniqueidentifier");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("Name").IsRequired().HasMaxLength(255)
				.HasColumnType("nvarchar(255)");
			b.Property<string>("PreviewPath").HasMaxLength(500).HasColumnType("nvarchar(500)");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<long>("SizeBytes").HasColumnType("bigint");
			b.Property<string>("StoragePath").IsRequired().HasMaxLength(500)
				.HasColumnType("nvarchar(500)");
			b.Property<string>("StorageProvider").IsRequired().HasMaxLength(40)
				.HasColumnType("nvarchar(40)");
			b.Property<string>("ThumbnailPath").HasMaxLength(500).HasColumnType("nvarchar(500)");
			b.Property<int>("Version").HasColumnType("int");
			b.HasKey("Id");
			b.HasIndex("FolderId");
			b.ToTable("Files", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Files.FileVersion", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Checksum").HasMaxLength(128).HasColumnType("nvarchar(128)");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid>("FileId").HasColumnType("uniqueidentifier");
			b.Property<long>("SizeBytes").HasColumnType("bigint");
			b.Property<string>("StoragePath").IsRequired().HasMaxLength(500)
				.HasColumnType("nvarchar(500)");
			b.Property<int>("Version").HasColumnType("int");
			b.HasKey("Id");
			b.HasIndex("FileId");
			b.ToTable("FileVersions", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Identity.ApiKey", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<Guid?>("CompanyId").HasColumnType("uniqueidentifier");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<DateTime?>("ExpiresAt").HasColumnType("datetime2");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<bool>("IsRevoked").HasColumnType("bit");
			b.Property<string>("KeyHash").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.Property<string>("KeyPrefix").IsRequired().HasMaxLength(20)
				.HasColumnType("nvarchar(20)");
			b.Property<DateTime?>("LastUsedAt").HasColumnType("datetime2");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("Name").IsRequired().HasMaxLength(120)
				.HasColumnType("nvarchar(120)");
			b.Property<DateTime?>("RevokedAt").HasColumnType("datetime2");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<string>("Scopes").HasMaxLength(1000).HasColumnType("nvarchar(1000)");
			b.Property<Guid>("UserId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("KeyPrefix");
			b.HasIndex("UserId");
			b.ToTable("ApiKeys", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Identity.ApplicationRole", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("ConcurrencyStamp").IsConcurrencyToken().HasColumnType("nvarchar(max)");
			b.Property<string>("Description").HasMaxLength(400).HasColumnType("nvarchar(400)");
			b.Property<bool>("IsSystem").HasColumnType("bit");
			b.Property<string>("Name").HasMaxLength(256).HasColumnType("nvarchar(256)");
			b.Property<string>("NormalizedName").HasMaxLength(256).HasColumnType("nvarchar(256)");
			b.HasKey("Id");
			b.HasIndex("NormalizedName").IsUnique().HasDatabaseName("RoleNameIndex")
				.HasFilter("[NormalizedName] IS NOT NULL");
			b.ToTable("Roles", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Identity.ApplicationUser", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<int>("AccessFailedCount").HasColumnType("int");
			b.Property<string>("AvatarUrl").HasMaxLength(500).HasColumnType("nvarchar(500)");
			b.Property<Guid?>("CompanyId").HasColumnType("uniqueidentifier");
			b.Property<string>("ConcurrencyStamp").IsConcurrencyToken().HasColumnType("nvarchar(max)");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<string>("Email").HasMaxLength(256).HasColumnType("nvarchar(256)");
			b.Property<bool>("EmailConfirmed").HasColumnType("bit");
			b.Property<string>("FirstName").HasMaxLength(80).HasColumnType("nvarchar(80)");
			b.Property<bool>("IsActive").HasColumnType("bit");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<bool>("IsMfaEnrolled").HasColumnType("bit");
			b.Property<DateTime?>("LastLoginAt").HasColumnType("datetime2");
			b.Property<string>("LastLoginIp").HasMaxLength(64).HasColumnType("nvarchar(64)");
			b.Property<string>("LastName").HasMaxLength(80).HasColumnType("nvarchar(80)");
			b.Property<string>("Locale").HasMaxLength(20).HasColumnType("nvarchar(20)");
			b.Property<bool>("LockoutEnabled").HasColumnType("bit");
			b.Property<DateTimeOffset?>("LockoutEnd").HasColumnType("datetimeoffset");
			b.Property<string>("NormalizedEmail").HasMaxLength(256).HasColumnType("nvarchar(256)");
			b.Property<string>("NormalizedUserName").HasMaxLength(256).HasColumnType("nvarchar(256)");
			b.Property<string>("PasswordHash").HasColumnType("nvarchar(max)");
			b.Property<string>("PhoneNumber").HasColumnType("nvarchar(max)");
			b.Property<bool>("PhoneNumberConfirmed").HasColumnType("bit");
			b.Property<string>("SecurityStamp").HasColumnType("nvarchar(max)");
			b.Property<string>("TimeZone").HasMaxLength(80).HasColumnType("nvarchar(80)");
			b.Property<bool>("TwoFactorEnabled").HasColumnType("bit");
			b.Property<string>("UserName").HasMaxLength(256).HasColumnType("nvarchar(256)");
			b.HasKey("Id");
			b.HasIndex("CompanyId");
			b.HasIndex("NormalizedEmail").HasDatabaseName("EmailIndex");
			b.HasIndex("NormalizedUserName").IsUnique().HasDatabaseName("UserNameIndex")
				.HasFilter("[NormalizedUserName] IS NOT NULL");
			b.ToTable("Users", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Identity.LoginHistory", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("City").HasMaxLength(120).HasColumnType("nvarchar(120)");
			b.Property<string>("Country").HasMaxLength(80).HasColumnType("nvarchar(80)");
			b.Property<string>("DeviceFingerprint").HasMaxLength(200).HasColumnType("nvarchar(200)");
			b.Property<string>("FailureReason").HasMaxLength(200).HasColumnType("nvarchar(200)");
			b.Property<string>("IpAddress").HasMaxLength(64).HasColumnType("nvarchar(64)");
			b.Property<DateTime>("LoginAt").HasColumnType("datetime2");
			b.Property<bool>("Success").HasColumnType("bit");
			b.Property<string>("UserAgent").HasMaxLength(500).HasColumnType("nvarchar(500)");
			b.Property<Guid>("UserId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("UserId", "LoginAt");
			b.ToTable("LoginHistory", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Identity.Permission", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Category").IsRequired().HasMaxLength(80)
				.HasColumnType("nvarchar(80)");
			b.Property<string>("Code").IsRequired().HasMaxLength(120)
				.HasColumnType("nvarchar(120)");
			b.Property<string>("Description").HasMaxLength(500).HasColumnType("nvarchar(500)");
			b.Property<string>("DisplayName").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.HasKey("Id");
			b.HasIndex("Code").IsUnique();
			b.ToTable("Permissions", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Identity.RefreshToken", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedAt").HasColumnType("datetime2");
			b.Property<string>("CreatedByIp").HasMaxLength(64).HasColumnType("nvarchar(64)");
			b.Property<DateTime>("ExpiresAt").HasColumnType("datetime2");
			b.Property<string>("ReasonRevoked").HasMaxLength(200).HasColumnType("nvarchar(200)");
			b.Property<string>("ReplacedByToken").HasMaxLength(128).HasColumnType("nvarchar(128)");
			b.Property<DateTime?>("RevokedAt").HasColumnType("datetime2");
			b.Property<string>("RevokedByIp").HasMaxLength(64).HasColumnType("nvarchar(64)");
			b.Property<string>("Token").IsRequired().HasMaxLength(128)
				.HasColumnType("nvarchar(128)");
			b.Property<Guid>("UserId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("Token").IsUnique();
			b.HasIndex("UserId");
			b.ToTable("RefreshTokens", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Identity.RolePermission", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<Guid>("PermissionId").HasColumnType("uniqueidentifier");
			b.Property<Guid>("RoleId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("PermissionId");
			b.HasIndex("RoleId", "PermissionId").IsUnique();
			b.ToTable("RolePermissions", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Identity.UserSession", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedAt").HasColumnType("datetime2");
			b.Property<string>("DeviceFingerprint").HasMaxLength(200).HasColumnType("nvarchar(200)");
			b.Property<DateTime?>("ExpiresAt").HasColumnType("datetime2");
			b.Property<string>("IpAddress").HasMaxLength(64).HasColumnType("nvarchar(64)");
			b.Property<bool>("IsActive").HasColumnType("bit");
			b.Property<DateTime?>("LastSeenAt").HasColumnType("datetime2");
			b.Property<string>("SessionToken").IsRequired().HasMaxLength(128)
				.HasColumnType("nvarchar(128)");
			b.Property<string>("UserAgent").HasMaxLength(500).HasColumnType("nvarchar(500)");
			b.Property<Guid>("UserId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("SessionToken").IsUnique();
			b.HasIndex("UserId");
			b.ToTable("UserSessions", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Notifications.Notification", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Body").IsRequired().HasMaxLength(2000)
				.HasColumnType("nvarchar(2000)");
			b.Property<string>("Channel").IsRequired().HasMaxLength(40)
				.HasColumnType("nvarchar(40)");
			b.Property<Guid?>("CompanyId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedAt").HasColumnType("datetime2");
			b.Property<DateTime?>("DismissedAt").HasColumnType("datetime2");
			b.Property<DateTime?>("ReadAt").HasColumnType("datetime2");
			b.Property<string>("Severity").IsRequired().HasMaxLength(20)
				.HasColumnType("nvarchar(20)");
			b.Property<string>("Tags").HasMaxLength(500).HasColumnType("nvarchar(500)");
			b.Property<string>("Title").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.Property<string>("Url").HasMaxLength(500).HasColumnType("nvarchar(500)");
			b.Property<Guid?>("UserId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("UserId", "CreatedAt");
			b.ToTable("Notifications", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Notifications.NotificationHistory", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Channel").IsRequired().HasMaxLength(40)
				.HasColumnType("nvarchar(40)");
			b.Property<Guid?>("CompanyId").HasColumnType("uniqueidentifier");
			b.Property<string>("ErrorMessage").HasMaxLength(2000).HasColumnType("nvarchar(2000)");
			b.Property<string>("Recipient").IsRequired().HasMaxLength(254)
				.HasColumnType("nvarchar(254)");
			b.Property<DateTime>("SentAt").HasColumnType("datetime2");
			b.Property<string>("Status").IsRequired().HasMaxLength(40)
				.HasColumnType("nvarchar(40)");
			b.Property<string>("TemplateCode").IsRequired().HasMaxLength(80)
				.HasColumnType("nvarchar(80)");
			b.Property<Guid?>("UserId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.ToTable("NotificationHistory", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Notifications.NotificationPreference", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Category").IsRequired().HasMaxLength(80)
				.HasColumnType("nvarchar(80)");
			b.Property<bool>("Email").HasColumnType("bit");
			b.Property<bool>("InApp").HasColumnType("bit");
			b.Property<bool>("Push").HasColumnType("bit");
			b.Property<Guid>("UserId").HasColumnType("uniqueidentifier");
			b.Property<bool>("Webhook").HasColumnType("bit");
			b.HasKey("Id");
			b.HasIndex("UserId", "Category").IsUnique();
			b.ToTable("NotificationPreferences", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Notifications.NotificationTemplate", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Body").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("Channel").IsRequired().HasMaxLength(40)
				.HasColumnType("nvarchar(40)");
			b.Property<string>("Code").IsRequired().HasMaxLength(80)
				.HasColumnType("nvarchar(80)");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<bool>("IsActive").HasColumnType("bit");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("Name").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<string>("Subject").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.HasKey("Id");
			b.HasIndex("Code").IsUnique();
			b.ToTable("NotificationTemplates", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.SystemEntities.AuditLog", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Action").IsRequired().HasMaxLength(120)
				.HasColumnType("nvarchar(120)");
			b.Property<Guid?>("ActorUserId").HasColumnType("uniqueidentifier");
			b.Property<Guid?>("CompanyId").HasColumnType("uniqueidentifier");
			b.Property<string>("Detail").HasMaxLength(4000).HasColumnType("nvarchar(4000)");
			b.Property<string>("EntityId").HasMaxLength(60).HasColumnType("nvarchar(60)");
			b.Property<string>("EntityType").IsRequired().HasMaxLength(120)
				.HasColumnType("nvarchar(120)");
			b.Property<string>("IpAddress").HasMaxLength(64).HasColumnType("nvarchar(64)");
			b.Property<string>("Severity").IsRequired().HasMaxLength(20)
				.HasColumnType("nvarchar(20)");
			b.Property<DateTime>("Timestamp").HasColumnType("datetime2");
			b.HasKey("Id");
			b.HasIndex("EntityType", "EntityId");
			b.ToTable("AuditLogs", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.SystemEntities.FeatureFlag", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Audience").HasMaxLength(500).HasColumnType("nvarchar(500)");
			b.Property<string>("Configuration").HasColumnType("nvarchar(max)");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<string>("Description").HasMaxLength(1000).HasColumnType("nvarchar(1000)");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<bool>("IsEnabled").HasColumnType("bit");
			b.Property<string>("Key").IsRequired().HasMaxLength(120)
				.HasColumnType("nvarchar(120)");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("Name").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.HasKey("Id");
			b.HasIndex("Key").IsUnique();
			b.ToTable("FeatureFlags", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.SystemEntities.SystemSetting", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Category").IsRequired().HasMaxLength(80)
				.HasColumnType("nvarchar(80)");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<string>("DataType").HasMaxLength(40).HasColumnType("nvarchar(40)");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<string>("Description").HasMaxLength(1000).HasColumnType("nvarchar(1000)");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<bool>("IsSecret").HasColumnType("bit");
			b.Property<string>("Key").IsRequired().HasMaxLength(120)
				.HasColumnType("nvarchar(120)");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<string>("Value").HasColumnType("nvarchar(max)");
			b.HasKey("Id");
			b.HasIndex("Key").IsUnique();
			b.ToTable("SystemSettings", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.SystemEntities.WebhookEndpoint", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<Guid>("CompanyId").HasColumnType("uniqueidentifier");
			b.Property<int>("ConsecutiveFailures").HasColumnType("int");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<string>("EventsCsv").HasMaxLength(1000).HasColumnType("nvarchar(1000)");
			b.Property<bool>("IsActive").HasColumnType("bit");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<DateTime?>("LastFailureAt").HasColumnType("datetime2");
			b.Property<DateTime?>("LastSuccessAt").HasColumnType("datetime2");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<string>("Secret").IsRequired().HasMaxLength(128)
				.HasColumnType("nvarchar(128)");
			b.Property<string>("Url").IsRequired().HasMaxLength(500)
				.HasColumnType("nvarchar(500)");
			b.HasKey("Id");
			b.ToTable("WebhookEndpoints", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskArtifact", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Checksum").HasMaxLength(128).HasColumnType("nvarchar(128)");
			b.Property<string>("ContentType").IsRequired().HasMaxLength(120)
				.HasColumnType("nvarchar(120)");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<string>("FileName").IsRequired().HasMaxLength(255)
				.HasColumnType("nvarchar(255)");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<bool>("IsFinal").HasColumnType("bit");
			b.Property<string>("Kind").HasMaxLength(40).HasColumnType("nvarchar(40)");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("PreviewUrl").HasMaxLength(500).HasColumnType("nvarchar(500)");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<long>("SizeBytes").HasColumnType("bigint");
			b.Property<string>("StoragePath").IsRequired().HasMaxLength(500)
				.HasColumnType("nvarchar(500)");
			b.Property<string>("StorageProvider").IsRequired().HasMaxLength(40)
				.HasColumnType("nvarchar(40)");
			b.Property<Guid>("TaskId").HasColumnType("uniqueidentifier");
			b.Property<string>("ThumbnailPath").HasColumnType("nvarchar(max)");
			b.Property<int>("Version").HasColumnType("int");
			b.HasKey("Id");
			b.HasIndex("TaskId");
			b.ToTable("TaskArtifacts", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskAssignment", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<DateTime>("AssignedAt").HasColumnType("datetime2");
			b.Property<Guid?>("AssignedUserId").HasColumnType("uniqueidentifier");
			b.Property<Guid?>("AssignedWorkerId").HasColumnType("uniqueidentifier");
			b.Property<string>("AssignmentType").IsRequired().HasMaxLength(20)
				.HasColumnType("nvarchar(20)");
			b.Property<string>("Notes").HasMaxLength(1000).HasColumnType("nvarchar(1000)");
			b.Property<Guid>("TaskId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("AssignedWorkerId");
			b.HasIndex("TaskId");
			b.ToTable("TaskAssignments", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskComment", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<Guid?>("AuthorUserId").HasColumnType("uniqueidentifier");
			b.Property<string>("Body").IsRequired().HasMaxLength(8000)
				.HasColumnType("nvarchar(max)");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<bool>("IsSystem").HasColumnType("bit");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<Guid>("TaskId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("TaskId");
			b.ToTable("TaskComments", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskDependency", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("DependencyType").IsRequired().HasMaxLength(40)
				.HasColumnType("nvarchar(40)");
			b.Property<Guid>("DependsOnTaskId").HasColumnType("uniqueidentifier");
			b.Property<bool>("IsHardDependency").HasColumnType("bit");
			b.Property<Guid>("TaskId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("DependsOnTaskId");
			b.HasIndex("TaskId", "DependsOnTaskId").IsUnique();
			b.ToTable("TaskDependencies", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskExecution", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<int>("AttemptNumber").HasColumnType("int");
			b.Property<string>("CheckpointJson").HasColumnType("nvarchar(max)");
			b.Property<DateTime?>("CompletedAt").HasColumnType("datetime2");
			b.Property<string>("ErrorMessage").HasMaxLength(2000).HasColumnType("nvarchar(2000)");
			b.Property<int>("FinalStatus").HasColumnType("int");
			b.Property<string>("OutputSummary").HasMaxLength(4000).HasColumnType("nvarchar(4000)");
			b.Property<DateTime>("StartedAt").HasColumnType("datetime2");
			b.Property<Guid>("TaskId").HasColumnType("uniqueidentifier");
			b.Property<Guid>("WorkerNodeId").HasColumnType("uniqueidentifier");
			b.Property<string>("WorkspacePath").HasMaxLength(500).HasColumnType("nvarchar(500)");
			b.HasKey("Id");
			b.HasIndex("WorkerNodeId");
			b.HasIndex("TaskId", "AttemptNumber");
			b.ToTable("TaskExecutions", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskFailure", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Category").HasMaxLength(40).HasColumnType("nvarchar(40)");
			b.Property<DateTime>("FailedAt").HasColumnType("datetime2");
			b.Property<bool>("IsTransient").HasColumnType("bit");
			b.Property<string>("Reason").IsRequired().HasMaxLength(2000)
				.HasColumnType("nvarchar(2000)");
			b.Property<string>("StackTrace").HasColumnType("nvarchar(max)");
			b.Property<Guid>("TaskId").HasColumnType("uniqueidentifier");
			b.Property<Guid?>("WorkerNodeId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("TaskId", "FailedAt");
			b.ToTable("TaskFailures", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskItem", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("CancelledAt").HasColumnType("datetime2");
			b.Property<int>("Category").HasColumnType("int");
			b.Property<string>("ClaimToken").HasMaxLength(64).HasColumnType("nvarchar(64)");
			b.Property<DateTime?>("ClaimedAt").HasColumnType("datetime2");
			b.Property<Guid?>("ClaimedByWorkerId").HasColumnType("uniqueidentifier");
			b.Property<Guid>("CompanyId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("CompletedAt").HasColumnType("datetime2");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<string>("CronExpression").HasMaxLength(100).HasColumnType("nvarchar(100)");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<string>("Description").HasMaxLength(8000).HasColumnType("nvarchar(max)");
			b.Property<string>("ErrorMessage").HasMaxLength(2000).HasColumnType("nvarchar(2000)");
			b.Property<string>("ErrorStack").HasColumnType("nvarchar(max)");
			b.Property<TimeSpan?>("EstimatedDuration").HasColumnType("time");
			b.Property<string>("InputPayload").HasColumnType("nvarchar(max)");
			b.Property<bool>("IsDeadLetter").HasColumnType("bit");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<bool>("IsRecurring").HasColumnType("bit");
			b.Property<DateTime?>("LastRunAt").HasColumnType("datetime2");
			b.Property<int>("MaxRetries").HasColumnType("int");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<DateTime?>("NextRetryAt").HasColumnType("datetime2");
			b.Property<int>("Origin").HasColumnType("int");
			b.Property<string>("OutputSummary").HasMaxLength(4000).HasColumnType("nvarchar(4000)");
			b.Property<Guid?>("ParentTaskId").HasColumnType("uniqueidentifier");
			b.Property<int>("Priority").HasColumnType("int");
			b.Property<int>("Progress").HasColumnType("int");
			b.Property<string>("PromptPayload").HasColumnType("nvarchar(max)");
			b.Property<string>("QueueName").HasMaxLength(80).HasColumnType("nvarchar(80)");
			b.Property<string>("ResultPayload").HasColumnType("nvarchar(max)");
			b.Property<int>("RetryCount").HasColumnType("int");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<DateTime?>("ScheduledAt").HasColumnType("datetime2");
			b.Property<DateTime?>("StartedAt").HasColumnType("datetime2");
			b.Property<int>("Status").HasColumnType("int");
			b.Property<string>("Tags").HasMaxLength(2000).HasColumnType("nvarchar(2000)");
			b.Property<Guid?>("TemplateId").HasColumnType("uniqueidentifier");
			b.Property<int>("TimeoutSeconds").HasColumnType("int");
			b.Property<string>("Title").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.Property<string>("ValidationReport").HasColumnType("nvarchar(max)");
			b.Property<string>("WorkspacePath").HasMaxLength(500).HasColumnType("nvarchar(500)");
			b.HasKey("Id");
			b.HasIndex("ClaimedByWorkerId");
			b.HasIndex("CreatedDate");
			b.HasIndex("ParentTaskId");
			b.HasIndex("Priority");
			b.HasIndex("Status");
			b.HasIndex("TemplateId");
			b.HasIndex("CompanyId", "Status");
			b.HasIndex("QueueName", "Status");
			b.ToTable("Tasks", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskLog", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("ContextJson").HasColumnType("nvarchar(max)");
			b.Property<string>("Level").IsRequired().HasMaxLength(20)
				.HasColumnType("nvarchar(20)");
			b.Property<string>("Message").IsRequired().HasMaxLength(4000)
				.HasColumnType("nvarchar(4000)");
			b.Property<string>("Source").HasMaxLength(120).HasColumnType("nvarchar(120)");
			b.Property<Guid>("TaskId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("Timestamp").HasColumnType("datetime2");
			b.HasKey("Id");
			b.HasIndex("TaskId", "Timestamp");
			b.ToTable("TaskLogs", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskRecommendation", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("AppliedAt").HasColumnType("datetime2");
			b.Property<string>("Body").IsRequired().HasMaxLength(4000)
				.HasColumnType("nvarchar(4000)");
			b.Property<Guid>("CompanyId").HasColumnType("uniqueidentifier");
			b.Property<double>("Confidence").HasColumnType("float");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<string>("Source").IsRequired().HasMaxLength(40)
				.HasColumnType("nvarchar(40)");
			b.Property<string>("Status").IsRequired().HasMaxLength(40)
				.HasColumnType("nvarchar(40)");
			b.Property<Guid?>("TaskId").HasColumnType("uniqueidentifier");
			b.Property<string>("Title").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.HasKey("Id");
			b.HasIndex("TaskId");
			b.ToTable("TaskRecommendations", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskRetry", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<int>("AttemptNumber").HasColumnType("int");
			b.Property<DateTime>("AttemptedAt").HasColumnType("datetime2");
			b.Property<string>("FailureReason").HasMaxLength(2000).HasColumnType("nvarchar(2000)");
			b.Property<string>("Strategy").HasMaxLength(40).HasColumnType("nvarchar(40)");
			b.Property<Guid>("TaskId").HasColumnType("uniqueidentifier");
			b.Property<Guid?>("WorkerNodeId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("TaskId", "AttemptNumber");
			b.ToTable("TaskRetries", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskTag", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Color").IsRequired().HasMaxLength(20)
				.HasColumnType("nvarchar(20)");
			b.Property<Guid>("CompanyId").HasColumnType("uniqueidentifier");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("Name").IsRequired().HasMaxLength(80)
				.HasColumnType("nvarchar(80)");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.HasKey("Id");
			b.HasIndex("CompanyId", "Name").IsUnique();
			b.ToTable("TaskTags", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskTagLink", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<Guid>("TagId").HasColumnType("uniqueidentifier");
			b.Property<Guid>("TaskId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("TagId");
			b.HasIndex("TaskId", "TagId").IsUnique();
			b.ToTable("TaskTagLinks", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskTemplate", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<int>("Category").HasColumnType("int");
			b.Property<Guid>("CompanyId").HasColumnType("uniqueidentifier");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<string>("DefaultInputJson").HasColumnType("nvarchar(max)");
			b.Property<int>("DefaultMaxRetries").HasColumnType("int");
			b.Property<int>("DefaultPriority").HasColumnType("int");
			b.Property<int>("DefaultTimeoutSeconds").HasColumnType("int");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<string>("Description").HasMaxLength(2000).HasColumnType("nvarchar(2000)");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<bool>("IsPublic").HasColumnType("bit");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("Name").IsRequired().HasMaxLength(120)
				.HasColumnType("nvarchar(120)");
			b.Property<string>("PromptTemplate").HasColumnType("nvarchar(max)");
			b.Property<string>("QueueName").HasMaxLength(80).HasColumnType("nvarchar(80)");
			b.Property<string>("RequiredCapabilities").HasMaxLength(500).HasColumnType("nvarchar(500)");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.HasKey("Id");
			b.HasIndex("CompanyId", "Name");
			b.ToTable("TaskTemplates", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Tenancy.Company", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("ContactEmail").HasMaxLength(254).HasColumnType("nvarchar(254)");
			b.Property<string>("ContactPhone").HasMaxLength(40).HasColumnType("nvarchar(40)");
			b.Property<string>("Country").HasMaxLength(80).HasColumnType("nvarchar(80)");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<bool>("IsActive").HasColumnType("bit");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<string>("LegalName").HasMaxLength(200).HasColumnType("nvarchar(200)");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("Name").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.Property<Guid?>("PlanId").HasColumnType("uniqueidentifier");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<string>("Slug").IsRequired().HasMaxLength(80)
				.HasColumnType("nvarchar(80)");
			b.Property<string>("Timezone").HasMaxLength(80).HasColumnType("nvarchar(80)");
			b.Property<string>("Website").HasMaxLength(200).HasColumnType("nvarchar(200)");
			b.HasKey("Id");
			b.HasIndex("Slug").IsUnique();
			b.ToTable("Companies", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Tenancy.CompanyBranding", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("AccentColor").IsRequired().HasMaxLength(20)
				.HasColumnType("nvarchar(20)");
			b.Property<string>("BackgroundColor").IsRequired().HasMaxLength(20)
				.HasColumnType("nvarchar(20)");
			b.Property<Guid>("CompanyId").HasColumnType("uniqueidentifier");
			b.Property<string>("CustomCss").HasColumnType("nvarchar(max)");
			b.Property<string>("EmailFromAddress").HasMaxLength(254).HasColumnType("nvarchar(254)");
			b.Property<string>("EmailFromName").HasMaxLength(120).HasColumnType("nvarchar(120)");
			b.Property<string>("FaviconUrl").HasMaxLength(500).HasColumnType("nvarchar(500)");
			b.Property<string>("LogoUrl").HasMaxLength(500).HasColumnType("nvarchar(500)");
			b.Property<string>("PrimaryColor").IsRequired().HasMaxLength(20)
				.HasColumnType("nvarchar(20)");
			b.Property<string>("ThemeMode").IsRequired().HasMaxLength(20)
				.HasColumnType("nvarchar(20)");
			b.HasKey("Id");
			b.HasIndex("CompanyId").IsUnique();
			b.ToTable("CompanyBranding", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Tenancy.CompanyPlan", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<decimal>("AnnualPrice").HasPrecision(18, 2).HasColumnType("decimal(18,2)");
			b.Property<string>("Code").IsRequired().HasMaxLength(40)
				.HasColumnType("nvarchar(40)");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<string>("Features").HasColumnType("nvarchar(max)");
			b.Property<bool>("IncludesPrioritySupport").HasColumnType("bit");
			b.Property<bool>("IncludesSelfHealing").HasColumnType("bit");
			b.Property<bool>("IsActive").HasColumnType("bit");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<int>("MaxConcurrentTasks").HasColumnType("int");
			b.Property<int>("MaxStorageGb").HasColumnType("int");
			b.Property<int>("MaxUsers").HasColumnType("int");
			b.Property<int>("MaxWorkers").HasColumnType("int");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<decimal>("MonthlyPrice").HasPrecision(18, 2).HasColumnType("decimal(18,2)");
			b.Property<string>("Name").IsRequired().HasMaxLength(120)
				.HasColumnType("nvarchar(120)");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.HasKey("Id");
			b.HasIndex("Code").IsUnique();
			b.ToTable("CompanyPlans", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Tenancy.CompanySettings", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<Guid>("CompanyId").HasColumnType("uniqueidentifier");
			b.Property<string>("CustomDomain").HasMaxLength(200).HasColumnType("nvarchar(200)");
			b.Property<int>("DefaultTaskTimeoutMinutes").HasColumnType("int");
			b.Property<bool>("EnableAutoRetry").HasColumnType("bit");
			b.Property<bool>("EnableSelfHealing").HasColumnType("bit");
			b.Property<int>("MaxConcurrentTasks").HasColumnType("int");
			b.Property<int>("MaxStorageGb").HasColumnType("int");
			b.Property<int>("MaxWorkers").HasColumnType("int");
			b.Property<int>("TaskRetentionDays").HasColumnType("int");
			b.Property<string>("WebhookSecret").HasMaxLength(128).HasColumnType("nvarchar(128)");
			b.HasKey("Id");
			b.HasIndex("CompanyId").IsUnique();
			b.ToTable("CompanySettings", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Workers.WorkerCapabilityEntry", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<int>("Capability").HasColumnType("int");
			b.Property<string>("Detail").HasColumnType("nvarchar(max)");
			b.Property<Guid>("WorkerNodeId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("WorkerNodeId", "Capability").IsUnique();
			b.ToTable("WorkerCapabilities", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Workers.WorkerHeartbeat", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<int>("ActiveTasks").HasColumnType("int");
			b.Property<double>("CpuPercent").HasColumnType("float");
			b.Property<double>("DiskFreeGb").HasColumnType("float");
			b.Property<double>("MemoryMb").HasColumnType("float");
			b.Property<string>("Notes").HasMaxLength(500).HasColumnType("nvarchar(500)");
			b.Property<int>("Status").HasColumnType("int");
			b.Property<DateTime>("Timestamp").HasColumnType("datetime2");
			b.Property<Guid>("WorkerNodeId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("WorkerNodeId", "Timestamp");
			b.ToTable("WorkerHeartbeats", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Workers.WorkerMetric", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("MetricName").IsRequired().HasMaxLength(80)
				.HasColumnType("nvarchar(80)");
			b.Property<string>("Tags").HasMaxLength(500).HasColumnType("nvarchar(500)");
			b.Property<DateTime>("Timestamp").HasColumnType("datetime2");
			b.Property<string>("Unit").HasMaxLength(20).HasColumnType("nvarchar(20)");
			b.Property<double>("Value").HasColumnType("float");
			b.Property<Guid>("WorkerNodeId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("WorkerNodeId", "MetricName", "Timestamp");
			b.ToTable("WorkerMetrics", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Workers.WorkerNode", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("AgentVersion").HasMaxLength(40).HasColumnType("nvarchar(40)");
			b.Property<string>("ApiKeyHash").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.Property<string>("Capabilities").HasMaxLength(2000).HasColumnType("nvarchar(2000)");
			b.Property<Guid?>("CompanyId").HasColumnType("uniqueidentifier");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<int>("CurrentConcurrency").HasColumnType("int");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<string>("IpAddress").HasMaxLength(64).HasColumnType("nvarchar(64)");
			b.Property<bool>("IsActive").HasColumnType("bit");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<string>("Labels").HasMaxLength(2000).HasColumnType("nvarchar(2000)");
			b.Property<DateTime?>("LastHeartbeatAt").HasColumnType("datetime2");
			b.Property<string>("MachineName").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.Property<int>("MaxConcurrency").HasColumnType("int");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("Name").IsRequired().HasMaxLength(120)
				.HasColumnType("nvarchar(120)");
			b.Property<string>("OperatingSystem").HasMaxLength(200).HasColumnType("nvarchar(200)");
			b.Property<string>("QueueName").HasMaxLength(80).HasColumnType("nvarchar(80)");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<DateTime?>("StartedAt").HasColumnType("datetime2");
			b.Property<int>("Status").HasColumnType("int");
			b.Property<string>("WorkspaceRoot").HasMaxLength(500).HasColumnType("nvarchar(500)");
			b.HasKey("Id");
			b.HasIndex("Status");
			b.HasIndex("MachineName", "Name", "CompanyId");
			b.ToTable("WorkerNodes", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Workers.WorkerQueue", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<Guid?>("CompanyId").HasColumnType("uniqueidentifier");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<string>("Description").HasColumnType("nvarchar(max)");
			b.Property<bool>("IsActive").HasColumnType("bit");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<int>("MaxParallelism").HasColumnType("int");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("Name").IsRequired().HasMaxLength(80)
				.HasColumnType("nvarchar(80)");
			b.Property<int>("Priority").HasColumnType("int");
			b.Property<string>("RequiredCapabilities").HasColumnType("nvarchar(max)");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.HasKey("Id");
			b.HasIndex("Name", "CompanyId").IsUnique().HasFilter("[CompanyId] IS NOT NULL");
			b.ToTable("WorkerQueues", (string?)null);
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<string>("ClaimType").HasColumnType("nvarchar(max)");
			b.Property<string>("ClaimValue").HasColumnType("nvarchar(max)");
			b.Property<Guid>("RoleId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("RoleId");
			b.ToTable("RoleClaims", (string?)null);
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<string>("ClaimType").HasColumnType("nvarchar(max)");
			b.Property<string>("ClaimValue").HasColumnType("nvarchar(max)");
			b.Property<Guid>("UserId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("UserId");
			b.ToTable("UserClaims", (string?)null);
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", delegate(EntityTypeBuilder b)
		{
			b.Property<string>("LoginProvider").HasColumnType("nvarchar(450)");
			b.Property<string>("ProviderKey").HasColumnType("nvarchar(450)");
			b.Property<string>("ProviderDisplayName").HasColumnType("nvarchar(max)");
			b.Property<Guid>("UserId").HasColumnType("uniqueidentifier");
			b.HasKey("LoginProvider", "ProviderKey");
			b.HasIndex("UserId");
			b.ToTable("UserLogins", (string?)null);
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("UserId").HasColumnType("uniqueidentifier");
			b.Property<Guid>("RoleId").HasColumnType("uniqueidentifier");
			b.HasKey("UserId", "RoleId");
			b.HasIndex("RoleId");
			b.ToTable("UserRoles", (string?)null);
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("UserId").HasColumnType("uniqueidentifier");
			b.Property<string>("LoginProvider").HasColumnType("nvarchar(450)");
			b.Property<string>("Name").HasColumnType("nvarchar(450)");
			b.Property<string>("Value").HasColumnType("nvarchar(max)");
			b.HasKey("UserId", "LoginProvider", "Name");
			b.ToTable("UserTokens", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Audit.AuditFinding", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Audit.AuditRun", "AuditRun").WithMany("Findings").HasForeignKey("AuditRunId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.HasOne("MADai.Domain.Tasks.TaskItem", "GeneratedTask").WithMany().HasForeignKey("GeneratedTaskId")
				.OnDelete(DeleteBehavior.NoAction);
			b.HasOne("MADai.Domain.Tasks.TaskItem", "RelatedTask").WithMany().HasForeignKey("RelatedTaskId")
				.OnDelete(DeleteBehavior.NoAction);
			b.Navigation("AuditRun");
			b.Navigation("GeneratedTask");
			b.Navigation("RelatedTask");
		});
		modelBuilder.Entity("MADai.Domain.Audit.AuditRecommendation", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Audit.AuditRun", "AuditRun").WithMany().HasForeignKey("AuditRunId");
			b.Navigation("AuditRun");
		});
		modelBuilder.Entity("MADai.Domain.Billing.Invoice", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Billing.Subscription", "Subscription").WithMany().HasForeignKey("SubscriptionId")
				.OnDelete(DeleteBehavior.NoAction);
			b.Navigation("Subscription");
		});
		modelBuilder.Entity("MADai.Domain.Billing.Payment", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Billing.Invoice", "Invoice").WithMany().HasForeignKey("InvoiceId")
				.OnDelete(DeleteBehavior.NoAction)
				.IsRequired();
			b.Navigation("Invoice");
		});
		modelBuilder.Entity("MADai.Domain.Billing.Subscription", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Billing.Plan", "Plan").WithMany().HasForeignKey("PlanId")
				.OnDelete(DeleteBehavior.NoAction)
				.IsRequired();
			b.Navigation("Plan");
		});
		modelBuilder.Entity("MADai.Domain.Files.FileAccessLog", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Files.FileItem", "File").WithMany("AccessLogs").HasForeignKey("FileId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.HasOne("MADai.Domain.Identity.ApplicationUser", "User").WithMany().HasForeignKey("UserId");
			b.Navigation("File");
			b.Navigation("User");
		});
		modelBuilder.Entity("MADai.Domain.Files.FileFolder", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Files.FileFolder", "ParentFolder").WithMany("Children").HasForeignKey("ParentFolderId")
				.OnDelete(DeleteBehavior.NoAction);
			b.Navigation("ParentFolder");
		});
		modelBuilder.Entity("MADai.Domain.Files.FileItem", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Files.FileFolder", "Folder").WithMany("Files").HasForeignKey("FolderId")
				.OnDelete(DeleteBehavior.NoAction);
			b.Navigation("Folder");
		});
		modelBuilder.Entity("MADai.Domain.Files.FileVersion", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Files.FileItem", "File").WithMany("Versions").HasForeignKey("FileId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("File");
		});
		modelBuilder.Entity("MADai.Domain.Identity.ApiKey", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Identity.ApplicationUser", "User").WithMany("ApiKeys").HasForeignKey("UserId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("User");
		});
		modelBuilder.Entity("MADai.Domain.Identity.ApplicationUser", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Tenancy.Company", "Company").WithMany("Users").HasForeignKey("CompanyId")
				.OnDelete(DeleteBehavior.NoAction);
			b.Navigation("Company");
		});
		modelBuilder.Entity("MADai.Domain.Identity.LoginHistory", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Identity.ApplicationUser", "User").WithMany("LoginHistory").HasForeignKey("UserId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("User");
		});
		modelBuilder.Entity("MADai.Domain.Identity.RefreshToken", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Identity.ApplicationUser", "User").WithMany("RefreshTokens").HasForeignKey("UserId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("User");
		});
		modelBuilder.Entity("MADai.Domain.Identity.RolePermission", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Identity.Permission", "Permission").WithMany().HasForeignKey("PermissionId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.HasOne("MADai.Domain.Identity.ApplicationRole", "Role").WithMany("RolePermissions").HasForeignKey("RoleId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("Permission");
			b.Navigation("Role");
		});
		modelBuilder.Entity("MADai.Domain.Identity.UserSession", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Identity.ApplicationUser", "User").WithMany().HasForeignKey("UserId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("User");
		});
		modelBuilder.Entity("MADai.Domain.Notifications.Notification", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Identity.ApplicationUser", "User").WithMany().HasForeignKey("UserId");
			b.Navigation("User");
		});
		modelBuilder.Entity("MADai.Domain.Notifications.NotificationPreference", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Identity.ApplicationUser", "User").WithMany().HasForeignKey("UserId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("User");
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskArtifact", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Tasks.TaskItem", "Task").WithMany("Artifacts").HasForeignKey("TaskId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("Task");
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskAssignment", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Workers.WorkerNode", "AssignedWorker").WithMany().HasForeignKey("AssignedWorkerId")
				.OnDelete(DeleteBehavior.NoAction);
			b.HasOne("MADai.Domain.Tasks.TaskItem", "Task").WithMany("Assignments").HasForeignKey("TaskId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("AssignedWorker");
			b.Navigation("Task");
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskComment", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Tasks.TaskItem", "Task").WithMany("Comments").HasForeignKey("TaskId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("Task");
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskDependency", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Tasks.TaskItem", "DependsOnTask").WithMany("DependentTasks").HasForeignKey("DependsOnTaskId")
				.OnDelete(DeleteBehavior.NoAction)
				.IsRequired();
			b.HasOne("MADai.Domain.Tasks.TaskItem", "Task").WithMany("Dependencies").HasForeignKey("TaskId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("DependsOnTask");
			b.Navigation("Task");
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskExecution", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Tasks.TaskItem", "Task").WithMany("Executions").HasForeignKey("TaskId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.HasOne("MADai.Domain.Workers.WorkerNode", "WorkerNode").WithMany().HasForeignKey("WorkerNodeId")
				.OnDelete(DeleteBehavior.NoAction)
				.IsRequired();
			b.Navigation("Task");
			b.Navigation("WorkerNode");
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskFailure", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Tasks.TaskItem", "Task").WithMany().HasForeignKey("TaskId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("Task");
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskItem", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Workers.WorkerNode", "ClaimedByWorker").WithMany("ClaimedTasks").HasForeignKey("ClaimedByWorkerId")
				.OnDelete(DeleteBehavior.SetNull);
			b.HasOne("MADai.Domain.Tasks.TaskItem", "ParentTask").WithMany("Subtasks").HasForeignKey("ParentTaskId")
				.OnDelete(DeleteBehavior.NoAction);
			b.HasOne("MADai.Domain.Tasks.TaskTemplate", "Template").WithMany().HasForeignKey("TemplateId")
				.OnDelete(DeleteBehavior.SetNull);
			b.Navigation("ClaimedByWorker");
			b.Navigation("ParentTask");
			b.Navigation("Template");
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskLog", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Tasks.TaskItem", "Task").WithMany("Logs").HasForeignKey("TaskId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("Task");
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskRecommendation", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Tasks.TaskItem", "Task").WithMany().HasForeignKey("TaskId")
				.OnDelete(DeleteBehavior.NoAction);
			b.Navigation("Task");
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskRetry", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Tasks.TaskItem", "Task").WithMany().HasForeignKey("TaskId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("Task");
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskTagLink", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Tasks.TaskTag", "Tag").WithMany("TagLinks").HasForeignKey("TagId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.HasOne("MADai.Domain.Tasks.TaskItem", "Task").WithMany("TagLinks").HasForeignKey("TaskId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("Tag");
			b.Navigation("Task");
		});
		modelBuilder.Entity("MADai.Domain.Tenancy.CompanyBranding", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Tenancy.Company", "Company").WithOne("Branding").HasForeignKey("MADai.Domain.Tenancy.CompanyBranding", "CompanyId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("Company");
		});
		modelBuilder.Entity("MADai.Domain.Tenancy.CompanySettings", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Tenancy.Company", "Company").WithOne("Settings").HasForeignKey("MADai.Domain.Tenancy.CompanySettings", "CompanyId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("Company");
		});
		modelBuilder.Entity("MADai.Domain.Workers.WorkerCapabilityEntry", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Workers.WorkerNode", "WorkerNode").WithMany().HasForeignKey("WorkerNodeId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("WorkerNode");
		});
		modelBuilder.Entity("MADai.Domain.Workers.WorkerHeartbeat", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Workers.WorkerNode", "WorkerNode").WithMany("Heartbeats").HasForeignKey("WorkerNodeId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("WorkerNode");
		});
		modelBuilder.Entity("MADai.Domain.Workers.WorkerMetric", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Workers.WorkerNode", "WorkerNode").WithMany("Metrics").HasForeignKey("WorkerNodeId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("WorkerNode");
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Identity.ApplicationRole", null).WithMany().HasForeignKey("RoleId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Identity.ApplicationUser", null).WithMany().HasForeignKey("UserId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Identity.ApplicationUser", null).WithMany().HasForeignKey("UserId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Identity.ApplicationRole", null).WithMany().HasForeignKey("RoleId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.HasOne("MADai.Domain.Identity.ApplicationUser", null).WithMany().HasForeignKey("UserId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", delegate(EntityTypeBuilder b)
		{
			b.HasOne("MADai.Domain.Identity.ApplicationUser", null).WithMany().HasForeignKey("UserId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("MADai.Domain.Audit.AuditRun", delegate(EntityTypeBuilder b)
		{
			b.Navigation("Findings");
		});
		modelBuilder.Entity("MADai.Domain.Files.FileFolder", delegate(EntityTypeBuilder b)
		{
			b.Navigation("Children");
			b.Navigation("Files");
		});
		modelBuilder.Entity("MADai.Domain.Files.FileItem", delegate(EntityTypeBuilder b)
		{
			b.Navigation("AccessLogs");
			b.Navigation("Versions");
		});
		modelBuilder.Entity("MADai.Domain.Identity.ApplicationRole", delegate(EntityTypeBuilder b)
		{
			b.Navigation("RolePermissions");
		});
		modelBuilder.Entity("MADai.Domain.Identity.ApplicationUser", delegate(EntityTypeBuilder b)
		{
			b.Navigation("ApiKeys");
			b.Navigation("LoginHistory");
			b.Navigation("RefreshTokens");
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskItem", delegate(EntityTypeBuilder b)
		{
			b.Navigation("Artifacts");
			b.Navigation("Assignments");
			b.Navigation("Comments");
			b.Navigation("Dependencies");
			b.Navigation("DependentTasks");
			b.Navigation("Executions");
			b.Navigation("Logs");
			b.Navigation("Subtasks");
			b.Navigation("TagLinks");
		});
		modelBuilder.Entity("MADai.Domain.Tasks.TaskTag", delegate(EntityTypeBuilder b)
		{
			b.Navigation("TagLinks");
		});
		modelBuilder.Entity("MADai.Domain.Tenancy.Company", delegate(EntityTypeBuilder b)
		{
			b.Navigation("Branding");
			b.Navigation("Settings");
			b.Navigation("Users");
		});
		modelBuilder.Entity("MADai.Domain.Workers.WorkerNode", delegate(EntityTypeBuilder b)
		{
			b.Navigation("ClaimedTasks");
			b.Navigation("Heartbeats");
			b.Navigation("Metrics");
		});
	}
}
