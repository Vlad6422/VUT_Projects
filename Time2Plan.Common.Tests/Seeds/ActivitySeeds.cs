﻿using Time2Plan.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Time2Plan.Common.Tests.Seeds
{
    public static class ActivitySeeds
    {
        public static readonly ActivityEntity EmptyActivity = new()
        {
            Id= default,
            End = default,
            Start = default,
            Tag= default,
            Type= default,
            Description= default,
            User= default,
            Project= default,
        };

        public static readonly ActivityEntity Code = new()
        {
            Id = Guid.NewGuid(),
            Start = new DateTime(2022, 1, 1, 8, 0, 0),
            End = new DateTime(2022, 1, 1, 10, 0, 0),
            Type = "Meeting",
            Tag = "Planning",
            Description = "Planning meeting with the team",
        };

        public static readonly ActivityEntity Run = new()
        {
            Id = Guid.NewGuid(),
            Start = new DateTime(2022, 1, 2, 13, 0, 0),
            End = new DateTime(2022, 1, 2, 16, 0, 0),
            Type = "Runnin",
            Tag = "Sport",
            Description = "5 km run",
        };

        public static readonly ActivityEntity CodeDelete = Code with { Id = Guid.NewGuid() };
        public static readonly ActivityEntity CodeUpdate = Code with { Id = Guid.NewGuid() };


        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityEntity>().HasData(Code,Run,CodeDelete,CodeUpdate);
        }
    }
}