using EnGram.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnGram.DB.EntityConfigurations;

public class LevelConfiguration : IEntityTypeConfiguration<Level>
{
    public void Configure(EntityTypeBuilder<Level> builder)
    {
        builder.Property(e => e.Code).HasMaxLength(10);
        builder.Property(e => e.Name).HasMaxLength(100);

        builder.HasData(
            new { Id = 1, Code = "A1", Name = "Beginner" },
            new { Id = 2, Code = "A2", Name = "Pre-Intermediate" },
            new { Id = 3, Code = "B1", Name = "Intermediate" },
            new { Id = 4, Code = "B2", Name = "Upper-Intermediate" },
            new { Id = 5, Code = "C1", Name = "Advanced" });

    }
}