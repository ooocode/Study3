using Study.Services.UserService.Res;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.Services.ArticalService.Res
{
    public class ArticalDto
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        public long Id { get; set; }


        /// <summary>
        ///分类Id
        /// </summary>
        public long ClassificationId { get; set; }



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
        public long VisitCount { get; set; }


        /// <summary>
        /// 文章是否置顶
        /// </summary>
        public bool IsSetTop { get; set; }


        /// <summary>
        /// 置顶时间
        /// </summary>
        public DateTimeOffset SetTopDateTime { get; set; }
    }
}
