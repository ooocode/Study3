using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;

namespace Study.Database.Entity
{
    /// <summary>
    /// 班级
    /// </summary>
    public class ClassBase
    {
        /// <summary>
        /// 班级id(唯一的)
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
