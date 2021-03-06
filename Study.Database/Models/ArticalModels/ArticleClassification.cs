﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Database.Entity.ArticalEntities
{
    /// <summary>
    /// 文章分类
    /// </summary>
    public class ArticleClassification
    {
        /// <summary>
        /// 分类ID
        /// </summary>
        [Key]
        public long Id  { get; set; }


        /// <summary>
        ///分类名称
        /// </summary>
        [Required]
        public string Name { get; set; }


        /// <summary>
        /// 时间戳
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
