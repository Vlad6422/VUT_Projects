using Time2Plan.DAL.Entities;

namespace Time2Plan.DAL.Mappers;

public class ActivityEntityMapper : IEntityMapper<ActivityEntity>
{
    public void MapToExistingEntity(ActivityEntity existingEntity, ActivityEntity newEntity)
    {
        existingEntity.Start = newEntity.Start;
        existingEntity.ProjectId = newEntity.ProjectId;
        existingEntity.UserId = newEntity.UserId;
        existingEntity.End = newEntity.End;
        existingEntity.Description = newEntity.Description;
        existingEntity.Tag = newEntity.Tag;
        existingEntity.Type = newEntity.Type;
    }
}
