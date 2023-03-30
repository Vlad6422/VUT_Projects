using Time2Plan.DAL.Interfaces;

namespace Time2Plan.DAL.Mappers;

public class UserEntityMapper : IEntityMapper<UserEntity>
{
    public void MapToExistingEntity(UserEntity existingEntity, UserEntity newEntity) 
    { 
        existingEntity.Surname = newEntity.Surname;
        existingEntity.Name = newEntity.Name;
        existingEntity.Photo = newEntity.Photo;
        // potreba promyslet co vsechno se ma namapovat
    }
}
