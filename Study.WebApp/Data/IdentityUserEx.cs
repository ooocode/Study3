using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Study.WebApp.Data
{
    public class IdentityUserEx : IdentityUser
    {
        /// <summary>
        /// 头像
        /// </summary>
        [PersonalData]
        public string Photo { get; set; }


        /// <summary>
        /// 姓名
        /// </summary>
        [PersonalData]
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [PersonalData]
        public byte Sex { get; set; }


        /// <summary>
        /// 个人简介
        /// </summary>
        [PersonalData]
        public string Desc { get; set; }
    }
}
