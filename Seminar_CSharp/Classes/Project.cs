using Seminar_CSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seminar_CSharp.Class
{
    internal class Project : IProject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Activity> Activitys { get; set; } = new();
        public List<User> Users { get; set; } = new();
    }
}
