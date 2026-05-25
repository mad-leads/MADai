using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class ServiceDependencyConfiguration : IEntityTypeConfiguration<ServiceDependency>
{
	public void Configure(EntityTypeBuilder<ServiceDependency> builder)
	{
		builder.ToTable("ServiceDependencies");
		builder.Property((ServiceDependency x) => x.ServiceName).IsRequired().HasMaxLength(160);
		builder.Property((ServiceDependency x) => x.DependsOnServiceName).IsRequired().HasMaxLength(160);
		builder.HasIndex((ServiceDependency x) => new { x.ServiceName, x.DependsOnServiceName }).IsUnique();
	}
}
