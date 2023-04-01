using System;
using System.Threading.Tasks;
using Time2Plan.Common.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;
using Time2Plan.DAL;
using Time2Plan.Common.Tests.Factories;

namespace Time2Plan.DAL.Tests;

public class  DbContextTestsBase : IAsyncLifetime
{
    protected DbContextTestsBase(ITestOutputHelper output)
    {
        XUnitTestOutputConverter converter = new(output);
        Console.SetOut(converter);
        
        // DbContextFactory = new DbContextTestingInMemoryFactory(GetType().Name, seedTestingData: true);
        DbContextFactory = new DbContextLocalDBTestingFactory(GetType().FullName!, seedTestingData: true);
        //DbContextFactory = new DbContextSqLiteTestingFactory(GetType().FullName!, seedTestingData: true);

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