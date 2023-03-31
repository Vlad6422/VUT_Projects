using Time2Plan.DAL;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Common.Tests.Factories;

public class DbContextLocalDBTestingFactory : IDbContextFactory<ApplicationContext>
{
    private readonly string _databaseName;
    private readonly bool _seedTestingData;

    public DbContextLocalDBTestingFactory(string databaseName, bool seedTestingData = false)
    {
        _databaseName = databaseName;
        _seedTestingData = seedTestingData;
    }
    public ApplicationContext CreateDbContext()
    {
        DbContextOptionsBuilder<ApplicationContext> builder = new();
        builder.UseSqlServer($"Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog = {_databaseName};MultipleActiveResultSets = True;Integrated Security = True; ");
        
        // contextOptionsBuilder.LogTo(System.Console.WriteLine); //Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
        // builder.EnableSensitiveDataLogging();
        
        return new ApplicationTestingDbContext(builder.Options, _seedTestingData);
    }
}