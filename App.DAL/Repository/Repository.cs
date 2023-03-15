using Time2Plan.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Time2Plan.DAL.Mappers;

namespace Time2Plan.DAL.Repositories;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity
{
    private readonly DbSet<TEntity> _dbSet;                // instance DbSet
    private readonly IEntityMapper<TEntity> _entityMapper; // instance mapperu - updatuje existujici entity

    public Repository(DbContext dbContext, IEntityMapper<TEntity> entityMapper)
    { 
        _dbSet = dbContext.Set<TEntity>(); // drzi databazove pripojeni
        _entityMapper = entityMapper;
    }

    public IQueryable<TEntity> Get() => _dbSet; // ve vyssich vrstvach je treba materializovat

    public async ValueTask<bool> ExistsAsync(TEntity entity) // asynchroni
        => entity.Id != Guid.Empty && await _dbSet.AnyAsync(e => e.Id == entity.Id);
    // prazde ID = nebude v databazi /// jinak se ptame databaze

 
    public async Task<TEntity> InsertAsync(TEntity entity)
        => (await _dbSet.AddAsync(entity)).Entity;


    //updatuje pouze entity - nenavigacni atributy, zadne kolekce
    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        TEntity existingEntity = await _dbSet.SingleAsync(e => e.Id == entity.Id);
        _entityMapper.MapToExistingEntity(existingEntity, entity);
        return existingEntity;
    }

    public void Delete(Guid entityId) => _dbSet.Remove(_dbSet.Single(i => i.Id == entityId));
}
