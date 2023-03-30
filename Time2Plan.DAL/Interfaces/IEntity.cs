using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time2Plan.DAL.Interfaces;

public interface IEntity
{
    Guid Id { get; set; }
}
