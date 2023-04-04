namespace Time2Plan.BL.Models;

public record ActivityDetailModel : ModelBase
{
    public required DateTime Start { get; set; }
    public required DateTime End { get; set; }
    public string? Type { get; set; }
    public string? Tag { get; set; }
    public string? Description { get; set; }

    // is necessary to reference back?

    //public ProjectDetailModel? Project { get; set; }
    //public UserDetailModel? User { get; set; }

    public static ActivityDetailModel Empty => new()
    {
        Id = Guid.Empty,
        Start = new DateTime(),
        End = new DateTime()
    };
}
