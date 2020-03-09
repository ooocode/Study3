using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Database.Entity.UserEntities
{
    public class TeacherClass
    {
        /// <summary>
        /// 教师id
        /// </summary>
        [Key]
        public string TeacherId { get; set; }

        /// <summary>
        /// 学校班级Id
        /// </summary>
        [Key]
        public string ClassId { get; set; }
    }
}
