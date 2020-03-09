using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Services.ArticalService.Req
{
    /// <summary>
    /// 更新文章分类
    /// </summary>
    public class UpdateArticalClassificationDto
    {
        [Required]
        public long Id  { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "最大长度30")]
        public string  Name { get; set; }
    }
}
