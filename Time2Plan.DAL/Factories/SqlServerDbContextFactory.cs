using Microsoft.EntityFrameworkCore;

namespace Time2Plan.DAL.Factories;

public class SqlServerDbContextFactory : IDbContextFactory<Time2PlanDbContext>
{
    private readonly bool _seedDemoData;
    private readonly DbContextOptionsBuilder<Time2PlanDbContext> _contextOptionsBuilder = new();

    public SqlServerDbContextFactory(string connectionString, bool seedDemoData = false)
    {
        _seedDemoData = seedDemoData;

        _contextOptionsBuilder.UseSqlServer(connectionString);

        ////Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
        //_contextOptionsBuilder.LogTo(System.Console.WriteLine);
        //_contextOptionsBuilder.EnableSensitiveDataLogging();
    }

    public Time2PlanDbContext CreateDbContext() => new(_contextOptionsBuilder.Options, _seedDemoData);
}
