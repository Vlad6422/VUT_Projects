using System;

namespace Time2Plan.DAL.Interfaces;

public record ProjectEntity : IEntity
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string ?Description { get; set; }

    public ICollection<ActivityEntity> Activities { get; init; } = new List<ActivityEntity>();
    //public ICollection<UserEntity> Users { get; init; } = new List<UserEntity>();
    public ICollection<ProjectUserRelation> UserProjects { get; init; } = new List<ProjectUserRelation>();
}
