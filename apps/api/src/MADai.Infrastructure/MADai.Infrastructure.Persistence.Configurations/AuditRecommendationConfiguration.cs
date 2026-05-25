using MADai.Domain.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class AuditRecommendationConfiguration : IEntityTypeConfiguration<AuditRecommendation>
{
	public void Configure(EntityTypeBuilder<AuditRecommendation> b)
	{
		b.ToTable("AuditRecommendations");
		b.Property((AuditRecommendation x) => x.Title).IsRequired().HasMaxLength(200);
		b.Property((AuditRecommendation x) => x.Body).IsRequired().HasMaxLength(4000);
	}
}
