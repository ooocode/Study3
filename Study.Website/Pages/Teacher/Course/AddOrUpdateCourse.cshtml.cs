using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Study.Service.CourseService.Req;
using Study.Service.CourseService.Res;
using Study.Services.CourseService;
using Study.Services.UserService;
using Study.WebApp.Data;
using Utility;

namespace Study.Website.Pages.Teacher.Course
{
    [Authorize(Policy = Permissons.Permisson.Teacher_Course_CRUD)]
    public class AddOrUpdateCourseModel : AppPageModel
    {

        public AddOrUpdateCourseDto AddOrUpdateCourseDto { get; set; }

        public async Task<IActionResult> OnGetAsync([ModelBinder(Name = "CourseId")]CourseDto course)
        {
            if (course != null)
            {
                AddOrUpdateCourseDto = new AddOrUpdateCourseDto
                {
                    Desc = course.Desc,
                    Id = course.Id,
                    Name = course.Name,
                    UserId = course.UserId
                };
            }
            else
            {
                AddOrUpdateCourseDto = new AddOrUpdateCourseDto
                {
                    UserId = CurUserId
                };
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(AddOrUpdateCourseDto AddOrUpdateCourseDto)
        {
            if (!TryValidateModel(AddOrUpdateCourseDto))
            {
                return BadRequest(ModelState);
            }

            var result = await CourseService.AddOrUpdateCourseAsync(AddOrUpdateCourseDto).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return BadRequest(ModelState);
            }

            return Content("/Teacher/Course/CourseList");
        }
    }
}