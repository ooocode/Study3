using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Study.Database.Entity.ArticalEntities
{
    /// <summary>
    /// 文章评论表
    /// </summary>
    public class ArticleComment
    {
        /// <summary>
        /// 评论Id
        /// </summary>
        [Key]
        public long Id { get; set; }


        /// <summary>
        /// 文章ID
        /// </summary>
        [Required]
        public long ArticalId { get; set; }


        /// <summary>
        /// 评论者id
        /// </summary>
        [Required]
        public string CommenterId { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        [Required]
        public string CommentContent { get; set; }

        /// <summary>
        /// 评论时间
        /// </summary>
        [Required]
        public DateTime CommentTime { get; set; }
    }
}
