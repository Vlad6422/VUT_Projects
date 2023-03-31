using Microsoft.EntityFrameworkCore;
using Time2Plan.DAL.Interfaces;

namespace Time2Plan.Common.Tests.Seeds
{
    public static class UserSeeds
    {
        public static readonly UserEntity EmptyUser = new()
        {
            Id = default,
            Name = default!,
            Surname = default!,
            Photo = default,
        };
        public static readonly UserEntity User1 = new()
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Surname = "Doe",
            Photo = "https://example.com/john-doe.jpg",
        };

        public static readonly UserEntity User2 = new()
        {
            Id = Guid.NewGuid(),
            Name = "Bob",
            Surname = "Smith",
            Photo = null,
        };

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().HasData(EmptyUser, User1, User2);
        }
    }
}
