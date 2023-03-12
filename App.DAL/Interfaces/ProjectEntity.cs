using System;

namespace App.DAL.Interfaces;

public record ProjectEntity : IEntity
{
    public required Guid ProjectID { get; set; }
    public required Guid Id { get; set; } // Odkazuje na IEntitu
    public required string Name { get; set; }
    public string ?Description { get; set; }
}
