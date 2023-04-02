namespace Time2Plan.BL.Models;

public record UserProjectDetailModel : ModelBase
{
    public required Guid UserId { get; set; }
    public UserDetailModel? User { get; init; }
    public required Guid ProjectId { get; set; }
    public ProjectDetailModel? Project { get; init; }

    public static UserProjectDetailModel Empty => new()
    {
        Id = Guid.Empty,
        UserId = Guid.Empty,
        ProjectId = Guid.Empty
    };
}
