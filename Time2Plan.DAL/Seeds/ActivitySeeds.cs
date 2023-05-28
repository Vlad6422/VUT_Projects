using Microsoft.EntityFrameworkCore;
using Time2Plan.DAL.Entities;

namespace Time2Plan.DAL.Seeds;

public static class ActivitySeeds
{
    public static readonly ActivityEntity EmptyActivity = new()
    {
        Id = default,
        End = default,
        Start = default,
        Tag = default,
        Type = default,
        Description = default,
        UserId = default,
        ProjectId = default,
    };

    public static readonly ActivityEntity Code = new()
    {
        Id = Guid.NewGuid(),
        Start = new DateTime(2022, 1, 1, 8, 0, 0),
        End = new DateTime(2022, 1, 1, 10, 0, 0),
        Type = "Meeting",
        Tag = "Planning",
        Description = "Planning meeting with the team",
        UserId= UserSeeds.StepanUser.Id,
        ProjectId= ProjectSeeds.ProjectAlpha.Id,
    };

    public static readonly ActivityEntity Run = new()
    {
        Id = Guid.NewGuid(),
        Start = new DateTime(2022, 1, 2, 13, 0, 0),
        End = new DateTime(2022, 1, 2, 16, 0, 0),
        Type = "Running",
        Tag = "Sport",
        Description = "5 km run",
        UserId = UserSeeds.StepanUser.Id,
        ProjectId = ProjectSeeds.ProjectBeta.Id,
    };
    public static readonly ActivityEntity ThisYearActivity = new()
    {
        Id = Guid.NewGuid(),
        Start = new DateTime(2023, 1, 2, 13, 0, 0),
        End = new DateTime(2023, 1, 2, 16, 0, 0),
        Tag = "Activitying",
        UserId = UserSeeds.AnnaUser.Id,
        ProjectId = ProjectSeeds.ProjectWithActivities.Id,
    };

    public static readonly ActivityEntity ActivityWithProject = new()
    {
        Id = Guid.NewGuid(),
        Start = new DateTime(2022, 4, 5, 15, 30, 0),
        End = new DateTime(2022, 6, 4, 3, 0, 0),
        UserId = UserSeeds.StepanUser.Id,
        ProjectId = ProjectSeeds.ProjectBeta.Id,
    };
    public static readonly ActivityEntity CodeDelete = Code with { Id = Guid.NewGuid(), Type = "Delete"};
    public static readonly ActivityEntity CodeUpdate = Code with { Id = Guid.NewGuid(), Type = "Update"};

    public static readonly ActivityEntity ActivityViewTest = new()
    {
        Id = Guid.NewGuid(),
        Tag = "PC",
        Type = "PC gaming",
        Start = new DateTime(2030, 9, 9, 9, 30, 0),
        End = new DateTime(2031, 10, 10, 10, 31, 0),
        UserId = UserSeeds.PatrickUser.Id,
        ProjectId = ProjectSeeds.ProjectAlpha.Id,
    };


    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityEntity>().HasData(Code, Run, ThisYearActivity, CodeDelete, CodeUpdate, ActivityViewTest);
    }
}
