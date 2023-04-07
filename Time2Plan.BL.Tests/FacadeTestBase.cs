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

namespace Time3Plan.BL.Tests;

public class FacadeTestBase : IAsyncLifetime
{
    protected FacadeTestBase(ITestOutputHelper output)
    {
        XunitTestOutputConverter converter = new(output);
        Console.SetOut(converter);

        //DbContextFactory = new DbContextTeestingInMemoryFactory(GetType().Name, seedTestingData: true);
        DbContextFactory = new DbContexLocalDBTestingFactory(GetType().FullName!, seedTestingData: true);
        //DbContextFactory = new DbContextSQLiteTestingFactory.cs(GetType().FullName!, seedTestingData: true);

        ActivityEntityMapper = new ActivityEntityMapper();
        ProjectEntityMapper = new ProjectEntityMapper();
        ProjectUserRelationMapper = new ProjectUserRelationMapper();
        UserEntityMapper = new UserEntityMApper();

        ActivityModelMapper = new ActivityModelMapper();
        ProjectModelMapper = new ProjectModelMapper();
        UserModelMapper = new UserModelMapper();
        UserProjectModelMapper = new UserProjectModelMapper();

        UnitOfWorkFactory = new UnitOfWorkFactory(DbContextFactory);
    }

    protected IDbContextFactory<CookBookDbContext> DbContextFactory { get; }

    protected ActivityEntityMapper ActivityEntityMapper { get; }
    protected ProjectEntityMapper ProjectEntityMapper { get; }
    protected ProjectUserRelationMapper ProjectUserRelationMapper { get; }
    protected UserEntityMapper UserEntityMapper { get; }

    protected IActivityModelMapper ActivityModelMapper { get; }
    protected ProjectModelMapper ProjectModelMapper { get; }
    protected UserModelMapper UserModelMapper { get; }
    protected UserProjectModelMapper UserProjectModelMapper { get; }

    public async Task InitializeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeleteAsync();
        await dbx.Database.EnsureCreateAsync();
    }

    public async Task DisposeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeleteAsync();
    }

}