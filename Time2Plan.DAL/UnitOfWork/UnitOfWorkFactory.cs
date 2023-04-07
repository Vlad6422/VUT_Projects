using Microsoft.EntityFrameworkCore;

namespace Time2Plan.DAL.UnitOfWork;

public class UnitOfWorkFactory : IUnitOfWorkFactory
{
    private readonly IDbContextFactory<Time2PlanDbContext> _dbContextFactory;

    public UnitOfWorkFactory(IDbContextFactory<Time2PlanDbContext> dbContextFactory) =>
        _dbContextFactory = dbContextFactory;

    public IUnitOfWork Create() => new UnitOfWork(_dbContextFactory.CreateDbContext());
}
