using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Database.VideoDb
{
    /// <summary>
    /// 视频播放链接
    /// </summary>
    public class VideoUrl
    {
        /// <summary>
        /// 视频id  [key]
        /// </summary>
        [Key]
        public int VideoId  { get; set; }


        /// <summary>
        /// 播放器标识 [key]
        /// </summary>
        [Key]
        public string  Flag { get; set; }

        /// <summary>
        /// 视频链接
        /// </summary>
        [Required]
        public string Url  { get; set; }
    }
}
