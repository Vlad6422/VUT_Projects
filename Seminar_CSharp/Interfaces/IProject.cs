using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seminar_CSharp.Interfaces
{
    internal interface IProject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<IActivity> Activitys { get; set; }
        public List<IUser> Users { get; set; }
        public IUser User { get; set; }
    }
}
