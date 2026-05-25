using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class TokenUsageConfiguration : IEntityTypeConfiguration<TokenUsage>
{
	public void Configure(EntityTypeBuilder<TokenUsage> builder)
	{
		builder.ToTable("TokenUsage");
		builder.Property((TokenUsage x) => x.SessionId).IsRequired().HasMaxLength(120);
		builder.Property((TokenUsage x) => x.RepositoryKey).IsRequired().HasMaxLength(160);
		builder.HasIndex((TokenUsage x) => new { x.RepositoryKey, x.SessionId, x.Timestamp });
	}
}
