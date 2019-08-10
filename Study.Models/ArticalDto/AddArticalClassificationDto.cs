using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Dto.ArticalDto
{
    public class AddArticalClassificationDto
    {
        /// <summary>
        ///分类名称
        /// </summary>
        [Required]
        public string Name { get; set; }


        /// <summary>
        /// 添加时间
        /// </summary>
        [Required]
        public DateTime AddDateTime { get; set; }
    }
}
