using Microsoft.EntityFrameworkCore;
using Seminar_CSharp.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seminar_CSharp
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Activity> Activities { get; set; }
        
        public ApplicationContext( DbContextOptions<ApplicationContext> options) :base(options)
        {
           // Database.EnsureDeleted();
         //  Database.EnsureCreated();
        }
       
       
    }
}
