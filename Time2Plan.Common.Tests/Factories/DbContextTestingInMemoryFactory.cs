using Time2Plan.DAL;
using Microsoft.EntityFrameworkCore;

namespace Time2Plan.Common.Tests.Factories;

public class DbContextTestingInMemoryFactory : IDbContextFactory<Time2PlanDbContext>
{
    private readonly string _databaseName;
    private readonly bool _seedTestingData;

    public DbContextTestingInMemoryFactory(string databaseName, bool seedTestingData = false)
    {
        _databaseName = databaseName;
        _seedTestingData = seedTestingData;
    }

    public Time2PlanDbContext CreateDbContext()
    {
        DbContextOptionsBuilder<Time2PlanDbContext> contextOptionsBuilder = new();
        contextOptionsBuilder.UseInMemoryDatabase(_databaseName);

        // contextOptionsBuilder.LogTo(System.Console.WriteLine); //Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
        // builder.EnableSensitiveDataLogging();

        return new Time2PlanTestingDbContext(contextOptionsBuilder.Options, _seedTestingData);
    }
}