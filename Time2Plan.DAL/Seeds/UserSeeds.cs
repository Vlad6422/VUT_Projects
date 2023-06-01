using Microsoft.EntityFrameworkCore;
using Time2Plan.DAL.Entities;

namespace Time2Plan.DAL.Seeds;
public static class UserSeeds
{
    public static readonly UserEntity StepanUser = new()
    {
        Id = Guid.Parse("fabde0cd-eefe-443f-baf6-3746cc2cbf2e"),
        Name = "Stepan",
        Surname = "Ukazkovy",
        NickName = "StepanTheBeast"
    };

    public static readonly UserEntity AnnaUser = new()
    {
        Id = Guid.Parse("0c2964cd-d4ff-4470-8118-13080cc955e7"),
        Name = "Anna",
        Surname = "Stein",
        NickName = "AnduleStainka",
        Photo = "https://www.flera.cz/wp-content/uploads/2018/11/anna-chomjakova.jpg"
    };

    public static readonly UserEntity PatrickUser = new()
    {
        Id = Guid.Parse("c78a4a0b-bcba-49bf-8759-f539726de3f3"),
        Name = "Patricius",
        Surname = "Robinson",
        NickName = "UserAgent007",
        Photo = "https://nickelodeonuniverse.com/wp-content/uploads/Patrick.png"
    };

    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<UserEntity>().HasData(
            StepanUser,
            AnnaUser,
            PatrickUser
        );
}