using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time2Plan.DAL.Factories;

public class SqlServerDbContext : IDbContextFactory<ApplicationContext>
{
    private readonly bool _seedDemoData;
    private readonly DbContextOptionsBuilder<ApplicationContext> _contextOptionsBuilder = new();

    public SqlServerDbContext(string connectionString, bool seedDemoData = false)
    {
        _seedDemoData = seedDemoData;

        _contextOptionsBuilder.UseSqlServer(connectionString);

        ////Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
        //_contextOptionsBuilder.LogTo(System.Console.WriteLine);
        //_contextOptionsBuilder.EnableSensitiveDataLogging();
    }

    public ApplicationContext CreateDbContext() => new(_contextOptionsBuilder.Options, _seedDemoData);
}
