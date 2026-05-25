using MADai.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class ApiKeyConfiguration : IEntityTypeConfiguration<ApiKey>
{
	public void Configure(EntityTypeBuilder<ApiKey> builder)
	{
		builder.ToTable("ApiKeys");
		builder.HasKey((ApiKey k) => k.Id);
		builder.Property((ApiKey k) => k.Name).IsRequired().HasMaxLength(120);
		builder.Property((ApiKey k) => k.KeyPrefix).IsRequired().HasMaxLength(20);
		builder.Property((ApiKey k) => k.KeyHash).IsRequired().HasMaxLength(200);
		builder.Property((ApiKey k) => k.Scopes).HasMaxLength(1000);
		builder.HasOne((ApiKey k) => k.User).WithMany((ApplicationUser u) => u.ApiKeys).HasForeignKey((ApiKey k) => k.UserId)
			.OnDelete(DeleteBehavior.Cascade);
		builder.HasIndex((ApiKey k) => k.KeyPrefix);
	}
}
