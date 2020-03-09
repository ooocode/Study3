using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Study.Database;
using Study.Services.UserService.Req;
using Study.Website.Pages;
using Z.EntityFramework.Plus;

namespace Study.Website.Pages.Teacher.Classes
{
    [IgnoreAntiforgeryToken]
    public class SchoolClassesModel : AppPageModel
    {
        public void OnGet()
        {

        }

        /// <summary>
        /// 从第一页开始
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetLoadSchoolClassesAsync(string name, int? pageIndex)
        {


            if (!pageIndex.HasValue || pageIndex.Value <= 0)
            {
                pageIndex = 1;
            }

            if (string.IsNullOrEmpty(name))
            {
                name = string.Empty;
            }

            int take = 10;
            //var rows = await schoolClass.Skip()
            //    .Take(take)
            //    .ToListAsync()
            //    .ConfigureAwait(false);

            var classes = await UserClient.GetSchoolClassesAsync(new UserGrpcService.SchoolClassesReq
            {
                QueryName = name,
                Skip = (uint)((pageIndex.Value - 1) * take),
                Take = (uint)take
            });


            //var teacherClasses = await UserService.(CurUserId).ToListAsync().ConfigureAwait(false);
            var teacherClasses = await UserDatabaseContext.TeacherClasses
                .Where(e => e.TeacherId == CurUserId)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            return new JsonResult(new
            {
                Total = classes.Total,
                Rows = classes.SchoolClasses.Select(e =>
                    new
                    {
                        e.Id,
                        e.Name,
                        e.Desc,
                        IsAdded = teacherClasses.Any(ee => ee.ClassId == e.Id)
                    }),
                PageSize = take
            });
        }


        /// <summary>
        /// 添加教师班级
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>

        public async Task<IActionResult> OnPostAddTeacherClassAsync([FromBody]AddTeacherClassDto dto)
        {
            if (!TryValidateModel(dto))
            {
                return BadRequest(ModelState);
            }

            var exist = await UserDatabaseContext.TeacherClasses
                .AnyAsync(e => e.TeacherId == CurUserId && e.ClassId == dto.ClassId)
                .ConfigureAwait(false);
            if (exist)
            {
                ModelState.AddModelError(string.Empty, "重复添加");
                return BadRequest(ModelState);
            }

            await UserDatabaseContext.TeacherClasses.AddAsync(new Database.Entity.UserEntities.TeacherClass
            {
                TeacherId = CurUserId,
                ClassId = dto.ClassId
            });


            int rows = await UserDatabaseContext.SaveChangesAsync().ConfigureAwait(false);

            if (rows == 0)
            {
                ModelState.AddModelError(string.Empty, "没有添加任何记录");
                return BadRequest(ModelState);
            }

            return new OkResult();
        }
    }
}