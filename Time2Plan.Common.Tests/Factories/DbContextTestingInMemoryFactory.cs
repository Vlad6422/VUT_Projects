using Microsoft.EntityFrameworkCore;
using Time2Plan.DAL;

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

        return new Time2PlanTestingDbContext(contextOptionsBuilder.Options, _seedTestingData);
    }
}