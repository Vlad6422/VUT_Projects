using Microsoft.EntityFrameworkCore;
using Time2Plan.DAL;
using Time2Plan.App.Options;

namespace Time2Plan.App;

interface IDbMigrator
{
    public void Migrate();
    public Task MigrateAsync(CancellationToken cancellationToken);
}

public class NoneDbMigrator : IDbMigrator
{
    public void Migrate() { }
    public Task MigrateAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

//public class SqliteDbMigrator : IDbMigrator
//{
//    private readonly IDbContextFactory<Time2PlanDbContext> _dbContextFactory;
//    private readonly SqliteOptions _sqliteOptions;

//    public SqliteDbMigrator(IDbContextFactory<Time2PlanDbContext> dbContextFactory, DALOptions dalOptions)
//    {
//        _dbContextFactory = dbContextFactory;
//        _sqliteOptions = dalOptions.Sqlite ?? throw new ArgumentNullException(nameof(dalOptions), $@"{nameof(DALOptions.Sqlite)} are not set");
//    }

//    public void Migrate() => MigrateAsync(CancellationToken.None).GetAwaiter().GetResult();

//    public async Task MigrateAsync(CancellationToken cancellationToken)
//    {
//        await using Time2PlanDbContext dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

//        if (_sqliteOptions.RecreateDatabaseEachTime)
//        {
//            await dbContext.Database.EnsureDeletedAsync(cancellationToken);
//        }

//        // Ensures that database is created applying the latest state
//        // Application of migration later on may fail
//        // If you want to use migrations, you should create database by calling  dbContext.Database.MigrateAsync(cancellationToken) instead
//        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
//    }
//}

public class LocalDbMigrator : IDbMigrator
{
    private readonly IDbContextFactory<Time2PlanDbContext> _dbContextFactory;
    //private readonly LocalDbOptions _localDbOptions;


    public LocalDbMigrator(IDbContextFactory<Time2PlanDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        // _localDbOptions = dalOptions.LocalDb ?? throw new ArgumentNullException(nameof(dalOptions), $@"{nameof(DALOptions.LocalDb)} are not set");
    }

    public void Migrate() => MigrateAsync(CancellationToken.None).GetAwaiter().GetResult();

    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        await using Time2PlanDbContext dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
        //await dbContext.Database.MigrateAsync();
    }

}

