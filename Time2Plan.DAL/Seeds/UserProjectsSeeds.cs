using Microsoft.EntityFrameworkCore;
using Time2Plan.DAL.Entities;

namespace Time2Plan.DAL.Seeds;

public static class UserProjectSeeds
{
    public static readonly ProjectUserRelation StepanBeta = new()
    {
        Id = Guid.NewGuid(),
        UserId = UserSeeds.StepanUser.Id,
        ProjectId = ProjectSeeds.ProjectBeta.Id
    };

    public static readonly ProjectUserRelation StepanAlpha = new()
    {
        Id = Guid.NewGuid(),
        UserId = UserSeeds.StepanUser.Id,
        ProjectId = ProjectSeeds.ProjectAlpha.Id
    };

    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<ProjectUserRelation>().HasData(
            StepanAlpha,
            StepanBeta
        );
}
