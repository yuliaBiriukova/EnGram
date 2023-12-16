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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EnGramDbContext).Assembly);
    }
}