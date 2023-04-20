using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using Time2Plan.BL.Facades;
using Time2Plan.BL.Facades.Interfaces;
using Time2Plan.BL.Models;
using Time2Plan.Common.Tests;
using Time2Plan.Common.Tests.Seeds;
using Xunit;
using Xunit.Abstractions;

namespace Time2Plan.BL.Tests;

public class UserFacadeTests : FacadeTestBase
{
    private readonly IUserFacade _userFacadeSUT;
    public UserFacadeTests(ITestOutputHelper output) : base(output)
    {
        _userFacadeSUT = new UserFasade(UnitOfWorkFactory, UserModelMapper);
    }
    [Fact]
    public async Task CreateNewUser()
    {
        var user = new UserDetailModel()
        {
            Name = "Patrick",
            Surname = "Bateman",
            NickName = "Trickman"
        };
        user = await _userFacadeSUT.SaveAsync(user);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var userFromDb = await dbxAssert.Users.SingleAsync(i => i.Id == user.Id);
        DeepAssert.Equal(user, UserModelMapper.MapToDetailModel(userFromDb));
    }
    [Fact]
    public async Task Create_user_with_nonexisting_activity()
    {
        //Arrange                                      
        var model = new UserDetailModel()
        {
            Id = Guid.Empty,
            Name = "User1",
            Surname = "Surname1",
            NickName = "NickName1",
            Activities = new ObservableCollection<ActivityListModel>()
            {
                new()
                {
                    Id = Guid.Empty,
                    Type = "TestType1",
                    Start = DateTime.Now.AddDays(-7),
                    End = DateTime.Now
                }
            }
        };
        //Act, assert
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _userFacadeSUT.SaveAsync(model));
    }

    [Fact]
    public async Task Create_WithActivities_DoesNotThrowAndEqualsCreated()
    {
        //Arrange
        var model = new UserDetailModel()
        {
            Name = "User2",
            Surname = "Surname2",
            NickName = "NickName2",
            Photo = "--",
            Activities = new ObservableCollection<ActivityListModel>()
            {
                new()
                {
                    Id = ActivitySeeds.Code.Id,
                    Start = ActivitySeeds.Code.Start,
                    End = ActivitySeeds.Code.End,
                    Type = ActivitySeeds.Code.Type,
                    Tag = ActivitySeeds.Code.Tag,
                }
            },
        };
        //Act && Assert
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _userFacadeSUT.SaveAsync(model));
    }

    [Fact]
    public async Task Create_WithExistingAndNotExistingActivities_Throws()
    {
        //Arrange
        var model = new UserDetailModel()
        {
            Name = "User2",
            Surname = "Surname2",
            NickName = "NickName2",
            Activities = new ObservableCollection<ActivityListModel>()
            {
                new ()
                {
                    Id = Guid.Empty,
                    Start = ActivitySeeds.Code.Start,
                    End = ActivitySeeds.Code.End,
                    Type = ActivitySeeds.Code.Type,
                    Tag = ActivitySeeds.Code.Tag,
                },
                ActivityModelMapper.MapToListModel(ActivitySeeds.Run),
            },
        };
        //Act & Assert
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _userFacadeSUT.SaveAsync(model));
    }

    [Fact]
    public async Task GetById_FromSeeded_DoesNotThrowAndEqualsSeeded()
    {
        //Act
        var user = await _userFacadeSUT.GetAsync(UserSeeds.User1.Id);
        //Assert
        DeepAssert.Equal(UserModelMapper.MapToDetailModel(UserSeeds.User1), user);
    }

    [Fact]
    public async Task GetAll_FromSeeded_DoesNotThrowAndContainsSeeded()
    {
        //Arrange
        var listModel = UserModelMapper.MapToListModel(UserSeeds.User1);
        //Act
        var returnedModel = await _userFacadeSUT.GetAsync();
        //Assert
        Assert.Contains(listModel, returnedModel);
    }

    [Fact]
    public async Task Update_FromSeeded_DoesNotThrow()
    {
        //Arrange
        var detailModel = UserModelMapper.MapToDetailModel(UserSeeds.User1Update);
        detailModel.Name = "Changed user name";
        //Act & Assert
        await _userFacadeSUT.SaveAsync(detailModel with { Activities = default });
    }

    [Fact]
    public async Task Update_Name_FromSeeded_CheckUpdated()
    {
        //Arrange
        var detailModel = UserModelMapper.MapToDetailModel(UserSeeds.User1Update);
        detailModel.Name = "Changed user name 1";
        //Act
        await _userFacadeSUT.SaveAsync(detailModel with { Activities = default });
        //Assert
        var returnedModel = await _userFacadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equal(detailModel, returnedModel);
    }

    [Fact]
    public async Task DeleteById_FromSeeded_DoesNotThrow()
    {
        //Arrange, act, assert
        await _userFacadeSUT.DeleteAsync(UserSeeds.User1Delete.Id);
    }
}
