using EnGram.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnGram.DB.EntityConfigurations;

public class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.Property(e => e.Name).HasMaxLength(128);

        builder.HasOne(t => t.Level)
            .WithMany(l => l.Topics)
            .HasForeignKey(t => t.LevelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}