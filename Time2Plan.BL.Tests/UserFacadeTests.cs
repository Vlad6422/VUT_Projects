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
using Xunit;
using Xunit.Abstractions;

namespace Time2Plan.BL.Tests;

public class UserFacadeTests : FacadeTestBase
{
    private readonly IUserFacade userTest;
    public UserFacadeTests(ITestOutputHelper output) : base(output)
    {
        userTest = new UserFacade(unitOfWorkFactory, modelMapper);
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
        var returnedModel = await userTest.CreateAsync(model);
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
                    Name = "TestAvtivity1"
                    //DateTime
                    //DateTime
                }
            }
        }
        //Act and assert
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => userTest.SaveAsync(model))

    }
    [Fact]
    public async Task GetById_FromSeeded_DoesNotThrowAndEqualsSeeded()
    {
        //Arrange
        var model = new UserDetailModel()
        //Act
        //Assert
    }
    private static void FixIds(UserDetailModel expectedModel, UserDetailModel returnedModel)
    {
        returnedModel.Id = expectedModel.Id;

        foreach (var UserListModel in returnedModel.Ingredients)
        {
            var ActivityDetailModel = expectedModel.Activities.FirstOrDefault(i =>
                i.User.Name == UserListMOdel.Name
                && i.Unit == ActivityModel.Unit);

            if (ingredientAmountDetailModel != null)
            {
                UserListModel.Id = UserDetailModel.Id;
                UserListModel.Activity.Id = UserDetailModel.Activity.Id;
            }
        }
    }
}
