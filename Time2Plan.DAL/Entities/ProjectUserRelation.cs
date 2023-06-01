namespace Time2Plan.DAL.Entities;

public record ProjectUserRelation : IEntity
{
    public required Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public required Guid ProjectId { get; set; }
    public UserEntity? User { get; set; }
    public ProjectEntity? Project { get; set; }
}
