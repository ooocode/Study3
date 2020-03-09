using Study.Database.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Study.Database.Entity.UserEntities;
using Study.Database.Entity.ArticalEntities;
using Study.Database.Entity.CourseEntities;
using Study.Database.Entity.TaskEntities;
namespace Study.Database
{
    public class UserDatabaseContext : DbContext
    {
        /// <summary>
        /// 用户朋友表
        /// </summary>
        public DbSet<UserFriend> UserFriends { get; set; }


        /// <summary>
        /// 教师的班级表
        /// </summary>
        public DbSet<TeacherClass> TeacherClasses { get; set; }


        /// <summary>
        /// 学生任务表
        /// </summary>
        public DbSet<StudentTask> StudentTasks { get; set; }

        /// <summary>
        /// 教师任务表
        /// </summary>
        public DbSet<TeacherTask> TeacherTasks { get; set; }


        /// <summary>
        /// 课程表
        /// </summary>
        public DbSet<Course> Courses { get; set; }



        /// <summary>
        /// 文章分类
        /// </summary>
        public DbSet<ArticleClassification> ArticalClassifications { get; set; }


        /// <summary>
        /// 文章表
        /// </summary>
        public DbSet<Article> Articals { get; set; }


        /// <summary>
        /// 文章评论表
        /// </summary>
        public DbSet<ArticleComment> ArticalComments { get; set; }



        public UserDatabaseContext(DbContextOptions<UserDatabaseContext> options)
            : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<ArticleClassification>().HasData(
                new ArticleClassification
                {
                    Id = 1,
                    Name = "系统公告"
                }
            );

            //设置教师班级主键
            builder.Entity<TeacherClass>().HasKey(e => new { e.TeacherId, e.ClassId });

            //设置学生作业主键
            builder.Entity<StudentTask>().HasKey(e => new { e.TaskId, e.StudentId });

            //设置用户好友表主键
            builder.Entity<UserFriend>().HasKey(e => new { e.UserId, e.FriendId });


            //设置课程表唯一索引  每个用户不允许同名课程
            builder.Entity<Course>().HasIndex(e => new { e.UserId, e.Name }).IsUnique();

            //设置教师作业表唯一索引  每个用户一个课程只能一个作业名称 不能重复作业名称
            builder.Entity<TeacherTask>().HasIndex(e => new { e.TeacherId, e.CourseId,e.TaskName }).IsUnique();
        }
    }
}
