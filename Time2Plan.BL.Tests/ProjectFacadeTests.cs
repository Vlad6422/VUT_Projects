using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Time2Plan.BL.Tests;

public sealed class ProjectFacadeTest : FacadeTestBase
{
    private readonly IProjectFacade facadeTest;
    public ProjectFacadeTests(ITestOutputHelper output) : base(output)
    {
        userTest = new ProjectFacade(unitOfWorkFactory, modelMapper);
    }

    [Fact]
    public async 
}