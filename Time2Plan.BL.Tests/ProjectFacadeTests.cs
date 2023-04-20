using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Time2Plan.BL.Facades;
using Time2Plan.BL.Facades.Interfaces;
using Time2Plan.BL.Mappers;
using Time2Plan.BL.Models;
using Time2Plan.Common.Tests;
using Time2Plan.Common.Tests.Seeds;
using Time2Plan.DAL.Entities;
using Xunit;
using Xunit.Abstractions;

namespace Time2Plan.BL.Tests;

public sealed class ProjectFacadeTest : FacadeTestBase
{
    private readonly IProjectFacade _projectFasadeSUT;
    public ProjectFacadeTest(ITestOutputHelper output) : base(output)
    {
        _projectFasadeSUT = new ProjectFasade(UnitOfWorkFactory, ProjectModelMapper);
    }

    [Fact]
    public async Task Create_New_Project()
    {
        //Arrange
        var project = new ProjectDetailModel()
        {
            Name = "Project 1",
            Description = "Description test 1",
        };

        //Act
        project = await _projectFasadeSUT.SaveAsync(project);

        //Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var projectFromDb = await dbxAssert.Projects.SingleAsync(i => i.Id == project.Id);
        DeepAssert.Equal(project, ProjectModelMapper.MapToDetailModel(projectFromDb));
    }

    [Fact]
    public async Task GetById_Single_ProjectAlpha()
    {
        var projects = await _projectFasadeSUT.GetAsync();
        var project = projects.Single(i => i.Id == ProjectSeeds.ProjectAlpha.Id);
        DeepAssert.Equal(ProjectModelMapper.MapToListModel(ProjectSeeds.ProjectAlpha), project);
    }

    [Fact]
    public async Task GetById_ProjectAlpha()
    {
        var project = await _projectFasadeSUT.GetAsync(ProjectSeeds.ProjectAlpha.Id);
        DeepAssert.Equal(ProjectModelMapper.MapToDetailModel(ProjectSeeds.ProjectAlpha), project);
    }

    [Fact]
    public async Task ProjectAlpha_DeleteById_Deleted()
    {
        var projectId = ProjectSeeds.ProjectAlphaDelete.Id;
        await _projectFasadeSUT.DeleteAsync(ProjectSeeds.ProjectAlphaDelete.Id);
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        Assert.False(await dbxAssert.Projects.AnyAsync<ProjectEntity>(i => i.Id == projectId));
    }

    [Fact]
    public async Task Delete_Project_With_Activities()
    {
        var projectId = ProjectSeeds.ProjectWithActivitiesDelete.Id;
        await _projectFasadeSUT.DeleteAsync(ProjectSeeds.ProjectWithActivitiesDelete.Id);


        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        Assert.False(await dbxAssert.Projects.AnyAsync<ProjectEntity>(i => i.Id == projectId));

    }


    [Fact]
    public async Task NewProject_InsertOrUpdate_ProjectAdded()
    {
        //Arrange
        var project = new ProjectDetailModel()
        {
            Id = Guid.Empty,
            Name = "Project test 2",
            Description = "Description test 2",
        };

        //Act
        project = await _projectFasadeSUT.SaveAsync(project);

        //Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var projectFromDb = await dbxAssert.Projects.SingleAsync(i => i.Id == project.Id);
        DeepAssert.Equal(project, ProjectModelMapper.MapToDetailModel(projectFromDb));
    }

    [Fact]
    public async Task ProjectAlpha_InsertOrUpdate_ProjectUpdated()
    {
        //Arrange
        var project = new ProjectDetailModel()
        {
            Id = ProjectSeeds.ProjectAlpha.Id,
            Name = ProjectSeeds.ProjectAlpha.Name,
            Description = ProjectSeeds.ProjectAlpha.Description,
        };
        project.Name += "updated";
        project.Description += "updated";

        //Act
        project = await _projectFasadeSUT.SaveAsync(project);

        //Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var projectFromDb = await dbxAssert.Projects.SingleAsync(i => i.Id == project.Id);
        DeepAssert.Equal(project, ProjectModelMapper.MapToDetailModel(projectFromDb));
    }
}