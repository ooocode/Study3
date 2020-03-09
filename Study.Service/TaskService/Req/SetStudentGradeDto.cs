using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Services.TaskService.Req
{
    public class SetStudentGradeDto
    {
        /// <summary>
        /// 学生id
        /// </summary>
        [Required]
        public string StudentId { get; set; }

        /// <summary>
        /// 任务id
        /// </summary>
        [Required]
        public string TaskId { get; set; }

        /// <summary>
        /// 分数
        /// </summary>
        [Required(ErrorMessage = "分数不能为空")]
        [Range(0, 100, ErrorMessage = "分数必须是0--100之间")]
        public int Grade { get; set; }

        /// <summary>
        /// 教师回复
        /// </summary>
        public string TeacherReply { get; set; }
    }
}
