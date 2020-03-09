using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Service.UserService.Req
{
    public class UpdatePasswordDto
    {
        [Required(ErrorMessage = "密码不能为空")]
        [StringLength(20,MinimumLength = 6,ErrorMessage = "密码长度6--20位")]
        [Compare("ConfirmPassword",ErrorMessage = "两次输入的密码不一致")]
        public string NewPassword  { get; set; }


        /// <summary>
        /// 确认密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度6--20位")]
        public string ConfirmPassword { get; set; }
    }
}
