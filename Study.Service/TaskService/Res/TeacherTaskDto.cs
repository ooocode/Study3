using Study.Service.CourseService.Res;
using Study.Services.UserService.Res;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.Services.TaskService.Res
{
    public class TeacherTaskDto
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public string Id { get; set; }


        /// <summary>
        /// 教师id
        /// </summary>
        public string TeacherId { get; set; }


        /// <summary>
        /// 修改作业的人id
        /// </summary>
        public string HelperId { get; set; }

        /// <summary>
        /// 课程id
        /// </summary>
        public string CourseId { get; set; }


        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// 任务内容
        /// </summary>
        public string TaskContent { get; set; }


        /// <summary>
        /// 任务写的时间  这个任务什么时候写的
        /// </summary>
        public DateTime TaskWriteTime { get; set; }

        /// <summary>
        /// 任务开始时间
        /// </summary>
        public DateTime TaskStartTime { get; set; }


        /// <summary>
        /// 任务结束时间
        /// </summary>
        public DateTime TaskEndTime { get; set; }

        /// <summary>
        /// 文件   json格式化  List<string>
        /// </summary>
        public string Files { get; set; }
    }
}
