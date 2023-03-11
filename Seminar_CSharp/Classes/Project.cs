using Seminar_CSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seminar_CSharp.Class
{
    class Project : IProject
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public string ?Description { get; set; }

        public List<Activity> Activities { get; set; } = new();
        public List<User> Users { get; set; } = new(); 
    }
}
