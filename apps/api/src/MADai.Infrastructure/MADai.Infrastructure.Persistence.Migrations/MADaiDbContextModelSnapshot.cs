using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Migrations;

[DbContext(typeof(MADaiDbContext))]
internal class MADaiDbContextModelSnapshot : ModelSnapshot
{
	protected override void BuildModel(ModelBuilder modelBuilder)
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
		modelBuilder.Entity("MADai.Domain.Tasks.ClaudePromptTemplate", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Content").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<string>("Description").HasMaxLength(2000).HasColumnType("nvarchar(2000)");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("Name").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.HasKey("Id");
			b.HasIndex("Name").IsUnique();
			b.ToTable("ClaudePromptTemplates", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Tasks.ClaudeTask", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("AttachmentsJson").HasColumnType("nvarchar(max)");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<string>("Description").HasMaxLength(8000).HasColumnType("nvarchar(max)");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("Notes").HasMaxLength(8000).HasColumnType("nvarchar(max)");
			b.Property<int>("Priority").HasColumnType("int");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<int>("Status").HasColumnType("int");
			b.Property<string>("Title").IsRequired().HasMaxLength(300)
				.HasColumnType("nvarchar(300)");
			b.HasKey("Id");
			b.HasIndex("CreatedDate");
			b.HasIndex("Status", "Priority", "Id");
			b.ToTable("ClaudeTasks", (string?)null);
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
		modelBuilder.Entity("MADai.Domain.Webhooks.WebhookEvent", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<int>("AttemptCount").HasColumnType("int");
			b.Property<Guid>("CompanyId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedAt").HasColumnType("datetime2");
			b.Property<DateTime?>("DeliveredAt").HasColumnType("datetime2");
			b.Property<Guid>("EndpointId").HasColumnType("uniqueidentifier");
			b.Property<string>("EventType").IsRequired().HasMaxLength(80)
				.HasColumnType("nvarchar(80)");
			b.Property<string>("LastError").HasMaxLength(2000).HasColumnType("nvarchar(2000)");
			b.Property<int?>("LastResponseStatus").HasColumnType("int");
			b.Property<DateTime?>("NextAttemptAt").HasColumnType("datetime2");
			b.Property<string>("PayloadJson").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("Status").IsRequired().HasMaxLength(20)
				.HasColumnType("nvarchar(20)");
			b.HasKey("Id");
			b.HasIndex("EndpointId", "CreatedAt");
			b.HasIndex("Status", "NextAttemptAt");
			b.ToTable("WebhookEvents", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Workers.ArchitectureSummary", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Content").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<DateTime>("GeneratedAt").HasColumnType("datetime2");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("RepositoryKey").IsRequired().HasMaxLength(160)
				.HasColumnType("nvarchar(160)");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<string>("Scope").IsRequired().HasMaxLength(80)
				.HasColumnType("nvarchar(80)");
			b.HasKey("Id");
			b.HasIndex("RepositoryKey", "Scope");
			b.ToTable("ArchitectureSummaries", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Workers.DependencyGraph", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<DateTime>("GeneratedAt").HasColumnType("datetime2");
			b.Property<string>("GraphJson").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("RepositoryKey").IsRequired().HasMaxLength(160)
				.HasColumnType("nvarchar(160)");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.HasKey("Id");
			b.HasIndex("RepositoryKey");
			b.ToTable("DependencyGraphs", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Workers.EntityMap", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<DateTime>("GeneratedAt").HasColumnType("datetime2");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<string>("MapJson").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("RepositoryKey").IsRequired().HasMaxLength(160)
				.HasColumnType("nvarchar(160)");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.HasKey("Id");
			b.HasIndex("RepositoryKey");
			b.ToTable("EntityMaps", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Workers.NativeService", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Command").IsRequired().HasMaxLength(1000)
				.HasColumnType("nvarchar(1000)");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<string>("HealthCheckCommand").HasMaxLength(1000).HasColumnType("nvarchar(1000)");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<bool>("IsEnabled").HasColumnType("bit");
			b.Property<string>("Kind").IsRequired().HasMaxLength(80)
				.HasColumnType("nvarchar(80)");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("Name").IsRequired().HasMaxLength(160)
				.HasColumnType("nvarchar(160)");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<int>("StartupOrder").HasColumnType("int");
			b.Property<string>("WorkingDirectory").IsRequired().HasMaxLength(600)
				.HasColumnType("nvarchar(600)");
			b.HasKey("Id");
			b.HasIndex("Name").IsUnique();
			b.HasIndex("StartupOrder");
			b.ToTable("NativeServices", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Workers.ProcessHealth", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CheckedAt").HasColumnType("datetime2");
			b.Property<string>("Details").HasColumnType("nvarchar(max)");
			b.Property<double>("MemoryMb").HasColumnType("float");
			b.Property<int?>("ProcessId").HasColumnType("int");
			b.Property<string>("ProcessKey").IsRequired().HasMaxLength(160)
				.HasColumnType("nvarchar(160)");
			b.Property<string>("ProcessName").IsRequired().HasMaxLength(160)
				.HasColumnType("nvarchar(160)");
			b.Property<string>("Status").IsRequired().HasMaxLength(80)
				.HasColumnType("nvarchar(80)");
			b.HasKey("Id");
			b.HasIndex("ProcessKey", "CheckedAt");
			b.ToTable("ProcessHealth", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Workers.ProcessRestart", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Details").HasColumnType("nvarchar(max)");
			b.Property<string>("ProcessKey").IsRequired().HasMaxLength(160)
				.HasColumnType("nvarchar(160)");
			b.Property<string>("Reason").IsRequired().HasMaxLength(500)
				.HasColumnType("nvarchar(500)");
			b.Property<DateTime>("RestartedAt").HasColumnType("datetime2");
			b.Property<bool>("Succeeded").HasColumnType("bit");
			b.HasKey("Id");
			b.HasIndex("ProcessKey", "RestartedAt");
			b.ToTable("ProcessRestarts", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Workers.RepositoryCache", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("CacheJson").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("CacheKey").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<DateTime>("ExpiresAt").HasColumnType("datetime2");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("RepositoryKey").IsRequired().HasMaxLength(160)
				.HasColumnType("nvarchar(160)");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.HasKey("Id");
			b.HasIndex("ExpiresAt");
			b.HasIndex("RepositoryKey", "CacheKey").IsUnique();
			b.ToTable("RepositoryCaches", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Workers.RepositoryIntelligence", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("ArchitectureJson").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("BranchName").HasMaxLength(160).HasColumnType("nvarchar(160)");
			b.Property<string>("BuildInstructions").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("CacheFingerprint").IsRequired().HasMaxLength(120)
				.HasColumnType("nvarchar(120)");
			b.Property<string>("CommitSha").HasMaxLength(80).HasColumnType("nvarchar(80)");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<string>("DependencyGraphJson").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("EndpointMapJson").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("EntityMapJson").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("RepositoryKey").IsRequired().HasMaxLength(160)
				.HasColumnType("nvarchar(160)");
			b.Property<string>("RepositoryPath").IsRequired().HasMaxLength(600)
				.HasColumnType("nvarchar(600)");
			b.Property<string>("RouteMapJson").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<DateTime>("ScannedAt").HasColumnType("datetime2");
			b.Property<string>("Summary").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("ValidationInstructions").IsRequired().HasColumnType("nvarchar(max)");
			b.HasKey("Id");
			b.HasIndex("RepositoryKey").IsUnique();
			b.HasIndex("ScannedAt");
			b.ToTable("RepositoryIntelligence", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Workers.RouteMap", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<DateTime>("GeneratedAt").HasColumnType("datetime2");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<string>("Kind").IsRequired().HasMaxLength(80)
				.HasColumnType("nvarchar(80)");
			b.Property<string>("MapJson").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("RepositoryKey").IsRequired().HasMaxLength(160)
				.HasColumnType("nvarchar(160)");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.HasKey("Id");
			b.HasIndex("RepositoryKey", "Kind");
			b.ToTable("RouteMaps", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Workers.ServiceDependency", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<string>("DependsOnServiceName").IsRequired().HasMaxLength(160)
				.HasColumnType("nvarchar(160)");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<bool>("IsRequired").HasColumnType("bit");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<string>("ServiceName").IsRequired().HasMaxLength(160)
				.HasColumnType("nvarchar(160)");
			b.HasKey("Id");
			b.HasIndex("ServiceName", "DependsOnServiceName").IsUnique();
			b.ToTable("ServiceDependencies", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Workers.SessionCheckpoint", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CapturedAt").HasColumnType("datetime2");
			b.Property<string>("CheckpointJson").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("RepositoryKey").IsRequired().HasMaxLength(160)
				.HasColumnType("nvarchar(160)");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<string>("SessionId").IsRequired().HasMaxLength(120)
				.HasColumnType("nvarchar(120)");
			b.Property<string>("Summary").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<Guid?>("WorkerNodeId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("CapturedAt");
			b.HasIndex("RepositoryKey", "SessionId");
			b.ToTable("SessionCheckpoints", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Workers.SessionMetric", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<int>("ActiveTaskCount").HasColumnType("int");
			b.Property<int>("EstimatedTokens").HasColumnType("int");
			b.Property<string>("Health").IsRequired().HasMaxLength(80)
				.HasColumnType("nvarchar(80)");
			b.Property<double>("MemoryMb").HasColumnType("float");
			b.Property<string>("Notes").HasColumnType("nvarchar(max)");
			b.Property<string>("RepositoryKey").IsRequired().HasMaxLength(160)
				.HasColumnType("nvarchar(160)");
			b.Property<string>("SessionId").IsRequired().HasMaxLength(120)
				.HasColumnType("nvarchar(120)");
			b.Property<DateTime>("Timestamp").HasColumnType("datetime2");
			b.Property<Guid?>("WorkerNodeId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("RepositoryKey", "SessionId", "Timestamp");
			b.ToTable("SessionMetrics", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Workers.SessionRotation", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("NewSessionId").HasMaxLength(120).HasColumnType("nvarchar(120)");
			b.Property<string>("OldSessionId").IsRequired().HasMaxLength(120)
				.HasColumnType("nvarchar(120)");
			b.Property<string>("Reason").IsRequired().HasMaxLength(500)
				.HasColumnType("nvarchar(500)");
			b.Property<string>("RepositoryKey").IsRequired().HasMaxLength(160)
				.HasColumnType("nvarchar(160)");
			b.Property<DateTime>("RotatedAt").HasColumnType("datetime2");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<string>("SummaryBeforeRotation").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<Guid?>("WorkerNodeId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("RepositoryKey", "RotatedAt");
			b.ToTable("SessionRotations", (string?)null);
		});
		modelBuilder.Entity("MADai.Domain.Workers.TokenUsage", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<int>("CompletionTokens").HasColumnType("int");
			b.Property<int>("PromptTokens").HasColumnType("int");
			b.Property<string>("RepositoryKey").IsRequired().HasMaxLength(160)
				.HasColumnType("nvarchar(160)");
			b.Property<string>("SessionId").IsRequired().HasMaxLength(120)
				.HasColumnType("nvarchar(120)");
			b.Property<Guid?>("TaskId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("Timestamp").HasColumnType("datetime2");
			b.Property<int>("TotalTokens").HasColumnType("int");
			b.HasKey("Id");
			b.HasIndex("RepositoryKey", "SessionId", "Timestamp");
			b.ToTable("TokenUsage", (string?)null);
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
		modelBuilder.Entity("MADai.Domain.Workers.WorkerMemory", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<DateTime>("LastUsedAt").HasColumnType("datetime2");
			b.Property<string>("MemoryJson").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("MemoryKey").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<string>("RepositoryKey").IsRequired().HasMaxLength(160)
				.HasColumnType("nvarchar(160)");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<Guid?>("WorkerNodeId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("LastUsedAt");
			b.HasIndex("RepositoryKey", "MemoryKey");
			b.ToTable("WorkerMemory", (string?)null);
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
		modelBuilder.Entity("MADai.Domain.Workers.WorkerProcess", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<Guid?>("CreatedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
			b.Property<Guid?>("DeletedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("DeletedDate").HasColumnType("datetime2");
			b.Property<bool>("IsDeleted").HasColumnType("bit");
			b.Property<DateTime?>("LastSeenAt").HasColumnType("datetime2");
			b.Property<Guid?>("ModifiedByUserId").HasColumnType("uniqueidentifier");
			b.Property<DateTime?>("ModifiedDate").HasColumnType("datetime2");
			b.Property<int?>("ProcessId").HasColumnType("int");
			b.Property<string>("ProcessKey").IsRequired().HasMaxLength(160)
				.HasColumnType("nvarchar(160)");
			b.Property<string>("RepositoryKey").IsRequired().HasMaxLength(160)
				.HasColumnType("nvarchar(160)");
			b.Property<byte[]>("RowVersion").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
				.HasColumnType("rowversion");
			b.Property<string>("SessionId").IsRequired().HasMaxLength(120)
				.HasColumnType("nvarchar(120)");
			b.Property<DateTime>("StartedAt").HasColumnType("datetime2");
			b.Property<string>("Status").IsRequired().HasMaxLength(80)
				.HasColumnType("nvarchar(80)");
			b.Property<Guid?>("WorkerNodeId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("SessionId");
			b.HasIndex("RepositoryKey", "ProcessKey");
			b.ToTable("WorkerProcesses", (string?)null);
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
		modelBuilder.Entity("MADai.Domain.Workers.WorkerStatistic", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("MetricName").IsRequired().HasMaxLength(120)
				.HasColumnType("nvarchar(120)");
			b.Property<string>("RepositoryKey").IsRequired().HasMaxLength(160)
				.HasColumnType("nvarchar(160)");
			b.Property<DateTime>("Timestamp").HasColumnType("datetime2");
			b.Property<string>("Unit").HasMaxLength(40).HasColumnType("nvarchar(40)");
			b.Property<double>("Value").HasColumnType("float");
			b.Property<Guid?>("WorkerNodeId").HasColumnType("uniqueidentifier");
			b.HasKey("Id");
			b.HasIndex("RepositoryKey", "MetricName", "Timestamp");
			b.ToTable("WorkerStatistics", (string?)null);
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
