using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time2Plan.DAL.Entities;

namespace Time2Plan.DAL.Seeds;
public static class UserSeeds
{
    public static readonly UserEntity StepanUser = new()
    {
        Id = Guid.Parse("fabde0cd-eefe-443f-baf6-3d96cc2cbf2e"),
        Name = "Stepan",
        Surname = "BasketbollMaster",
        NickName = "Stepan123"
    };
    static UserSeeds()
    {

    }
    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<UserEntity>().HasData(
            StepanUser 
        );
}