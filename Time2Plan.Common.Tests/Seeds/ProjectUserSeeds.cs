using Time2Plan.DAL.Interfaces;

namespace Time2Plan.Common.Tests.Seeds;

internal class ProjectUserSeeds
{
    public static readonly ProjectUserRelation EmtpyRelation = new()
    {
        Id = default,
        Project = default,
        User = default,
        UserId = default,
        ProjectId = default,
    };

    public static readonly ProjectUserRelation ProjectAlphaUser1 = new()
    {
        Id = Guid.NewGuid(),
        Project = ProjectSeeds.ProjectAlpha,
        User = UserSeeds.User1,
        UserId = UserSeeds.User1.Id,
        ProjectId = ProjectSeeds.ProjectAlpha.Id,
    };


}
