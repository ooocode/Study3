using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Services.UserService.Req
{
    public class UpdateDto
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Required(ErrorMessage = "姓名不能为空")]
        [StringLength(10,MinimumLength = 3,ErrorMessage = "姓名必须是3--10个字符")]
        public string Name { get; set; }


        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; set; }


        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public byte Sex { get; set; }


        /// <summary>
        /// 个人简介
        /// </summary>
        public string Desc { get; set; }


        /// <summary>
        /// 班级id
        /// </summary>
        public string ClassId { get; set; }
    }
}
