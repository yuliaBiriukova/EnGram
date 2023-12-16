using EnGram.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace EnGram.DB.EntityConfigurations;

public class CompletedTopicConfiguration : IEntityTypeConfiguration<CompletedTopic>
{
    public void Configure(EntityTypeBuilder<CompletedTopic> builder)
    {
        builder.HasOne(c => c.Topic)
            .WithMany()
            .HasForeignKey(c => c.TopicId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}