using Microsoft.EntityFrameworkCore;
using App.DAL.Interfaces;

namespace App.DAL
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();
        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<ActivityEntity> Activities => Set<ActivityEntity>();
        
        public ApplicationContext( DbContextOptions<ApplicationContext> options) :base(options)
        {
           // Database.EnsureDeleted();
         //  Database.EnsureCreated();
        }
       
       
    }
}
