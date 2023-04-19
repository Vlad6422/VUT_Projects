using Microsoft.EntityFrameworkCore;
using Time2Plan.DAL.Entities;

namespace Time2Plan.DAL;

public class Time2PlanDbContext : DbContext
{
    // nastaveni propojeni entit s databazi
    public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<ActivityEntity> Activities => Set<ActivityEntity>();
    public DbSet<ProjectUserRelation> UserProjects => Set<ProjectUserRelation>();

    private readonly bool _seedTestData;
    public Time2PlanDbContext(DbContextOptions options, bool seedData = false) : base(options) => _seedTestData = seedData;


    // nastaveni modelu databaze - jednotlivych vazeb + pripadne delete constraint a jine(M-N, M-1, One - One)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProjectUserRelation>().HasKey(up => new { up.UserId, up.ProjectId });

        modelBuilder.Entity<ProjectUserRelation>()
            .HasOne<ProjectEntity>(up => up.Project)
            .WithMany(p => p.UserProjects)
            .HasForeignKey(up => up.ProjectId);

        modelBuilder.Entity<ProjectUserRelation>()
            .HasOne<UserEntity>(up => up.User)
            .WithMany(u => u.UserProjects)
            .HasForeignKey(up => up.UserId);

        modelBuilder.Entity<ProjectEntity>()
            .HasMany(i => i.Activities)
            .WithOne(i => i.Project)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserEntity>()
            .HasMany(a => a.Activities)
            .WithOne(u => u.User)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
