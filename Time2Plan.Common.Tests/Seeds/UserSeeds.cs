using Microsoft.EntityFrameworkCore;
using Time2Plan.DAL.Entities;

namespace Time2Plan.Common.Tests.Seeds;

public static class UserSeeds
{
    public static readonly UserEntity EmptyUser = new()
    {
        Id = default,
        Name = default!,
        Surname = default!,
        NickName = default!,
        ShowNickName = default,
        Photo = default
    };
    public static readonly UserEntity User1 = new()
    {
        Id = Guid.Parse(input: "06a8a2cf-ea03-4095-a3e4-aa0291fe9c75"),
        Name = "John",
        Surname = "Doe",
        NickName = "johndoe",
        ShowNickName = true,
        Photo = "https://example.com/john-doe.jpg"
    };

    public static UserEntity UserEntity3 = new()
    {
        Id = Guid.Parse(input: "e50551ce-6300-4784-ac8b-0b1f86e2bd76"),
        Name = "Adolf Entity",
        Surname = "Sheeter Entity",
        NickName = "xxadolph0 Entity",
        ShowNickName = false,
        Photo = null
    };

    public static readonly UserEntity User1Update = User1 with { Id = Guid.Parse("42985040-fa83-4819-b981-4a06d593adbf") };
    public static readonly UserEntity User1Delete = User1 with { Id = Guid.Parse("8ba63dbc-38f5-456f-8b77-481b7f274261") };

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>().HasData(
            User1,
            User1Update,
            User1Delete,
            UserEntity3);
    }
}
