using Microsoft.EntityFrameworkCore;
using Time2Plan.BL.Facades;
using Time2Plan.BL.Models;
using Time2Plan.Common.Tests;
using Time2Plan.Common.Tests.Seeds;
using Xunit;
using Xunit.Abstractions;

namespace Time2Plan.BL.Tests;

public class ActivityFacadeTests : FacadeTestBase
{
    private readonly IActivityFacade _activityFacadeSUT;
    public ActivityFacadeTests(ITestOutputHelper output) : base(output)
    {
        _activityFacadeSUT = new ActivityFacade(UnitOfWorkFactory, ActivityModelMapper);
    }

    [Fact]
    public async Task Create_New_Activity()
    {
        var activity = new ActivityDetailModel()
        {
            Start = new DateTime(2001, 1, 1, 8, 0, 0),
            End = new DateTime(2002, 1, 1, 10, 0, 0),
            Type = "test type of activity",
            UserId = UserSeeds.UserEntity3.Id,
            ProjectId = ProjectSeeds.ProjectBeta.Id
        };

        var savedActivity = await _activityFacadeSUT.SaveAsync(activity);
        activity.Id = savedActivity.Id;

        //Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var activityFromDb = await dbxAssert.Activities.SingleAsync(i => i.Id == activity.Id);

        DeepAssert.Equal(activity, ActivityModelMapper.MapToDetailModel(activityFromDb));
    }

    [Fact]
    public async Task GetAll_Single_SeededCode()
    {
        var activities = await _activityFacadeSUT.GetAsync();
        var activity = activities.Single(i => i.Id == ActivitySeeds.Code.Id);

        DeepAssert.Equal(ActivityModelMapper.MapToListModel(ActivitySeeds.Code), activity);
    }

    [Fact]
    public async Task GetFilterAsync()
    {
        var userId = ActivitySeeds.Code.UserId;
        var projectId = ActivitySeeds.Code.ProjectId;
        var fromDate = new DateTime(2000, 1, 1, 15, 30, 0);
        var toDate = new DateTime(2022, 12, 30);
        var tag = ActivitySeeds.Code.Tag;

        var activities = await _activityFacadeSUT.GetAsyncFilter(userId, fromDate, toDate, tag, projectId);
        var activity = activities.Single(a => a.Id == ActivitySeeds.Code.Id);

        DeepAssert.Equal(ActivityModelMapper.MapToListModel(ActivitySeeds.Code), activity);
    }

    [Fact]
    public async Task GetFilterAsyncTagOnly()
    {
        string tag = ActivitySeeds.Run.Tag!;
        var userId = ActivitySeeds.Code.UserId;
        var activities = await _activityFacadeSUT.GetAsyncFilter(userId, null, null, tag, null);
        var activity = activities.Single(a => a.Id == ActivitySeeds.Run.Id);

        DeepAssert.Equal(ActivityModelMapper.MapToListModel(ActivitySeeds.Run), activity);
    }

    [Fact]
    public async Task GetFilterAsyncProjectOnly()
    {
        Guid projectId = ActivitySeeds.Run.ProjectId;
        var userId = ActivitySeeds.Code.UserId;
        var activities = await _activityFacadeSUT.GetAsyncFilter(userId, null, null, null, projectId);
        var activity = activities.Single(a => a.Id == ActivitySeeds.Run.Id);

        DeepAssert.Equal(ActivityModelMapper.MapToListModel(ActivitySeeds.Run), activity);
    }

    [Fact]
    public async Task GetFilterAsyncDatesAll()
    {
        var fromDate = new DateTime(2000, 1, 1, 15, 30, 0);
        var toDate = new DateTime(2022, 12, 30);
        var tag = ActivitySeeds.Code.Tag;
        var userId = ActivitySeeds.Code.UserId;
        var activities = await _activityFacadeSUT.GetAsyncFilter(userId, fromDate, toDate, tag, ActivitySeeds.Code.ProjectId);
        var activity = activities.Single(a => a.Id == ActivitySeeds.Code.Id);

        DeepAssert.Equal(ActivityModelMapper.MapToListModel(ActivitySeeds.Code), activity);
    }

    [Fact]
    public async Task GetById_SeededCode()
    {
        var activity = await _activityFacadeSUT.GetAsync(ActivitySeeds.Code.Id);

        DeepAssert.Equal(ActivityModelMapper.MapToDetailModel(ActivitySeeds.Code), activity);
    }

    [Fact]
    public async Task GetById_NonExistent()
    {
        var activity = await _activityFacadeSUT.GetAsync(ActivitySeeds.EmptyActivity.Id);

        Assert.Null(activity);
    }

    [Fact]
    public async Task Delete_activity()
    {
        await _activityFacadeSUT.DeleteAsync(ActivitySeeds.Run.Id);
    }


    [Fact]
    public async Task SeededCode_InsertOrUpdate_ActivityUpdated()
    {
        var activity = new ActivityDetailModel()
        {
            Id = ActivitySeeds.Code.Id,
            Description = ActivitySeeds.Code.Description,
            Start = ActivitySeeds.Code.Start,
            End = ActivitySeeds.Code.End,
            UserId = UserSeeds.User1.Id,
            ProjectId = ProjectSeeds.ProjectBeta.Id,
        };
        activity.Description += "updated";

        await _activityFacadeSUT.SaveAsync(activity);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var activityFromDb = await dbxAssert.Activities.SingleAsync(i => i.Id == activity.Id);
        DeepAssert.Equal(activity, ActivityModelMapper.MapToDetailModel(activityFromDb));
    }
}