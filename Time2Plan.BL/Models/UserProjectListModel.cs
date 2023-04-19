namespace Time2Plan.BL.Models;

public record UserProjectListModel : ModelBase
{
    public Guid UserId { get; set; }
    public Guid ProjectId { get; set; }


    public static UserProjectListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        ProjectId = Guid.NewGuid(),
        UserId = Guid.NewGuid()
    };
}
