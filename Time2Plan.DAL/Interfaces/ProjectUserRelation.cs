using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time2Plan.DAL.Interfaces;

public record ProjectUserRelation : IEntity
{
    public required Guid UserId { get; set; }
    public required Guid ProjectId { get; set; }
    public UserEntity? User { get; init; }
    public ProjectEntity? Project { get; init; }
    public required Guid Id { get; set; }
}
