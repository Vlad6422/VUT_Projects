using Time2Plan.BL.Facades;
using Time2Plan.BL.Models;
using Time2Plan.Common.Tests;
using Time2Plan.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit;
using Time2Plan.BL.Tests;
using Time2Plan.BL.Facades.Interfaces;

namespace Time2Plan.BL.Tests;

public class ActivityFacadeTests : FacadeTestBase
{
    private readonly IActivityFacade _activityTest;
    public ActivityFacadeTests(ITestOutputHelper output) : base(output)
    {
        _activityTest = new ActivityFacade(UnitOfWorkFactory, ActivityModelMapper);
    }

    [Fact]
    public async Task Create_new_activity()
    {
        var model = new ActivityDetailModel()
        {
            Start = DateTime.Now.AddDays(-7),
            End = DateTime.Now,
            Type = "test type of activity"
        };
        var _ = await _activityTest.SaveAsync(model);
    }

    [Fact]
    public async Task GetAll_Single_SeededCode()
    {
        var activities = await _activityTest.GetAsync();
        var activity = activities.Single(i => i.Id == ActivitySeeds.Code.Id);

        DeepAssert.Equal(ActivityModelMapper.MapToListModel(ActivitySeeds.Code), activity);
    }

    [Fact]
    public async Task GetById_SeededCode()
    {
        var activity = await _activityTest.GetAsync(ActivitySeeds.Code.Id);

        DeepAssert.Equal(ActivityModelMapper.MapToDetailModel(ActivitySeeds.Code), activity);
    }

    [Fact]
    public async Task GetById_NonExistent()
    {
        var activity = await _activityTest.GetAsync(ActivitySeeds.EmptyActivity.Id);

        Assert.Null(activity);
    }

    [Fact]
    public async Task Delete_activity()
    {
        await _activityTest.DeleteAsync(ActivitySeeds.Run.Id);
    }


    [Fact]
    public async Task SeededCode_InsertOrUpdate_ActivityUpdated()
    {
        var activity = new ActivityDetailModel()
        {
            Id = ActivitySeeds.Code.Id,
            Description = ActivitySeeds.Code.Description,
            Start = ActivitySeeds.Code.Start,
            End = ActivitySeeds.Code.End
        };
        activity.Description += "updated";

        await _activityTest.SaveAsync(activity);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var activityFromDb = await dbxAssert.Activities.SingleAsync(i => i.Id == activity.Id);
        DeepAssert.Equal(activity, ActivityModelMapper.MapToDetailModel(activityFromDb));
    }
}