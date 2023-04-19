using Time2Plan.DAL.Entities;

namespace Time2Plan.DAL.Mappers;

public interface IEntityMapper<in TEntity> where TEntity : IEntity
{
    public void MapToExistingEntity(TEntity existingEntity, TEntity newEntity);
}
