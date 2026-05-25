using MADai.Domain.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class AuditRunConfiguration : IEntityTypeConfiguration<AuditRun>
{
	public void Configure(EntityTypeBuilder<AuditRun> b)
	{
		b.ToTable("AuditRuns");
		b.Property((AuditRun x) => x.Status).HasMaxLength(40);
		b.Property((AuditRun x) => x.Summary).HasMaxLength(2000);
		b.HasMany((AuditRun x) => x.Findings).WithOne((AuditFinding f) => f.AuditRun).HasForeignKey((AuditFinding f) => f.AuditRunId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
