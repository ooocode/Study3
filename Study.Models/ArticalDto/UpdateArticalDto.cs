using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Dto.ArticalDto
{
    /// <summary>
    /// 更新文章
    /// </summary>
    public class UpdateArticalDto
    {
        /// <summary>
        /// 文章id
        /// </summary>
        [Required]
        public string ArticalId  { get; set; }


        /// <summary>
        ///分类Id
        /// </summary>
        [Required]
        public string ClassificationId { get; set; }



        /// <summary>
        /// 文章标题
        /// </summary>
        [Required]
        public string Title { get; set; }


        /// <summary>
        /// 文章内容
        /// </summary>
        [Required]
        public string Content { get; set; }


        /// <summary>
        /// 访问量
        /// </summary>
        public long VisitCount { get; set; }
    }
}
