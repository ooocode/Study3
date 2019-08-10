using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Database.Entity
{
    public class TeacherCourse
    {
        /// <summary>
        /// 课程id
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// 教师id
        /// </summary>
        //[Key]
        [Required]
        public string TeacherId { get; set; }


        /// <summary>
        /// 课程名称
        /// </summary>
        [Required]
        public string Name { get; set; }



        /// <summary>
        /// 课程描述
        /// </summary>
        public string Desc { get; set; }



        /// <summary>
        /// 添加时间（这个课程是什么时候添加的）
        /// </summary>
        [Required]
        public DateTime AddDateTime { get; set; }


        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastModifyDatetime { get; set; }
    }
}
