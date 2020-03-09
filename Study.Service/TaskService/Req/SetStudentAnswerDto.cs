using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Services.TaskService.Req
{
    /// <summary>
    /// 学生答题
    /// </summary>
    public class SetStudentAnswerDto
    {
        /// <summary>
        /// 作业id
        /// </summary>
        [Required]
        public string TaskId { get; set; }

        /// <summary>
        /// 学生id
        /// </summary>
        [Required]
        public string StudentId  { get; set; }

        /// <summary>
        /// 作业答案
        /// </summary>
        [Required(ErrorMessage = "答案不能为空")]
        public string Answer { get; set; }
    }
}
