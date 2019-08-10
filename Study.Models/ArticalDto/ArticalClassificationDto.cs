using System;
using System.Collections.Generic;
using System.Text;

namespace Study.Dto.ArticalDto
{
    public class ArticalClassificationDto
    {
        /// <summary>
        /// 分类ID
        /// </summary>

        public string Id { get; set; }


        /// <summary>
        ///分类名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDateTime { get; set; }
    }
}
