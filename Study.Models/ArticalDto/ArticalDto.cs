using System;
using System.Collections.Generic;
using System.Text;

namespace Study.Dto.ArticalDto
{
    public class ArticalDto
    {
        /// <summary>
        /// 文章GUID
        /// </summary>
        public string Id { get; set; }


        /// <summary>
        ///分类Id
        /// </summary>
        public string ClassificationId { get; set; }



        /// <summary>
        /// 作者Id
        /// </summary>
        public string UserId { get; set; }



        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// 文章内容
        /// </summary>
        public string Content { get; set; }


        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime PublishTime { get; set; }


        /// <summary>
        /// 访问量
        /// </summary>
        public long VisitCount  { get; set; }
    }
}
