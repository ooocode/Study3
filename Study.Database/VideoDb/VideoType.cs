using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Study.Database.VideoDb
{
    /// <summary>
    /// 视频类型
    /// </summary>
    public class VideoType
    {
        [Key]
        public int Id  { get; set; }


        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 分类是否在页面上显示
        /// </summary>
        public bool ShowInPage  { get; set; }


        /// <summary>
        /// 父类型Id
        /// </summary>
        public int? ParentTypeId  { get; set; }
    }
}
