using MADai.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class LoginHistoryConfiguration : IEntityTypeConfiguration<LoginHistory>
{
	public void Configure(EntityTypeBuilder<LoginHistory> builder)
	{
		builder.ToTable("LoginHistory");
		builder.HasKey((LoginHistory h) => h.Id);
		builder.Property((LoginHistory h) => h.IpAddress).HasMaxLength(64);
		builder.Property((LoginHistory h) => h.UserAgent).HasMaxLength(500);
		builder.Property((LoginHistory h) => h.DeviceFingerprint).HasMaxLength(200);
		builder.Property((LoginHistory h) => h.FailureReason).HasMaxLength(200);
		builder.Property((LoginHistory h) => h.Country).HasMaxLength(80);
		builder.Property((LoginHistory h) => h.City).HasMaxLength(120);
		builder.HasOne((LoginHistory h) => h.User).WithMany((ApplicationUser u) => u.LoginHistory).HasForeignKey((LoginHistory h) => h.UserId)
			.OnDelete(DeleteBehavior.Cascade);
		builder.HasIndex((LoginHistory h) => new { h.UserId, h.LoginAt });
	}
}
