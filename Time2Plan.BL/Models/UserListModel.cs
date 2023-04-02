namespace Time2Plan.BL.Models;

public record UserListModel : ModelBase
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string NickName { get; set; }
    public string? Photo { get; set; }

    public static UserListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Name = string.Empty,
        Surname = string.Empty,
        NickName = string.Empty,
        Photo = string.Empty
    };
}
