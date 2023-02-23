using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seminar_CSharp.Interfaces
{
    internal interface IProject
    {
        int ProjectId { get; set; }
        string Name { get; set; }
        string Description { get; set; }
       // List<IUser> Users { get; set; }
       // List<IActivity> Activities { get; set; }
    }
}
