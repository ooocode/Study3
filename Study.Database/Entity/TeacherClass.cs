using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Database.Entity
{
    /// <summary>
    /// 教师班级
    /// </summary>
    public class TeacherClass
    {
        /// <summary>
        /// 班级id(唯一的)
        /// </summary>
        [Key]
        public string Id { get; set; }


        /// <summary>
        /// 教师Id
        /// </summary>
        [Key]
        public string TeacherId { get; set; }


        /// <summary>
        /// 班级描述
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// 添加时间（这个班级是什么时候添加的）
        /// </summary>
        [Required]
        public DateTime AddDateTime { get; set; }
    }
}
