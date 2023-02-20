using Seminar_CSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seminar_CSharp.Class
{
    enum Type
    {
        Unknown = 0,
    }
    enum Tag
    {
        Unknown = 0,
    }
    internal class Activity : IActivity
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Type? Type { get; set; }
        public Tag Tag { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public Project? Project { get; set; }
        public User? User { get; set; }
    }
}
