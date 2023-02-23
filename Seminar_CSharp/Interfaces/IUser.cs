using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seminar_CSharp.Interfaces
{
    internal interface IUser
    {
        int UserId { get; set; }
        string Name { get; set; }
        string Surname { get; set; }
        string Photo { get; set; }
      //  List<IProject> Projects { get; set; }
        //List<IActivity> Activities { get; set; }


    }
}
