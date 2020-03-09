using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Services.ArticalService.Req
{
    public class AddArticalClassificationDto
    {
        /// <summary>
        ///分类名称
        /// </summary>
        [Required]
        [StringLength(30,ErrorMessage = "最大长度30")]
        public string Name { get; set; }
    }
}
