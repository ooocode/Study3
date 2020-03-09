using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.Service.CourseService.Res
{
    [ModelBinder(BinderType = typeof(CourseModelBinder))]
    public class CourseDto
    {
        /// <summary>
        /// 课程id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }


        /// <summary>
        /// 课程名称
        /// </summary>
        public string Name { get; set; }



        /// <summary>
        /// 课程描述
        /// </summary>
        public string Desc { get; set; }



        /// <summary>
        /// 添加时间（这个课程是什么时候添加的）
        /// </summary>
        public DateTime AddDateTime { get; set; }


        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastModifyDatetime { get; set; }
    }
}
