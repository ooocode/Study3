using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Service.CourseService.Req
{
    public class AddOrUpdateCourseDto
    {
        /// <summary>
        /// 课程id(如果为空 则创建)
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        [Required]
        public string Name { get; set; }


        /// <summary>
        /// 课程描述
        /// </summary>
        public string Desc { get; set; }
    }
}
