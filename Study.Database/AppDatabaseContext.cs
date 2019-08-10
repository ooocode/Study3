using Study.Database.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Study.Database.Entity.ArticalEntities;

namespace Study.Database
{
    public class AppDatabaseContext : DbContext
    {
        /// <summary>
        /// 电影
        /// </summary>
        public DbSet<Movie> Movies { get; set; }


        /// <summary>
        /// 班级表
        /// </summary>
        public DbSet<ClassBase> ClassBases { get; set; }




        /// <summary>
        /// 学生任务表
        /// </summary>
        public DbSet<StudentTask> StudentTasks { get; set; }




        /// <summary>
        /// 教师课程表
        /// </summary>
        public DbSet<TeacherCourse> TeacherCourses { get; set; }


        /// <summary>
        /// 教师课程表
        /// </summary>
        public DbSet<TeacherClass> TeacherClasses { get; set; }

        /// <summary>
        /// 教师任务表
        /// </summary>
        public DbSet<TeacherTask> TeacherTasks { get; set; }


        /// <summary>
        /// 教师助手
        /// </summary>
        public DbSet<TeacherHelper> TeacherHelpers { get; set; }


        /// <summary>
        /// 文章分类
        /// </summary>
        public DbSet<ArticalClassification> ArticalClassifications { get; set; }

        /// <summary>
        /// 文章表
        /// </summary>
        public DbSet<Artical> Articals { get; set; }


        /// <summary>
        /// 文章评论表
        /// </summary>
        public DbSet<ArticalComment> ArticalComments { get; set; }

        bool hadParam = false;

        public AppDatabaseContext()
        {
            hadParam = true;
        }

        public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (hadParam)
            {
                optionsBuilder.UseSqlite("Data Source=database.db");
            }
            //optionsBuilder.UseSqlite($"Data Source={AppContext.BaseDirectory}\\mydb.db;");
            //optionsBuilder.UseSqlServer("Server = localhost;Database = SchoolDB;Trusted_Connection=True;",
            //    e => e.MigrationsAssembly("Blog.Demo"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //设置主键

            modelBuilder.Entity<TeacherClass>().HasKey(
                 e => new { e.Id, e.TeacherId });


            modelBuilder.Entity<StudentTask>().HasKey(
            e => new { e.TaskId, e.StudentId });


            modelBuilder.Entity<TeacherHelper>().HasKey(
           e => new { e.TeacherId, e.HelpId });



            modelBuilder.Entity<ArticalClassification>().HasData(
                new ArticalClassification
                {
                    Id = "1F1E7427-977C-4922-B450-7B63784F24FE",
                    AddDateTime = DateTime.Now,
                    Name = "系统公告"
                },
                new ArticalClassification
                {
                    Id = "2F1E7427-977C-4922-B450-7B63784F24FE",
                    AddDateTime = DateTime.Now,
                    Name = "Asp.Net"
                },

                new ArticalClassification
                {
                    Id = "221E7427-977C-4922-B450-7B63784F24FE",
                    AddDateTime = DateTime.Now,
                    Name = "数据库"
                },

                new ArticalClassification
                {
                    Id = "221E5427-977C-4922-B450-7B63784F24FE",
                    AddDateTime = DateTime.Now,
                    Name = "数据结构与算法"
                },

                new ArticalClassification
                {
                    Id = "221E5427-977C-4922-B450-7B63784F25FE",
                    AddDateTime = DateTime.Now,
                    Name = "计算机网络"
                }
            );
            //modelBuilder.FinalizeModel();
        }
    }
}
