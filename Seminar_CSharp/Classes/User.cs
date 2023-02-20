using Seminar_CSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seminar_CSharp.Class
{
    internal class User : IUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Foto { get; set; }
        public string NickName { get; set; }
        public List<Activity> Activitys { get; set; } = new();
        public List<Project> Projects { get; set; } =new();
     //  public int ProjectId { get; set; }
       // public Project? Project { get; set; }
    }
}
