using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Services.TaskService.Req
{
    /// <summary>
    /// 添加或更新教师任务dto
    /// </summary>
    public class AddOrUpdateTeacherTaskDto
    {
        /// <summary>
        /// 作业id (为空时则创建，否则就是更新)
        /// </summary>
        public string TaskId  { get; set; }

        /// <summary>
        /// 课程id
        /// </summary>
        [Required(ErrorMessage = "课程不能为空")]
        public string CourseId { get; set; }


        /// <summary>
        /// 任务名称
        /// </summary>
        [Required(ErrorMessage = "任务名称不能为空")]
        [StringLength(256, ErrorMessage = "任务名称最大为256个字符")]
        public string TaskName { get; set; }

        /// <summary>
        /// 任务内容
        /// </summary>
        [Required(ErrorMessage = "任务内容不能为空")]
        [StringLength(4 * 1024 * 1024, ErrorMessage = "内容最大为4MB")]
        public string TaskContent { get; set; }


        /// <summary>
        /// 任务开始时间
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy年MM月dd日}")]
        [Required(ErrorMessage = "开始时间不能为空")]
        public string TaskStartTime { get; set; }


        /// <summary>
        /// 任务结束时间
        /// </summary>
        [Required(ErrorMessage = "结束时间不能为空")]
        [DisplayFormat(DataFormatString = "{0}:yyyy-MM-dd")]
        public string TaskEndTime { get; set; }


        /// <summary>
        /// 发布给了哪些学生
        /// </summary>
        //[Required(ErrorMessage = "至少选择一个班级")]
        public List<string> StudentIds { get; set; }


        /// <summary>
        /// 助手id（修改作业的人）
        /// </summary>
        public string HelperId { get; set; }


        /// <summary>
        /// 文件   json格式化  List<string>
        /// </summary>
        public string Files { get; set; }
    }
}
