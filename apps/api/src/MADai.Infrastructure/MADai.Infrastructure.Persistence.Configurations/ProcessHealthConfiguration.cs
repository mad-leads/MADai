using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class ProcessHealthConfiguration : IEntityTypeConfiguration<ProcessHealth>
{
	public void Configure(EntityTypeBuilder<ProcessHealth> builder)
	{
		builder.ToTable("ProcessHealth");
		builder.Property((ProcessHealth x) => x.ProcessKey).IsRequired().HasMaxLength(160);
		builder.Property((ProcessHealth x) => x.ProcessName).IsRequired().HasMaxLength(160);
		builder.Property((ProcessHealth x) => x.Status).IsRequired().HasMaxLength(80);
		builder.HasIndex((ProcessHealth x) => new { x.ProcessKey, x.CheckedAt });
	}
}
