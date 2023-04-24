using Microsoft.EntityFrameworkCore;
using Time2Plan.DAL.Factories;
using Time2Plan.App.Options;
using Time2Plan.DAL;
using Time2Plan.DAL.Mappers;
using Microsoft.Extensions.Configuration;

namespace Time2Plan.App;

public static class DALInstaller
{
    public static IServiceCollection AddDALServices(this IServiceCollection services, IConfiguration configuration)
    {
        DALOptions dalOptions = new();
        configuration.GetSection("Time2Plan:DAL").Bind(dalOptions);

        services.AddSingleton<DALOptions>(dalOptions);

        if (dalOptions.LocalDb is null && dalOptions.Sqlite is null)
        {
            throw new InvalidOperationException("No persistence provider configured");
        }

        if (dalOptions.LocalDb?.Enabled == false && dalOptions.Sqlite?.Enabled == false)
        {
            throw new InvalidOperationException("No persistence provider enabled");
        }

        if ((dalOptions.LocalDb?.Enabled == true) && (dalOptions.Sqlite?.Enabled == true))
        {
            throw new InvalidOperationException("Both persistence providers enabled");
        }

        if (dalOptions.LocalDb?.Enabled == true)
        {
            services.AddSingleton<IDbContextFactory<Time2PlanDbContext>>(provider => new SqlServerDbContextFactory(dalOptions.LocalDb.ConnectionString));
            services.AddSingleton<IDbMigrator, NoneDbMigrator>();
        }

        if (dalOptions.Sqlite?.Enabled == true)
        {
            if (dalOptions.Sqlite.DatabaseName is null)
            {
                throw new InvalidOperationException($"{nameof(dalOptions.Sqlite.DatabaseName)} is not set");

            }
            string databaseFilePath = Path.Combine(FileSystem.AppDataDirectory, dalOptions.Sqlite.DatabaseName!);
            services.AddSingleton<IDbContextFactory<Time2PlanDbContext>>(provider => new DbContextSqLiteFactory(databaseFilePath, dalOptions?.Sqlite?.SeedDemoData ?? false));
            services.AddSingleton<IDbMigrator, SqliteDbMigrator>();
        }

        services.AddSingleton<UserEntityMapper>();
        services.AddSingleton<ProjectEntityMapper>();
        services.AddSingleton<ActivityEntityMapper>();
        services.AddSingleton<ProjectUserRelationMapper>();

        return services;
    }
}
