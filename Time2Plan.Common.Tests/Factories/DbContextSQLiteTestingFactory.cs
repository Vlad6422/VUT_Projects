using Microsoft.EntityFrameworkCore;
using Time2Plan.DAL;

namespace Time2Plan.Common.Tests.Factories;

public class DbContextSqLiteTestingFactory : IDbContextFactory<Time2PlanDbContext>
{
    private readonly string _databaseName;
    private readonly bool _seedTestingData;

    public DbContextSqLiteTestingFactory(string databaseName, bool seedTestingData = false)
    {
        _databaseName = databaseName;
        _seedTestingData = seedTestingData;
    }
    public Time2PlanDbContext CreateDbContext()
    {
        DbContextOptionsBuilder<Time2PlanDbContext> builder = new();
        builder.UseSqlite($"Data Source={_databaseName};Cache=Shared");

        return new Time2PlanTestingDbContext(builder.Options, _seedTestingData);
    }
}