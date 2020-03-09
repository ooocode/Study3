using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Study.Database.Entity.ArticalEntities
{
    public class Article
    {
        /// <summary>
        /// 文章GUID
        /// </summary>
        [Key]
        public long Id { get; set; }


        /// <summary>
        ///分类Id
        /// </summary>
        [Required]
        public long ClassificationId { get; set; }



        /// <summary>
        /// 作者Id
        /// </summary>
        [Required]
        public string UserId { get; set; }



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
        /// 发布时间
        /// </summary>
        [Required]
        public DateTime PublishTime { get; set; }


        /// <summary>
        /// 访问量
        /// </summary>
        public long VisitCount { get; set; }


        /// <summary>
        /// 文章是否置顶
        /// </summary>
        public bool IsSetTop  { get; set; }


        /// <summary>
        /// 置顶时间
        /// </summary>
        public DateTimeOffset SetTopDateTime  { get; set; }
    }
}
