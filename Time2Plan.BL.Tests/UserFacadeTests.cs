using Time2Plan.BL.Facades;
using Time2Plan.BL.Models;
using Time2Plan.Common.Enums;
using Time2Plan.Common.Tests;
using Time2Plan.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using XUnit;
using XUnit.Abstractions;

namespace Time2Plan.BL.Tests;

public class UserFacadeTests : FacadeTestBase
{
    private readonly IUserFacade _userTest;
    public UserFacadeTests(ITestOutputHelper output) : base(output)
    {
        _userTest = new UserFacade(unitOfWorkFactory, modelMapper);
    }

    [Fact]
    public async Task Create_new_user_without_activities()
    {
        //Arrange
        var model = new UserDetailModel()
        {
            Id = Guid.Empty,
            Name = "TestName1",
            Surname = "TestSurname1",
            NickName = "TestNickName1"
        };
        //Act
        var returnedModel = await _userTest.CreateAsync(model);
        //Assert
        FixIds(mode1, returnedModel);
        DeepAssert.Equal(mode1, returnedModel);
    }

    [Fact]
    public async Task Create_new_user_with_activity()
    {
        //Arrange                                      
        var model = new UserDetailModel()
        {
            Id = Guid.Empty,
            Name = "TestName2",
            Surname = "TestSurname2",
            NickName = "Testnickname",
            Activities = new ObservableCollection<ActivityEntity>()
            {
                new()
                {
                    Id = Guid.Empty,
                    Type = "TestType1",
                    Name = "TestAvtivity1",
                    Start = DateTime.Now.AddDays(-7),
                    End = DateTime.Now
                }
            }
        }
        //Act, assert
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _userTest.SaveAsync(model))
    }

    [Fact]
    public async Task GetById_FromSeeded_DoesNotThrowAndEqualsSeeded()
    {
        //Arrange
        var model = new UserDetailModel()
        //Act
        var returnModel = await _userTest.GetAsync(detailModel.Id);
        //Assert
        DeepAssert.Equal(detailModel, returnModel);
    }

    [Fact]
    public async Task GetAll_FromSeeded_DoesNotThrowAndContainsSeeded()
    {
        //Arrange
        var listModel = UserModelMapper.MapToListModel(UserSeeds.UserEntity);
        //Act
        var returnedModel = await _userTest.GetAsync();
        //Assert
        Assert.Contains(listModel, returnedModel);
    }

    [Fact]
    public async Task Update_FromSeeded_DoesNotThrow()
    {
        //Arrange
        var detailModel = UserModelMapper.MapToDetailModel(UserSeeds.UserEntity);
        detailModel.Name = "Changed user name";
        //Act & Assert
        await _userTest.SaveAsync(detailModel with { Activities = default });
    }

    [Fact]
    public async Task Update_Name_FromSeeded_CheckUpdated()
    {
        //Arrange
        var detailModel = UserModelMapper.MapToDetailModel(UserSeeds.UserEntity);
        detailModel.Name = "Changed user name 1";
        //Act
        await _userTest.SaveAsync(detailModel with { Actvities = default });
        //Assert
        var returnedModel = await _userTest.GetAsync(detailModel.Id);
        DeepAssert.Equal(detailModel, returnedModel);
    }

    [Fact] 
    public async Task Update_RemoveOneOfActivities_FromSeeded_CheckUpdated()
    {
        //Arrange
        var detailModel = UserModelMapper.MapToDetailModel(UserSeeds.UserEntity);
        detailModel.Activity.Remove(detailModel.Activity.First());
        //Act
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _userTest.SaveAsync(detailModel));
        //Assert
        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equal(UserModelMapper.MapToDetailModel(UserSeeds.UserEntity), returnedModel);
    }

    [Fact]
    public async Task DeleteById_FromSeeded_DoesNotThrow()
    {
        //Arrange, act, assert
        await _userTest.DeleteAsync(UserSeeds.UserEntity.Id);
    }

    private static void FixIds(UserDetailModel expectedModel, UserDetailModel returnedModel)
    {
        returnedModel.Id = expectedModel.Id;

        foreach (var UserListModel in returnedModel.Activity) //?
        {
            var ActivityDetailModel = expectedModel.Activities.FirstOrDefault(i =>
                i.User.Name == UserListModel.Name && i.Unit == ActivityModel.Unit);

            if (ingredientAmountDetailModel != null)
            {
                UserListModel.Id = UserDetailModel.Id;
                UserListModel.Activity.Id = UserDetailModel.Activity.Id;
            }
        }
    }
}
