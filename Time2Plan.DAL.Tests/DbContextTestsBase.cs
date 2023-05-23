using Microsoft.EntityFrameworkCore;
using Time2Plan.Common.Tests;
using Time2Plan.Common.Tests.Factories;
using Xunit;
using Xunit.Abstractions;

namespace Time2Plan.DAL.Tests;

public class DbContextTestsBase : IAsyncLifetime
{
    protected DbContextTestsBase(ITestOutputHelper output)
    {
        XUnitTestOutputConverter converter = new(output);
        Console.SetOut(converter);

        DbContextFactory = new DbContextSqLiteTestingFactory(GetType().FullName!, seedTestingData: true);

        Time2PlanDbContextSUT = DbContextFactory.CreateDbContext();
    }

    protected IDbContextFactory<Time2PlanDbContext> DbContextFactory { get; }
    protected Time2PlanDbContext Time2PlanDbContextSUT { get; }


    public async Task InitializeAsync()
    {
        await Time2PlanDbContextSUT.Database.EnsureDeletedAsync();
        await Time2PlanDbContextSUT.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await Time2PlanDbContextSUT.Database.EnsureDeletedAsync();
        await Time2PlanDbContextSUT.DisposeAsync();
    }
}