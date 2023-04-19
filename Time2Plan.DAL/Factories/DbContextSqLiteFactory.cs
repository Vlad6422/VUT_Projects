using Microsoft.EntityFrameworkCore;

namespace Time2Plan.DAL.Factories;

public class DbContextSqLiteFactory : IDbContextFactory<Time2PlanDbContext>
{
    private readonly bool _seedTestingData;
    private readonly DbContextOptionsBuilder<Time2PlanDbContext> _contextOptionsBuilder = new();

    public DbContextSqLiteFactory(string databaseName, bool seedTestingData = false)
    {
        _seedTestingData = seedTestingData;

        _contextOptionsBuilder.UseSqlite($"Data Source={databaseName};Cache=Shared");
    }

    public Time2PlanDbContext CreateDbContext() => new(_contextOptionsBuilder.Options, _seedTestingData);
}
