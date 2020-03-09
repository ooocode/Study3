using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Services.UserService.Req
{
    public class AddSchoolClassDto
    {
        /// <summary>
        /// 班级名称
        /// </summary>
        [Required]
        public string Name { get; set; }


        /// <summary>
        /// 班级描述
        /// </summary>
        public string Desc { get; set; }
    }
}
