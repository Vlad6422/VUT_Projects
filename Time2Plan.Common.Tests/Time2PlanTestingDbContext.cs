using Microsoft.EntityFrameworkCore;
using Time2Plan.Common.Tests.Seeds;
using Time2Plan.DAL;

namespace Time2Plan.Common.Tests;

public class Time2PlanTestingDbContext : Time2PlanDbContext
{
    private readonly bool _seedTestingData;

    public Time2PlanTestingDbContext(DbContextOptions contextOptions, bool seedTestingData = false)
        : base(contextOptions, seedData: false)
    {
        _seedTestingData = seedTestingData;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (_seedTestingData)
        {
            ActivitySeeds.Seed(modelBuilder);
            ProjectSeeds.Seed(modelBuilder);
            UserSeeds.Seed(modelBuilder);
        }
    }
}