using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Time2Plan.DAL.Factories
{
    /// <summary>
    ///     EF Core CLI migration generation uses this DbContext to create model and migration
    /// </summary>
    public class DesignTimeDbContext : IDesignTimeDbContextFactory<ApplicationContext>
    {
        private readonly SqLiteDbContext _sqLiteDbContext;
        private const string ConnectionString = $"Data Source=Time2Plan;Cache=Shared";

        public DesignTimeDbContext()
        {
            _sqLiteDbContext = new SqLiteDbContext(ConnectionString);
        }

        public ApplicationContext CreateDbContext(string[] args) => _sqLiteDbContext.CreateDbContext();
    }
}
