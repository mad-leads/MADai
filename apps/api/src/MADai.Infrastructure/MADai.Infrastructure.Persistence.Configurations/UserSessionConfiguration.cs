using MADai.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
	public void Configure(EntityTypeBuilder<UserSession> builder)
	{
		builder.ToTable("UserSessions");
		builder.HasKey((UserSession s) => s.Id);
		builder.Property((UserSession s) => s.SessionToken).IsRequired().HasMaxLength(128);
		builder.Property((UserSession s) => s.IpAddress).HasMaxLength(64);
		builder.Property((UserSession s) => s.UserAgent).HasMaxLength(500);
		builder.Property((UserSession s) => s.DeviceFingerprint).HasMaxLength(200);
		builder.HasOne((UserSession s) => s.User).WithMany().HasForeignKey((UserSession s) => s.UserId)
			.OnDelete(DeleteBehavior.Cascade);
		builder.HasIndex((UserSession s) => s.SessionToken).IsUnique();
	}
}
