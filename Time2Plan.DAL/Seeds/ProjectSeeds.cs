using Microsoft.EntityFrameworkCore;
using Time2Plan.DAL.Entities;

namespace Time2Plan.DAL.Seeds;

public static class ProjectSeeds
{
    public static readonly ProjectEntity EmptyProject = new()
    {
        Id = default,
        Name = default!,
        Description = default,
    };

    public static readonly ProjectEntity ProjectAlpha = new()
    {
        Id = Guid.Parse("4ebe27b1-a851-4f00-a93b-edccad5587af"),
        Name = "Project Alpha",
        Description = "A software development project"
    };

    public static readonly ProjectEntity ProjectBeta = new()
    {
        Id = Guid.Parse("58084c98-638e-4294-b713-a36dac80c453"),
        Name = "Project Beta",
        Description = "A marketing campaign"
    };

    public static readonly ProjectEntity ProjectWithActivities = new()
    {
        Id = Guid.Parse("1030a9da-1592-4f6b-9e49-af4b59edc52c"),
        Name = "Project with activities",
        Description = "Coding and running"
    };


    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProjectEntity>().HasData(
            ProjectAlpha,
            ProjectBeta,
            ProjectWithActivities);
    }
}

