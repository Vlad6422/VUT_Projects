using System;

namespace Time2Plan.DAL.Interfaces
{
    public record UserEntity : IEntity
    { 
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string NickName { get; set; }
        public bool ?ShowNickName { get; set; }
        public string ?Photo { get; set; }
        public ICollection<ActivityEntity> Activities { get; init; } = new List<ActivityEntity>();
        public ICollection<ProjectUserRelation> UserProjects { get; init; } = new List<ProjectUserRelation>();
    }
}
