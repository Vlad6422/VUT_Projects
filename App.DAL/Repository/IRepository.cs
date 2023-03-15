using System;
using System.Linq;
using System.Threading.Tasks;
using Time2Plan.DAL.Interfaces;

namespace Time2Plan.DAL.Repositories;

/// <summary>
/// Genericke rozhrani repozitare nad entitami - mozne pridat/odebrat metody
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IRepository<TEntity> where TEntity : class, IEntity
{
    IQueryable<TEntity> Get();
    void Delete(Guid entityId);
    ValueTask<bool> ExistsAsync(TEntity entity);
    Task<TEntity> InsertAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
}
