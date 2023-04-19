using Microsoft.EntityFrameworkCore;
using Time2Plan.Common.Tests;
using Time2Plan.Common.Tests.Seeds;
using Time2Plan.DAL.Interfaces;
using Xunit;
using Xunit.Abstractions;

namespace Time2Plan.DAL.Tests;

public class DbContextUserTests : DbContextTestsBase
{
    public DbContextUserTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task AddNew_User_DefaultSeed()
    {
        // arrange
        var entity = UserSeeds.EmptyUser with
        {
            Name = "Evzen",
            Surname = "Nevidoma",
            NickName = "xevzen007"
        };

        // Act
        Time2PlanDbContextSUT.Users.Add(entity);
        await Time2PlanDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Users.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task AddNew_User_Persisted()
    {
        // arrange
        UserEntity entity = new()
        {
            Id = Guid.Parse(input: "6b38d1a7-e5a4-4188-9bd0-cb9f8ba4a24c"),
            Name = "Lois",
            Surname = "Cherry",
            NickName = "LoliCher89",
            ShowNickName = false,
            Photo = "https://cdn-icons-png.flaticon.com/512/219/219983.png"
        };

        // Act
        Time2PlanDbContextSUT.Users.Add(entity);
        await Time2PlanDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Users.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task GetById_User_User1Retrieved()
    {
        //Act
        var entity = await Time2PlanDbContextSUT.Users.SingleAsync(i => i.Id == UserSeeds.User1.Id);

        //Assert
        DeepAssert.Equal(UserSeeds.User1, entity);
    }

    [Fact]
    public async Task Update_User_Persisted()
    {
        //Arrange
        var baseEntity = UserSeeds.User1Update;
        var entity =
            baseEntity with
            {
                Surname = baseEntity.Surname + "Update",
                Photo = "https://www.freeimages.com/photo/security-camera-1-1217965"
            };

        //Act
        Time2PlanDbContextSUT.Users.Update(entity);
        await Time2PlanDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Users.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task Delete_User_User1Delete()
    {
        //Arrange
        var entityBase = UserSeeds.User1Delete;

        //Act
        Time2PlanDbContextSUT.Users.Remove(entityBase);
        await Time2PlanDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await Time2PlanDbContextSUT.Users.AnyAsync(i => i.Id == entityBase.Id));
    }

    [Fact]
    public async Task DeleteById_User_User1Delete()
    {
        //Arrange
        var entityBase = UserSeeds.User1Delete;

        //Act
        Time2PlanDbContextSUT.Remove(
            Time2PlanDbContextSUT.Users.Single(i => i.Id == entityBase.Id));
        await Time2PlanDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await Time2PlanDbContextSUT.Users.AnyAsync(i => i.Id == entityBase.Id));
    }
}