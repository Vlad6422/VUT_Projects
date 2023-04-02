using System;

namespace Time2Plan.BL.Models;

public record ActivityListModel : ModelBase
{
    public required DateTime Start { get; set; }
    public required DateTime End { get; set; }
    public string? Type { get; set; }
    public string? Tag { get; set; }

    public static ActivityListModel Empty
        => new()
        {
            Id = Guid.NewGuid(),
            Start = new DateTime(),
            End = new DateTime()
        };
}
