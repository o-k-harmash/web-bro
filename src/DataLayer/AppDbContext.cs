using Microsoft.EntityFrameworkCore;
using WebBro.DataLayer.EfClasses;

namespace DataLayer;

public class AppDbContext : DbContext
{
    private static string _connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=postgres";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(_connectionString);

    public DbSet<LearningPath> LearningPaths { get; set; }
    public DbSet<Step> Steps { get; set; }
    public DbSet<StepProgress> StepProgresses { get; set; }
    public DbSet<Challenge> Challenges { get; set; }
    public DbSet<Article> Articles { get; set; }
}
