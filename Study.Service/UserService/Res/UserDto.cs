using Study.Service.UserService.Res;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.Services.UserService.Res
{
    public class UserDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }


        /// <summary>
        /// 账号（是唯一的）
        /// </summary>
        public string Account { get; set; }


        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; set; }


        /// <summary>
        /// 手机号码是否验证了
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }


        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }


        /// <summary>
        /// 归一化的邮箱（大写的邮箱）
        /// </summary>
        public string NormalizedEmail { get; set; }


        /// <summary>
        /// 邮箱是否被验证了
        /// </summary>
        public bool EmailConfirmed { get; set; }


        /// <summary>
        /// 登录失败次数
        /// </summary>
        public int AccessFailedCount { get; set; }


        /// <summary>
        /// 账号是否被锁定了（封号）
        /// </summary>
        public bool LockoutEnabled { get; set; }



        /// <summary>
        /// 封号结束时间
        /// </summary>
        public DateTimeOffset? LockoutEnd { get; set; }


        /// <summary>
        /// 头像
        /// </summary>
        public string Photo { get; set; }



        /// <summary>
        /// 性别
        /// </summary>
        public byte Sex { get; set; }


        /// <summary>
        /// 个人简介
        /// </summary>
        public string Desc { get; set; }


        /// <summary>
        /// 班级信息
        /// </summary>
        public SchoolClassDto ClassInfo { get; set; }
    }
}
