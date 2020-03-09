using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Study.Website.Pages.Teacher.Classes
{
    public class StudentsOfClassModel : AppPageModel
    {
        /// <summary>
        /// 班级id
        /// </summary>
        [FromRoute]
        public string ClassId  { get; set; }

        /// <summary>
        /// 根据班级id加载学生
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetLoadStudentsAsync()
        {
            var students = await UserClient.GetUsersByClassIdAsync(new UserGrpcService.UserNameReq 
            { 
                UserName = ClassId 
            });

            return new JsonResult(students.Users);
        }
    }
}