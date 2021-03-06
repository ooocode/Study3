﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Database.Entity.TaskEntities
{
    public class TeacherTask
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        [Key]
        public string Id { get; set; }


        /// <summary>
        /// 教师id
        /// </summary>
        [Required]
        public string TeacherId { get; set; }


        /// <summary>
        /// 修改作业的人id
        /// </summary>
        public string HelperId { get; set; }

        /// <summary>
        /// 课程id
        /// </summary>
        [Required]
        public string CourseId { get; set; }


        /// <summary>
        /// 任务名称
        /// </summary>
        [Required]
        public string TaskName { get; set; }

        /// <summary>
        /// 任务内容
        /// </summary>
        [Required]
        public string TaskContent { get; set; }


        /// <summary>
        /// 任务写的时间  这个任务什么时候写的
        /// </summary>
        [Required]
        public DateTime TaskWriteTime { get; set; }

        /// <summary>
        /// 任务开始时间
        /// </summary>
        [Required]
        public DateTime TaskStartTime { get; set; }


        /// <summary>
        /// 任务结束时间
        /// </summary>
        [Required]
        public DateTime TaskEndTime { get; set; }

        /// <summary>
        /// 文件   json格式化  List<string>
        /// </summary>
        public string  Files { get; set; }
    }
}
