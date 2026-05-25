using MADai.Domain.SystemEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
	public void Configure(EntityTypeBuilder<AuditLog> b)
	{
		b.ToTable("AuditLogs");
		b.Property((AuditLog x) => x.Action).IsRequired().HasMaxLength(120);
		b.Property((AuditLog x) => x.EntityType).HasMaxLength(120);
		b.Property((AuditLog x) => x.EntityId).HasMaxLength(60);
		b.Property((AuditLog x) => x.IpAddress).HasMaxLength(64);
		b.Property((AuditLog x) => x.Severity).HasMaxLength(20);
		b.Property((AuditLog x) => x.Detail).HasMaxLength(4000);
		b.HasIndex((AuditLog x) => new { x.EntityType, x.EntityId });
	}
}
