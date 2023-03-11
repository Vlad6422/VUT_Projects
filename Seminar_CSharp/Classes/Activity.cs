using Seminar_CSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seminar_CSharp.Class
{
    
     class Activity : IActivity
    {
        public int Id { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }
        public string ?Type { get; set; }
        public string ?Tag { get; set; }
        public string ?Description { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public Project? Project { get; set; }
        public User? User { get; set; }
    }
}
