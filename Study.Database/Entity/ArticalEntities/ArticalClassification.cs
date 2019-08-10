using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Database.Entity.ArticalEntities
{
    /// <summary>
    /// 文章分类
    /// </summary>
    public class ArticalClassification
    {
        /// <summary>
        /// 分类ID
        /// </summary>
        [Key]
        public string Id  { get; set; }


        /// <summary>
        ///分类名称
        /// </summary>
        [Required]
        public string Name { get; set; }


        /// <summary>
        /// 添加时间
        /// </summary>
        [Required]
        public DateTime AddDateTime  { get; set; }
    }
}
