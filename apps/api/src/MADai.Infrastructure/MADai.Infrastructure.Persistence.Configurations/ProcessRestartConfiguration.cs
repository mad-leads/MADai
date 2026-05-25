using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class ProcessRestartConfiguration : IEntityTypeConfiguration<ProcessRestart>
{
	public void Configure(EntityTypeBuilder<ProcessRestart> builder)
	{
		builder.ToTable("ProcessRestarts");
		builder.Property((ProcessRestart x) => x.ProcessKey).IsRequired().HasMaxLength(160);
		builder.Property((ProcessRestart x) => x.Reason).IsRequired().HasMaxLength(500);
		builder.HasIndex((ProcessRestart x) => new { x.ProcessKey, x.RestartedAt });
	}
}
