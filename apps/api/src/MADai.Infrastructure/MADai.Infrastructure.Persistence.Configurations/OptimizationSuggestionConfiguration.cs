using MADai.Domain.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class OptimizationSuggestionConfiguration : IEntityTypeConfiguration<OptimizationSuggestion>
{
	public void Configure(EntityTypeBuilder<OptimizationSuggestion> b)
	{
		b.ToTable("OptimizationSuggestions");
		b.Property((OptimizationSuggestion x) => x.Area).HasMaxLength(80);
		b.Property((OptimizationSuggestion x) => x.Title).HasMaxLength(200);
		b.Property((OptimizationSuggestion x) => x.Description).HasMaxLength(4000);
	}
}
