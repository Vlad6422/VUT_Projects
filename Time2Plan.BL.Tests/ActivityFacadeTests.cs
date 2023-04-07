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

public class ActivityFacadeTests : FacadeTestBase
{
    private readonly IActivityFacade facadeTest;
    public ActivityFacadeTests(ITestOutputHelper output) : base(output)
    {
        facadeTest = new ActivityFacade();//
    }

    [Fact]
    public async Task Create_new_activity()
    {
        //Arrange
        var model = new ActivityDetailModel()
        {
            DateTime = "";
        //Act
        //Assert
    }
}