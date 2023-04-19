namespace Time2Plan.DAL.Entities;

public record ActivityEntity : IEntity
{
    public required Guid Id { get; set; }
    public required DateTime Start { get; set; }
    public required DateTime End { get; set; }
    public string? Type { get; set; }
    public string? Tag { get; set; }
    public string? Description { get; set; }
    public ProjectEntity? Project { get; set; }
    public UserEntity? User { get; set; }
}
