﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Study.Database;

namespace Study.WebApp.Migrations
{
    [DbContext(typeof(AppDatabaseContext))]
    [Migration("20190809130324_InitCreate1")]
    partial class InitCreate1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("Study.Database.Entity.ArticalEntities.Artical", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClassificationId")
                        .IsRequired();

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("PublishTime");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.Property<long>("VisitCount");

                    b.HasKey("Id");

                    b.ToTable("Articals");
                });

            modelBuilder.Entity("Study.Database.Entity.ArticalEntities.ArticalClassification", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddDateTime");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("ArticalClassifications");

                    b.HasData(
                        new
                        {
                            Id = "1F1E7427-977C-4922-B450-7B63784F24FE",
                            AddDateTime = new DateTime(2019, 8, 9, 21, 3, 23, 837, DateTimeKind.Local).AddTicks(8948),
                            Name = "系统公告"
                        },
                        new
                        {
                            Id = "2F1E7427-977C-4922-B450-7B63784F24FE",
                            AddDateTime = new DateTime(2019, 8, 9, 21, 3, 23, 840, DateTimeKind.Local).AddTicks(5760),
                            Name = "Asp.Net"
                        },
                        new
                        {
                            Id = "221E7427-977C-4922-B450-7B63784F24FE",
                            AddDateTime = new DateTime(2019, 8, 9, 21, 3, 23, 840, DateTimeKind.Local).AddTicks(5768),
                            Name = "数据库"
                        },
                        new
                        {
                            Id = "221E5427-977C-4922-B450-7B63784F24FE",
                            AddDateTime = new DateTime(2019, 8, 9, 21, 3, 23, 840, DateTimeKind.Local).AddTicks(5770),
                            Name = "数据结构与算法"
                        },
                        new
                        {
                            Id = "221E5427-977C-4922-B450-7B63784F25FE",
                            AddDateTime = new DateTime(2019, 8, 9, 21, 3, 23, 840, DateTimeKind.Local).AddTicks(5770),
                            Name = "计算机网络"
                        });
                });

            modelBuilder.Entity("Study.Database.Entity.ArticalEntities.ArticalComment", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ArticalId")
                        .IsRequired();

                    b.Property<string>("CommentContent")
                        .IsRequired();

                    b.Property<DateTime>("CommentTime");

                    b.Property<string>("CommenterId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("ArticalComments");
                });

            modelBuilder.Entity("Study.Database.Entity.ClassBase", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("ClassBases");
                });

            modelBuilder.Entity("Study.Database.Entity.Movie", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Title");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("Study.Database.Entity.StudentTask", b =>
                {
                    b.Property<string>("TaskId");

                    b.Property<string>("StudentId");

                    b.Property<DateTime>("DateTime");

                    b.Property<float>("Grade");

                    b.Property<bool>("IsTeacherModified");

                    b.Property<string>("StudentAnswer")
                        .IsRequired();

                    b.Property<string>("TeacherReply");

                    b.HasKey("TaskId", "StudentId");

                    b.HasAlternateKey("StudentId", "TaskId");

                    b.ToTable("StudentTasks");
                });

            modelBuilder.Entity("Study.Database.Entity.TeacherClass", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("TeacherId");

                    b.Property<DateTime>("AddDateTime");

                    b.Property<string>("Description");

                    b.HasKey("Id", "TeacherId");

                    b.ToTable("TeacherClasses");
                });

            modelBuilder.Entity("Study.Database.Entity.TeacherCourse", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddDateTime");

                    b.Property<string>("Desc");

                    b.Property<DateTime>("LastModifyDatetime");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("TeacherId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("TeacherCourses");
                });

            modelBuilder.Entity("Study.Database.Entity.TeacherHelper", b =>
                {
                    b.Property<string>("TeacherId");

                    b.Property<string>("HelpId");

                    b.HasKey("TeacherId", "HelpId");

                    b.HasAlternateKey("HelpId", "TeacherId");

                    b.ToTable("TeacherHelpers");
                });

            modelBuilder.Entity("Study.Database.Entity.TeacherTask", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClassIds")
                        .IsRequired();

                    b.Property<string>("CourseId")
                        .IsRequired();

                    b.Property<string>("HelperId");

                    b.Property<string>("TaskContent")
                        .IsRequired();

                    b.Property<DateTime>("TaskEndTime");

                    b.Property<string>("TaskName")
                        .IsRequired();

                    b.Property<DateTime>("TaskStartTime");

                    b.Property<DateTime>("TaskWriteTime");

                    b.Property<string>("TeacherId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("TeacherTasks");
                });
#pragma warning restore 612, 618
        }
    }
}
