using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Study.Database.Entity.UserEntities;
using Study.Service.UserService.Res;
using Study.Services.UserService;
using Study.WebApp;
using Study.WebApp.Data;
using Utility;

namespace Study.Website.Pages.Teacher
{
    [Authorize(Policy = Permissons.Permisson.Teacher_Classes_CRUD)]
    [IgnoreAntiforgeryToken]
    public class AddTeacherClassModel : AppPageModel
    {
        /// <summary>
        /// 学校全部班级
        /// </summary>
        public IEnumerable<SchoolClassDto> SchoolClasses { get; set; }

        public async Task OnGetAsync()
        {

        }


        /// <summary>
        /// 添加教师班级
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>

        public async Task<IActionResult> OnPostAddTeacherClassAsync(
            [FromBody]Study.Services.UserService.Req.AddTeacherClassDto addTeacherClassModel)
        {

            addTeacherClassModel.TeacherId = CurUserId;
            if (!TryValidateModel(addTeacherClassModel))
            {
                return BadRequest(ModelState);
            }

            var exist = await UserDatabaseContext.TeacherClasses
                .AnyAsync(e => e.ClassId == addTeacherClassModel.ClassId && e.TeacherId == CurUserId)
                .ConfigureAwait(false);

            if (exist)
            {
                AddModelError("已经存在班级");
                return BadRequest(ModelState);
            }

            await UserDatabaseContext.TeacherClasses.AddAsync(new TeacherClass
            {
                TeacherId = addTeacherClassModel.TeacherId,
                ClassId = addTeacherClassModel.ClassId
            });

            await UserDatabaseContext.SaveChangesAsync().ConfigureAwait(false);


            return new OkResult();
        }
    }
}