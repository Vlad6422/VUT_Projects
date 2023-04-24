using Microsoft.EntityFrameworkCore;
using Time2Plan.Common.Tests.Seeds;
using Time2Plan.Common.Tests;
using Xunit;
using Xunit.Abstractions;

namespace Time2Plan.DAL.Tests;

public class DbContextProjectTests : DbContextTestsBase
{
    public DbContextProjectTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task AddNew_Project_DefaultSeed()
    {
        // arrange
        var entity = ProjectSeeds.EmptyProject with
        {
            Name = "Project test 1",
            Description = "Mix of all",
        };

        // Act
        Time2PlanDbContextSUT.Projects.Add(entity);
        await Time2PlanDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Projects.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task GetById_Project_AlphaRetrieved()
    {
        //Act
        var entity = await Time2PlanDbContextSUT.Projects.SingleAsync(i => i.Id == ProjectSeeds.ProjectAlpha.Id);

        //Assert
        DeepAssert.Equal(ProjectSeeds.ProjectAlpha, entity);
    }

    [Fact]
    public async Task Update_Project_Persisted()
    {
        //Arrange
        var baseEntity = ProjectSeeds.ProjectAlphaUpdate;
        var entity =
            baseEntity with
            {
                Description = baseEntity.Description + "Update",
            };

        //Act
        Time2PlanDbContextSUT.Projects.Update(entity);
        await Time2PlanDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Projects.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task Delete_Project_AlphaDelte()
    {
        //Arrange
        var entityBase = ProjectSeeds.ProjectAlphaDelete;

        //Act
        Time2PlanDbContextSUT.Projects.Remove(entityBase);
        await Time2PlanDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await Time2PlanDbContextSUT.Projects.AnyAsync(i => i.Id == entityBase.Id));
    }

    [Fact]
    public async Task DeleteById_Activity_CodeDelete()
    {
        //Arrange
        var entityBase = ProjectSeeds.ProjectAlphaDelete;

        //Act
        Time2PlanDbContextSUT.Remove(
            Time2PlanDbContextSUT.Projects.Single(i => i.Id == entityBase.Id));
        await Time2PlanDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await Time2PlanDbContextSUT.Projects.AnyAsync(i => i.Id == entityBase.Id));
    }
}