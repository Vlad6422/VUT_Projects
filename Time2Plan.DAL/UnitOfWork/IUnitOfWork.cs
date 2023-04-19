using Time2Plan.DAL.Entities;
using Time2Plan.DAL.Mappers;
using Time2Plan.DAL.Repository;

namespace Time2Plan.DAL.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    IRepository<TEntity> GetRepository<TEntity, TEntityMapper>()
    where TEntity : class, IEntity
    where TEntityMapper : IEntityMapper<TEntity>, new();

    Task CommitAsync();
}
