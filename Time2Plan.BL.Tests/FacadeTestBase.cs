using Time2Plan.BL.Mappers;
using Time2Plan.Common.Tests;
using Time2Plan.Common.Tests.Factories;
using Time2Plan.DAL;
using Time2Plan.DAL.Mappers;
using Time2Plan.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Time2Plan.BL.Mappers.Interfaces;

namespace Time2Plan.BL.Tests;

public class FacadeTestBase : IAsyncLifetime
{
    protected FacadeTestBase(ITestOutputHelper output)
    {
        XUnitTestOutputConverter converter = new(output);
        Console.SetOut(converter);

        //DbContextFactory = new DbContextTeestingInMemoryFactory(GetType().Name, seedTestingData: true);
        DbContextFactory = new DbContextLocalDBTestingFactory(GetType().FullName!, seedTestingData: true);
        //DbContextFactory = new DbContextSqLiteTestingFactory(GetType().FullName!, seedTestingData: true);

        ActivityEntityMapper = new ActivityEntityMapper();
        ProjectEntityMapper = new ProjectEntityMapper();
        ProjectUserRelationMapper = new ProjectUserRelationMapper();
        UserEntityMapper = new UserEntityMapper();

        ActivityModelMapper = new ActivityModelMapper();
        ProjectModelMapper = new ProjectModelMapper(ActivityModelMapper);
        UserModelMapper = new UserModelMapper(ActivityModelMapper);
        UserProjectModelMapper = new UserProjectModelMapper();

        UnitOfWorkFactory = new UnitOfWorkFactory(DbContextFactory);
    }

    protected IDbContextFactory<Time2PlanDbContext> DbContextFactory { get; }

    protected ActivityEntityMapper ActivityEntityMapper { get; }
    protected ProjectEntityMapper ProjectEntityMapper { get; }
    protected ProjectUserRelationMapper ProjectUserRelationMapper { get; }
    protected UserEntityMapper UserEntityMapper { get; }

    protected IActivityModelMapper ActivityModelMapper { get; }
    protected IProjectModelMapper ProjectModelMapper { get; }
    protected IUserModelMapper UserModelMapper { get; }
    protected UserProjectModelMapper UserProjectModelMapper { get; }

    protected UnitOfWorkFactory UnitOfWorkFactory { get; }

    public async Task InitializeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeletedAsync();
        await dbx.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeletedAsync();
    }

}