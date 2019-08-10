using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Database.Entity
{
    /// <summary>
    /// 学生任务表
    /// </summary>
    public class StudentTask
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        [Key]
        public string TaskId { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        [Key]
        public string StudentId { get; set; }


        /// <summary>
        /// 学生答案
        /// </summary>
        [Required]
        public string StudentAnswer { get; set; }

        /// <summary>
        /// 任务成绩
        /// </summary>
        public float Grade { get; set; }

        /// <summary>
        /// 答题时间
        /// </summary>
        public DateTime DateTime { get; set; }


        /// <summary>
        /// 教师回复信息
        /// </summary>
        public string TeacherReply  { get; set; }


        /// <summary>
        /// 教师是否改过这份作业
        /// </summary>
        public bool IsTeacherModified { get; set; }
    }
}
