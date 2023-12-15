using EnGram.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnGram.DB.Database;

public class EnGramDbContext : DbContext
{
    public DbSet<Level> Levels { get; set; }

    public DbSet<Topic> Topics { get; set; }

    public DbSet<Exercise> Exercises { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<CompletedTopic> CompletedTopics { get; set; }

    public EnGramDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CompletedTopic>()
            .HasOne(c => c.Topic)
            .WithMany()
            .HasForeignKey(c => c.TopicId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CompletedTopic>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EnGramDbContext).Assembly);
    }
}