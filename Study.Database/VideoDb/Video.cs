using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Study.Database.VideoDb
{
    public class Video
    {
        /// <summary>
        /// 视频id
        /// </summary>
        [Key]
        public int Id { get; set; }


        /// <summary>
        /// 视频类型Id
        /// </summary>
        [Required]
        public int VideoTypeId { get; set; }



        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTimeOffset? LastUpdateTime { get; set; }

        /// <summary>
        /// 视频名称
        /// </summary>
        [Required]
        public string Name { get; set; }



        /// <summary>
        /// 图片路径
        /// </summary>
        public string Picture { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        public string Lang { get; set; }


        /// <summary>
        /// 地区
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// 状态 不知道什么东西
        /// </summary>
        public int State { get; set; }


        /// <summary>
        /// 说明（更新到第几集  、已完结）
        /// </summary>
        public string Note { get; set; }


        /// <summary>
        /// 主演
        /// </summary>
        public string Actor { get; set; }


        /// <summary>
        /// 导演
        /// </summary>
        public string Director { get; set; }


        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }


        /// <summary>
        /// 是否是否在页面上显示
        /// </summary>
        public bool ShowInPage { get; set; }
    }
}
