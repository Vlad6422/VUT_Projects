namespace Time2Plan.BL.Models;

public record UserProjectListModel : ModelBase
{
    public Guid UserId { get; set; }
    public Guid ProjectId { get; set; }
    public string? Surname { get; set; }

    public string? Nickname { get; set; }
    public string? UserName { get; set; }
    public string? Photo { get; set; }
    public string? ProjectName { get; set; }


    public static UserProjectListModel Empty => new()
    {
        Id = Guid.Empty,
        UserId = Guid.Empty,
        ProjectId = Guid.Empty,
        UserName = string.Empty,
        ProjectName = string.Empty,
        Photo = string.Empty,
        Nickname = string.Empty,
        Surname = string.Empty,
    };
}
