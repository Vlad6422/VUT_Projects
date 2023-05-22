using Time2Plan.DAL.Entities;

namespace Time2Plan.DAL.Mappers;

public class ProjectUserRelationMapper : IEntityMapper<ProjectUserRelation>
{
    //public void MapToExistingEntity(ProjectUserRelation existingEntity, ProjectUserRelation newEntity)
    //{
    //    throw new NotImplementedException();
    //}

    public void MapToExistingEntity(ProjectUserRelation existingEntity, ProjectUserRelation newEntity)
    {
        existingEntity.ProjectId = newEntity.ProjectId;
        existingEntity.UserId = newEntity.UserId;
    }
}
