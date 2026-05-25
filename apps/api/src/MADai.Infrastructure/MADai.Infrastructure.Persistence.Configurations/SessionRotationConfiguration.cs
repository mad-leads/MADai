using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class SessionRotationConfiguration : IEntityTypeConfiguration<SessionRotation>
{
	public void Configure(EntityTypeBuilder<SessionRotation> builder)
	{
		builder.ToTable("SessionRotations");
		builder.Property((SessionRotation x) => x.RepositoryKey).IsRequired().HasMaxLength(160);
		builder.Property((SessionRotation x) => x.OldSessionId).IsRequired().HasMaxLength(120);
		builder.Property((SessionRotation x) => x.NewSessionId).HasMaxLength(120);
		builder.Property((SessionRotation x) => x.Reason).IsRequired().HasMaxLength(500);
		builder.HasIndex((SessionRotation x) => new { x.RepositoryKey, x.RotatedAt });
	}
}
