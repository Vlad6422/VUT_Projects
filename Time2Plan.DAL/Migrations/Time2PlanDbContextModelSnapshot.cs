﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Time2Plan.DAL;

#nullable disable

namespace Time2Plan.DAL.Migrations
{
    [DbContext(typeof(Time2PlanDbContext))]
    partial class Time2PlanDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.4");

            modelBuilder.Entity("Time2Plan.DAL.Entities.ActivityEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("End")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Start")
                        .HasColumnType("TEXT");

                    b.Property<string>("Tag")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("UserId");

                    b.ToTable("Activities", (string)null);
                });

            modelBuilder.Entity("Time2Plan.DAL.Entities.ProjectEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Projects", (string)null);
                });

            modelBuilder.Entity("Time2Plan.DAL.Entities.ProjectUserRelation", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "ProjectId");

                    b.HasIndex("ProjectId");

                    b.ToTable("UserProjects", (string)null);
                });

            modelBuilder.Entity("Time2Plan.DAL.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Photo")
                        .HasColumnType("TEXT");

                    b.Property<bool?>("ShowNickName")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Time2Plan.DAL.Entities.ActivityEntity", b =>
                {
                    b.HasOne("Time2Plan.DAL.Entities.ProjectEntity", "Project")
                        .WithMany("Activities")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Time2Plan.DAL.Entities.UserEntity", "User")
                        .WithMany("Activities")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Time2Plan.DAL.Entities.ProjectUserRelation", b =>
                {
                    b.HasOne("Time2Plan.DAL.Entities.ProjectEntity", "Project")
                        .WithMany("UserProjects")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Time2Plan.DAL.Entities.UserEntity", "User")
                        .WithMany("UserProjects")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Time2Plan.DAL.Entities.ProjectEntity", b =>
                {
                    b.Navigation("Activities");

                    b.Navigation("UserProjects");
                });

            modelBuilder.Entity("Time2Plan.DAL.Entities.UserEntity", b =>
                {
                    b.Navigation("Activities");

                    b.Navigation("UserProjects");
                });
#pragma warning restore 612, 618
        }
    }
}
