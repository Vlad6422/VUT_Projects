using System;
using Microsoft.EntityFrameworkCore;

namespace Time2Plan.DAL.Factories;

public class DbContextSqLiteFactory : IDbContextFactory<Time2PlanDbContext>
{
    private readonly bool _seedTestingData;
    private readonly DbContextOptionsBuilder<Time2PlanDbContext> _contextOptionsBuilder = new();

    public DbContextSqLiteFactory(string databaseName, bool seedTestingData = false)
    {
        _seedTestingData = seedTestingData;

        ////May be helpful for ad-hoc testing, not drop in replacement, needs some more configuration.
        //builder.UseSqlite($"Data Source =:memory:;");
        _contextOptionsBuilder.UseSqlite($"Data Source={databaseName};Cache=Shared");

        ////Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
        //_contextOptionsBuilder.EnableSensitiveDataLogging();
        //_contextOptionsBuilder.LogTo(Console.WriteLine);
    }

    public Time2PlanDbContext CreateDbContext() => new(_contextOptionsBuilder.Options, _seedTestingData);
}
