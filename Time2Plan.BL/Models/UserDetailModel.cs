using System.Collections.ObjectModel;

namespace Time2Plan.BL.Models;

public record UserDetailModel : ModelBase
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string NickName { get; set; }
    public bool? ShowNickName { get; set; }
    public string? Photo { get; set; }
    public ObservableCollection<ActivityListModel> Activities { get; init; } = new();
    public ObservableCollection<UserProjectListModel> UserProjects { get; init; } = new();

    public static UserDetailModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty,
        Surname = string.Empty,
        NickName = string.Empty,
        Photo = null
    };
}
