using EnGram.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnGram.DB.EntityConfigurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.Property(e => e.Task).HasMaxLength(256);
        builder.Property(e => e.Answer).HasMaxLength(256);

        builder.HasOne(e => e.Topic)
           .WithMany(t => t.Exercises)
           .HasForeignKey(e => e.TopicId)
           .OnDelete(DeleteBehavior.Cascade);
    }
}