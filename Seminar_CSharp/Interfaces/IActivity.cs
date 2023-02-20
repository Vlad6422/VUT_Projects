using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seminar_CSharp.Interfaces
{
    enum Type
    {
        Unknown = 0,
    }
    internal interface IActivity
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Type Type { get; set; }
        public string Description { get; set; }
        public IProject Project { get; set; }
        public IUser User { get; set; }
    }
}
