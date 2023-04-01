using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Time2Plan.DAL.Factories;

/// <summary>
///     EF Core CLI migration generation uses this DbContext to create model and migration
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<Time2PlanDbContext>
{
    private readonly DbContextSqLiteFactory _dbContextSqLiteFactory;
    private const string ConnectionString = $"Data Source=Time2Plan;Cache=Shared";

    public DesignTimeDbContextFactory()
    {
        _dbContextSqLiteFactory = new DbContextSqLiteFactory(ConnectionString);
    }

    public Time2PlanDbContext CreateDbContext(string[] args) => _dbContextSqLiteFactory.CreateDbContext();
}
