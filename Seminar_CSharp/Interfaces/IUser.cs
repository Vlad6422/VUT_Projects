using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seminar_CSharp.Interfaces
{
    internal interface IUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Foto { get; set; }
        public string Email { get; set; }
        public List<IActivity> Activitys { get; set; }
        public List<IProject> Projects { get; set; }
        public IProject Project { get; set; }

    }
}
