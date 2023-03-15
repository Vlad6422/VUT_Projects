using Microsoft.EntityFrameworkCore;
using Time2Plan.DAL.Interfaces;

namespace Time2Plan.DAL;

public class ApplicationContext : DbContext
{
    // nastaveni propojeni entit s databazi
    public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<ActivityEntity> Activities => Set<ActivityEntity>();

    private readonly bool _seedTestData;
    public ApplicationContext(DbContextOptions options, bool seedData) : base(options) => _seedTestData = seedData;


    // nastaveni modelu databaze - jednotlivych vazeb + pripadne delete constraint a jine(M-N, M-1, One - One)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ProjectEntity>()
            .HasMany<ActivityEntity>();
    }

}
