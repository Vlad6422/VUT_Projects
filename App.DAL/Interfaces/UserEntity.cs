using System;

namespace App.DAL.Interfaces
{
    public record UserEntity : IEntity
    {
        public required Guid UserId { get; set; }
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public string ?Photo { get; set; }
    }
}
