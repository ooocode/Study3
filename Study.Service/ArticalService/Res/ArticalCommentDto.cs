using Study.Services.UserService.Res;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.Services.ArticalService.Res
{
    /// <summary>
    /// 文章评论
    /// </summary>
    public class ArticalCommentDto
    {
        /// <summary>
        /// 评论Id
        /// </summary>
        public long Id { get; set; }


        /// <summary>
        /// 文章ID
        /// </summary>
        public long ArticalId { get; set; }


        /// <summary>
        /// 评论者id
        /// </summary>
        public string CommenterId { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string CommentContent { get; set; }

        /// <summary>
        /// 评论时间
        /// </summary>
        public DateTime CommentTime { get; set; }
    }
}
