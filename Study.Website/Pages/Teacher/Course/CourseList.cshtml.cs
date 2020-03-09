using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Study.Website.Pages.Teacher.Course
{
    public class CourseListModel : AppPageModel
    {
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnGetLoadCoursesAsync()
        {
            var myCourses = await CourseService.GetCourses()
                .Where(e => e.UserId == CurUserId)
                .OrderBy(e => e.Name)
                .ToListAsync().ConfigureAwait(false);

            var result = myCourses.Select(e => new
            {
                AddDateTime = e.AddDateTime.ToString("yyyy年MM月dd HH时mm分"),
                Desc = e.Desc,
                Id = e.Id,
                LastModifyDatetime = e.LastModifyDatetime.ToString("yyyy年MM月dd HH时mm分"),
                Name = e.Name,
                UserId = e.UserId
            });

            return new JsonResult(result);
        }
    }
}