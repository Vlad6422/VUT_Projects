using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seminar_CSharp.Interfaces
{

    public interface IActivity
    {
        int Id { get; set; }
        DateTime Start { get; set; }
        DateTime End { get; set; }
        string ?Type { get; set; }
        string ?Tag { get; set; }
        string ?Description { get; set; }
        int UserId { get; set; }
        int ProjectId { get; set; }
    }
}
