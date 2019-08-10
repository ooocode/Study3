using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Study.Database.Entity.ArticalEntities
{
    public class Artical
    {
        /// <summary>
        /// 文章GUID
        /// </summary>
        [Key]
        public string Id { get; set; }


        /// <summary>
        ///分类Id
        /// </summary>
        [Required]
        public string ClassificationId { get; set; }



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
    }
}
