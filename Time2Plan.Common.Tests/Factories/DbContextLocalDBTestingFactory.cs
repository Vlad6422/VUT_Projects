using Microsoft.EntityFrameworkCore;
using Time2Plan.DAL;

namespace Time2Plan.Common.Tests.Factories;

public class DbContextLocalDBTestingFactory : IDbContextFactory<Time2PlanDbContext>
{
    private readonly string _databaseName;
    private readonly bool _seedTestingData;

    public DbContextLocalDBTestingFactory(string databaseName, bool seedTestingData = false)
    {
        _databaseName = databaseName;
        _seedTestingData = seedTestingData;
    }
    public Time2PlanDbContext CreateDbContext()
    {
        DbContextOptionsBuilder<Time2PlanDbContext> builder = new();
        builder.UseSqlServer($"Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog = {_databaseName};MultipleActiveResultSets = True;Integrated Security = True;");

        return new Time2PlanTestingDbContext(builder.Options, _seedTestingData);
    }
}