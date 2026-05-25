using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class NativeServiceConfiguration : IEntityTypeConfiguration<NativeService>
{
	public void Configure(EntityTypeBuilder<NativeService> builder)
	{
		builder.ToTable("NativeServices");
		builder.Property((NativeService x) => x.Name).IsRequired().HasMaxLength(160);
		builder.Property((NativeService x) => x.Kind).IsRequired().HasMaxLength(80);
		builder.Property((NativeService x) => x.Command).IsRequired().HasMaxLength(1000);
		builder.Property((NativeService x) => x.WorkingDirectory).HasMaxLength(600);
		builder.Property((NativeService x) => x.HealthCheckCommand).HasMaxLength(1000);
		builder.HasIndex((NativeService x) => x.Name).IsUnique();
		builder.HasIndex((NativeService x) => x.StartupOrder);
	}
}
