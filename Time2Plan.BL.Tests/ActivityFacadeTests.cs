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
using Time2Plan.BL.Tests;
using Time2Plan.BL.Facades.Interfaces;

namespace Time2Plan.BL.Tests;

public class ActivityFacadeTests : FacadeTestBase
{
    private readonly IActivityFacade _facadeTest;
    public ActivityFacadeTests(ITestOutputHelper output) : base(output)
    {
        _facadeTest = new ActivityFacade(UnitOfWorkFactory, ActivityModelMapper);
    }

    [Fact]
    public async Task Create_new_activity()
    {
        //Arrange
        var model = new ActivityDetailModel()
        {
            Start = DateTime.Now.AddDays(-7),
            End = DateTime.Now,
            Type = "test type of activity"
        };
        //Act
        //Assert
    }
}