namespace Time2Plan.BL.Models;

public record UserProjectDetailModel : ModelBase
{
    public required Guid UserId { get; set; }
    //public UserDetailModel? User { get; init; }
    public string? UserName { get; set; }
    public required Guid ProjectId { get; set; }
    //public ProjectDetailModel? Project { get; init; }
    public string? ProjectName { get; set; }

    public static UserProjectDetailModel Empty => new()
    {
        Id = Guid.Empty,
        UserId = Guid.Empty,
        ProjectId = Guid.Empty,
        UserName = string.Empty,
        ProjectName = string.Empty
    };
}
