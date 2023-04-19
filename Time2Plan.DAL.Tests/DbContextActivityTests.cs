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
}