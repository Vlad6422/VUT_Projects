using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time2Plan.DAL.Interfaces;

namespace Time2Plan.Common.Tests.Seeds
{
    internal class ProjectUserSeeds
    {
        public static readonly ProjectUserRelation EmtpyRelation = new()
        {
            Id = default,
            Project = default,
            User = default,
        };

        public static readonly ProjectUserRelation ProjectAlphaUser1 = new()
        {
            Id = Guid.NewGuid(),
            Project = ProjectSeeds.ProjectAlpha,
            User = UserSeeds.User1,
        };


    }
}
