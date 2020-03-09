using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Study.Website.Pages.Teacher.Classes
{
    [IgnoreAntiforgeryToken]
    public class ListModel : AppPageModel
    {
        public void OnGet()
        {

        }

        /// <summary>
        /// 加载教师班级
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetLoadClassesAsync()
        {
            var classes = await UserDatabaseContext.TeacherClasses
                .Where(e => e.TeacherId == CurUserId)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            async IAsyncEnumerable<UserGrpcService.SchoolClassReply> getClasses()
            {
                foreach (var item in classes)
                {
                    yield return await UserClient
                        .GetSchoolClassByIdAsync(new UserGrpcService.IdReq { Id = item.ClassId });
                }
            };
      
            return new JsonResult(getClasses());
        }

        /// <summary>
        /// 删除教师班级
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostDeleteTeacherClassAsync([FromBody]DeleteTeacherClassViewModel model)
        {
            if (!TryValidateModel(model))
            {
                return BadRequest(ModelState);
            }

            var entity = await UserDatabaseContext.TeacherClasses
                .FirstOrDefaultAsync(e => e.ClassId == model.ClassId && e.TeacherId == CurUserId).ConfigureAwait(false);
            if(entity == null)
            {
                return NotFound();
            }

            var classInfo = await UserClient.GetSchoolClassByIdAsync(new UserGrpcService.IdReq { Id = entity.ClassId });
            if (!classInfo.Name.Equals(model.ClassName))
            {
                AddModelError("不正确的班级名称输入");
                return BadRequest(ModelState);
            }

            UserDatabaseContext.TeacherClasses.Remove(entity);
            await UserDatabaseContext.SaveChangesAsync().ConfigureAwait(false);

            return new OkResult();
        }


        public class DeleteTeacherClassViewModel
        {
            [Required]
            public string ClassId { get; set; }

            [Required]
            public string ClassName { get; set; }
        }
    }
}