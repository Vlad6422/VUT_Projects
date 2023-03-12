using System;

namespace App.DAL.Interfaces
{
    public record ActivityEntity : IEntity
    {
        public required Guid ActivityID { get; set; }
        public required Guid Id { get; set; }
        public required DateTime Start { get; set; }
        public required DateTime End { get; set; }
        public string ?Type { get; set; }
        public string ?Tag { get; set; }
        public string ?Description { get; set; }
        public required int UserId { get; set; }
        public required int ProjectId { get; set; }
    }
}
