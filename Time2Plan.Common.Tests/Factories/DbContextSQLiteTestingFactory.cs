using Time2Plan.DAL;
using Microsoft.EntityFrameworkCore;

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
        
        // contextOptionsBuilder.LogTo(System.Console.WriteLine); //Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
        // builder.EnableSensitiveDataLogging();
        
        return new Time2PlanTestingDbContext(builder.Options, _seedTestingData);
    }
}