using Seminar_CSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seminar_CSharp.Class
{
    internal class User : IUser
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Photo { get; set; }

        [Required]
        [MaxLength(20)]
        public string NickName { get; set; }
        public bool ShowNickName { get; set; }

        public List<Activity> Activities { get; set; }
        public List<Project> Projects { get; set; } 
     //  public int ProjectId { get; set; }
       // public Project? Project { get; set; }
    }
}
