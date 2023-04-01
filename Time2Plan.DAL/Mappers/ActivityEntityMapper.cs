using Time2Plan.DAL.Interfaces;

namespace Time2Plan.DAL.Mappers;

public class ActivityEntityMapper : IEntityMapper<ActivityEntity>
{
    public void MapToExistingEntity(ActivityEntity existingEntity, ActivityEntity newEntity)
    {
        existingEntity.Start = newEntity.Start;
        existingEntity.End = newEntity.End;
        existingEntity.Description = newEntity.Description;
        existingEntity.Tag = newEntity.Tag;
        existingEntity.Type = newEntity.Type;
        //...nevim
    }
}
