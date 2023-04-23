using Microsoft.EntityFrameworkCore;
using Time2Plan.Common.Tests;
using Time2Plan.Common.Tests.Seeds;
using Xunit;
using Xunit.Abstractions;

namespace Time2Plan.DAL.Tests;

public class DbContextActivityTests : DbContextTestsBase
{
    public DbContextActivityTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task AddNew_Activity_Persisted()
    {
        //Arrange
        var entity = ActivitySeeds.EmptyActivity with
        {
            End = DateTime.UtcNow,
            Start = new DateTime(2022, 3, 15)
        };

        //Act
        Time2PlanDbContextSUT.Activities.Add(entity);
        await Time2PlanDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Activities
            .SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task AddNew_Activity_DefaultSeed()
    {
        // arrange
        var entity = ActivitySeeds.EmptyActivity with
        {
            Start = new DateTime(2020, 8, 20, 6, 30, 0),
            End = new DateTime(2022, 8, 21, 10, 30, 0),
            Type = "Coding",
            Tag = "Work",
            Description = "Coding project to my job",
        };

        // Act
        Time2PlanDbContextSUT.Activities.Add(entity);
        await Time2PlanDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Activities.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task GetById_Activity_CodeRetrieved()
    {
        //Act
        var entity = await Time2PlanDbContextSUT.Activities.SingleAsync(i => i.Id == ActivitySeeds.Run.Id);

        //Assert
        DeepAssert.Equal(ActivitySeeds.Run, entity);
    }

    [Fact]
    public async Task Update_Activity_Persisted()
    {
        //Arrange
        var baseEntity = ActivitySeeds.CodeUpdate;
        var entity =
            baseEntity with
            {
                Type = baseEntity.Type + "Update",
                Description = baseEntity.Description + "Update",
            };

        //Act
        Time2PlanDbContextSUT.Activities.Update(entity);
        await Time2PlanDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Activities.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task Delete_Activity_CodeDelete()
    {
        //Arrange
        var entityBase = ActivitySeeds.CodeDelete;

        //Act
        Time2PlanDbContextSUT.Activities.Remove(entityBase);
        await Time2PlanDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await Time2PlanDbContextSUT.Activities.AnyAsync(i => i.Id == entityBase.Id));
    }

    [Fact]
    public async Task DeleteById_Activity_CodeDelete()
    {
        //Arrange
        var entityBase = ActivitySeeds.CodeDelete;

        //Act
        Time2PlanDbContextSUT.Remove(
            Time2PlanDbContextSUT.Activities.Single(i => i.Id == entityBase.Id));
        await Time2PlanDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await Time2PlanDbContextSUT.Activities.AnyAsync(i => i.Id == entityBase.Id));
    }
}