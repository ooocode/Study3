using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Services.ArticalService.Req
{
    /// <summary>
    /// 添加文章
    /// </summary>
    public class AddArticalDto
    {
        /// <summary>
        /// 作者Id
        /// </summary>
        public string UserId { get; set; }


        /// <summary>
        ///分类Id
        /// </summary>
        [Required]
        public long ClassificationId { get; set; }


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
    }
}
