using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Services.ArticalService.Req
{
    public class AddArticalCommentDto
    {
        /// <summary>
        /// 文章id
        /// </summary>
        [Required]
        public long ArticalId { get; set; }

        /// <summary>
        /// 评论者id
        /// </summary>
        [Required]
        public string CommenterId{ get; set; }


        /// <summary>
        /// 评论内容
        /// </summary>
        [Required(ErrorMessage = "评论内容不能为空")]
        public string CommentContent { get; set; }
    }
}
