using MADai.Domain.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class AuditFindingConfiguration : IEntityTypeConfiguration<AuditFinding>
{
	public void Configure(EntityTypeBuilder<AuditFinding> b)
	{
		b.ToTable("AuditFindings");
		b.Property((AuditFinding x) => x.Title).IsRequired().HasMaxLength(200);
		b.Property((AuditFinding x) => x.Details).HasMaxLength(4000);
		b.Property((AuditFinding x) => x.AffectedResource).HasMaxLength(500);
		b.Property((AuditFinding x) => x.RecommendedAction).HasMaxLength(1000);
		b.HasOne((AuditFinding x) => x.RelatedTask).WithMany().HasForeignKey((AuditFinding x) => x.RelatedTaskId)
			.OnDelete(DeleteBehavior.NoAction);
		b.HasOne((AuditFinding x) => x.GeneratedTask).WithMany().HasForeignKey((AuditFinding x) => x.GeneratedTaskId)
			.OnDelete(DeleteBehavior.NoAction);
		b.HasIndex((AuditFinding x) => new { x.Status, x.Severity });
	}
}
