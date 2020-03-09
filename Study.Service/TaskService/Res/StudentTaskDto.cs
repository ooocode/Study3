using Study.Services.UserService.Res;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.Services.TaskService.Res
{
    /// <summary>
    /// 学生作业
    /// </summary>
    public class StudentTaskDto
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public string TaskId { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        public string StudentId { get; set; }


        /// <summary>
        /// 学生答案
        /// </summary>
        public string StudentAnswer { get; set; }

        /// <summary>
        /// 任务成绩
        /// </summary>
        public decimal Grade { get; set; }

        /// <summary>
        /// 答题时间
        /// </summary>
        public DateTime DateTime { get; set; }


        /// <summary>
        /// 是否答题了（这个字段有点冗余啊）
        /// </summary>
        public bool IsAlreadyAnswered { get; set; }


        /// <summary>
        /// 教师回复信息
        /// </summary>
        public string TeacherReply { get; set; }


        /// <summary>
        /// 教师是否改过这份作业
        /// </summary>
        public bool IsTeacherModified { get; set; }
    }
}
