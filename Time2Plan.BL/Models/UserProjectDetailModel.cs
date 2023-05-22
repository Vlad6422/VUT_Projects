namespace Time2Plan.BL.Models;

public record UserProjectDetailModel : ModelBase
{
    public required Guid UserId { get; set; }
    public string? UserName { get; set; }
    public string? UserSurname { get; set; }
    public string? UserNickname { get; set; }
    public required Guid ProjectId { get; set; }
    public string? ProjectName { get; set; }

    public string? Photo { get; set; }

    public static UserProjectDetailModel Empty => new()
    {
        Id = Guid.Empty,
        UserId = Guid.Empty,
        ProjectId = Guid.Empty,
        UserName = string.Empty,
        ProjectName = string.Empty,
        Photo = string.Empty,
        UserNickname = string.Empty,
        UserSurname = string.Empty,
    };
}
