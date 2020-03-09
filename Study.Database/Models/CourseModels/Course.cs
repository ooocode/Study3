using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Database.Entity.CourseEntities
{
    /// <summary>
    /// 课程
    /// </summary>
    public class Course
    {
        /// <summary>
        /// 课程id
        /// </summary>
        [Key]
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



        /// <summary>
        /// 添加时间（这个课程是什么时候添加的）
        /// </summary>
        [Required]
        public DateTime AddDateTime { get; set; }


        /// <summary>
        /// 最后修改时间
        /// </summary>
        [Required]
        public DateTime LastModifyDatetime { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
