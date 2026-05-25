using MADai.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
	public void Configure(EntityTypeBuilder<RefreshToken> builder)
	{
		builder.ToTable("RefreshTokens");
		builder.HasKey((RefreshToken t) => t.Id);
		builder.Property((RefreshToken t) => t.Token).IsRequired().HasMaxLength(128);
		builder.Property((RefreshToken t) => t.CreatedByIp).HasMaxLength(64);
		builder.Property((RefreshToken t) => t.RevokedByIp).HasMaxLength(64);
		builder.Property((RefreshToken t) => t.ReplacedByToken).HasMaxLength(128);
		builder.Property((RefreshToken t) => t.ReasonRevoked).HasMaxLength(200);
		builder.HasOne((RefreshToken t) => t.User).WithMany((ApplicationUser u) => u.RefreshTokens).HasForeignKey((RefreshToken t) => t.UserId)
			.OnDelete(DeleteBehavior.Cascade);
		builder.HasIndex((RefreshToken t) => t.Token).IsUnique();
	}
}
