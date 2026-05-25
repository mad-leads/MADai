using MADai.Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class TaskArtifactConfiguration : IEntityTypeConfiguration<TaskArtifact>
{
	public void Configure(EntityTypeBuilder<TaskArtifact> b)
	{
		b.ToTable("TaskArtifacts");
		b.Property((TaskArtifact x) => x.FileName).IsRequired().HasMaxLength(255);
		b.Property((TaskArtifact x) => x.ContentType).HasMaxLength(120);
		b.Property((TaskArtifact x) => x.StoragePath).IsRequired().HasMaxLength(500);
		b.Property((TaskArtifact x) => x.StorageProvider).HasMaxLength(40);
		b.Property((TaskArtifact x) => x.Checksum).HasMaxLength(128);
		b.Property((TaskArtifact x) => x.PreviewUrl).HasMaxLength(500);
		b.Property((TaskArtifact x) => x.Kind).HasMaxLength(40);
		b.HasIndex((TaskArtifact x) => x.TaskId);
	}
}
