using Time2Plan.DAL;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Common.Tests.Factories;

public class DbContextTestingInMemoryFactory : IDbContextFactory<ApplicationContext>
{
    private readonly string _databaseName;
    private readonly bool _seedTestingData;

    public DbContextTestingInMemoryFactory(string databaseName, bool seedTestingData = false)
    {
        _databaseName = databaseName;
        _seedTestingData = seedTestingData;
    }

    public ApplicationContext CreateDbContext()
    {
        DbContextOptionsBuilder<ApplicationContext> contextOptionsBuilder = new();
        contextOptionsBuilder.UseInMemoryDatabase(_databaseName);

        // contextOptionsBuilder.LogTo(System.Console.WriteLine); //Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
        // builder.EnableSensitiveDataLogging();

        return new ApplicationTestingDbContext(contextOptionsBuilder.Options, _seedTestingData);
    }
}