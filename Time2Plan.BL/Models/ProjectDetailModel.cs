using System.Collections.ObjectModel;

namespace Time2Plan.BL.Models;

public record ProjectDetailModel : ModelBase
{
    public required string Name { get; set; }
    public string? Description { get; set; }

    public ObservableCollection<ActivityListModel> Activities { get; init; } = new();
    public ObservableCollection<UserProjectListModel> UserProjects { get; init; } = new();

    public static ProjectDetailModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty
    };
}
