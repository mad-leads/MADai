using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class WorkerMemoryConfiguration : IEntityTypeConfiguration<WorkerMemory>
{
	public void Configure(EntityTypeBuilder<WorkerMemory> builder)
	{
		builder.ToTable("WorkerMemory");
		builder.Property((WorkerMemory x) => x.RepositoryKey).IsRequired().HasMaxLength(160);
		builder.Property((WorkerMemory x) => x.MemoryKey).IsRequired().HasMaxLength(200);
		builder.HasIndex((WorkerMemory x) => new { x.RepositoryKey, x.MemoryKey });
		builder.HasIndex((WorkerMemory x) => x.LastUsedAt);
	}
}
