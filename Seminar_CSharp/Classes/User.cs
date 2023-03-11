using Seminar_CSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seminar_CSharp.Class
{
     class User : IUser
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        public string ?Photo { get; set; }

        [Required]
        [MaxLength(20)]
        public string NickName { get; set; }

        [Required]
        public bool ShowNickName { get; set; }

        public List<Activity> Activities { get; set; } = new();
        public List<Project> Projects { get; set; } = new();
     
    }
}
