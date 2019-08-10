using System;
using System.Collections.Generic;
using System.Text;

namespace Study.Dto.ArticalDto
{
    public class ArticalCommentDto
    {
        /// <summary>
        /// 评论Id
        /// </summary>
        public string Id { get; set; }


        /// <summary>
        /// 文章ID
        /// </summary>
        public string ArticalId { get; set; }


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
