namespace Time2Plan.BL.Models;

public record ActivityDetailModel : ModelBase
{
    public required DateTime Start { get; set; }
    public required DateTime End { get; set; }
    public string? Type { get; set; }
    public string? Tag { get; set; }
    public string? Description { get; set; }

    public required Guid UserId { get; set; }
    public Guid ProjectId { get; set; }

    public static ActivityDetailModel Empty => new()
    {
        Id = Guid.Empty,
        Start = DateTime.Now,
        End = DateTime.Now.AddHours(1),
        UserId = default,
        ProjectId = default,
    };
}
