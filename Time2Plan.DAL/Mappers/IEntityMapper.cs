using Time2Plan.DAL.Interfaces;

namespace Time2Plan.DAL.Mappers;

/// <summary>
/// Generické rozhraní pro mapování dat z jedné instance entity na druhou
/// Lze použít pro entity, které implementují rozhraní IEntity
/// klíčové slovo in zajišťuje, že TEntity resp. IEntity je pouze vstupní parametr
/// bere pouze skalarni property - ne navigacni
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IEntityMapper<in TEntity> where TEntity : IEntity
{
    /// <summary>
    /// Aktualizuje data stávající entity pomocí dat z nové entity.
    /// </summary>
    /// <param name="existingEntity"></param>
    /// <param name="newEntity"></param>
    public void MapToExistingEntity(TEntity existingEntity, TEntity newEntity);
}
