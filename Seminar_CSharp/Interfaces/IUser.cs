using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seminar_CSharp.Interfaces
{
    public interface IUser
    {
        int UserId { get; set; }
        string Name { get; set; }
        string Surname { get; set; }
        string ?Photo { get; set; }
    }
}
