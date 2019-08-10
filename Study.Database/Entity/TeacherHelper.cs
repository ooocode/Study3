using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Database.Entity
{
    /// <summary>
    /// 教师助手
    /// </summary>
    public class TeacherHelper
    {
        /// <summary>
        /// 教师id
        /// </summary>
        [Key]
        public string TeacherId  { get; set; }


        [Key]
        public string HelpId { get; set; }
    }
}
