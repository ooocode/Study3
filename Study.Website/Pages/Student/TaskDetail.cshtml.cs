using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Study.Service.CourseService.Res;
using Study.Services.CourseService;
using Study.Services.TaskService;
using Study.Services.TaskService.Req;
using Study.Services.TaskService.Res;
using Study.Services.UserService;
using Study.WebApp.Data;
using Utility;

namespace Study.Website.Pages.Student
{
    [Authorize(Roles = ConstStrings.Role_Student)]
    public class TaskDetailModel : AppPageModel
    {
        [FromRoute]
        public string TaskId { get; set; }

        /// <summary>
        /// 学生的作业信息
        /// </summary>
        public StudentTaskDto StudentTaskInfo { get; set; }

        /// <summary>
        /// 作业信息
        /// </summary>
        public TeacherTaskDto TaskInfo  { get; set; }


        /// <summary>
        /// 课程信息
        /// </summary>
        public CourseDto CourseInfo  { get; set; }

        /// <summary>
        /// 答题模型
        /// </summary>
        public SetStudentAnswerDto setStudentAnswerModel { get; set; }

        public List<dynamic> Files  { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(TaskId))
            {
                return BadRequest();
            }


            //根据作业id和学生id获取作业
            StudentTaskInfo = await TaskService.GetStudentTasks()
                .FirstOrDefaultAsync(e => e.TaskId == TaskId
                && e.StudentId == CurUserId).ConfigureAwait(false);

            if (StudentTaskInfo == null)
            {
                return NotFound();
            }

            TaskInfo = await TaskService.GetTeacherTaskByIdAsync(TaskId);
            if(TaskInfo == null)
            {
                return NotFound();
            }

            CourseInfo = await CourseService.GetCourseByIdAsync(TaskInfo.CourseId);
            if(CourseInfo == null)
            {
                return NotFound();
            }

            //学生答题
            setStudentAnswerModel = new SetStudentAnswerDto
            {
                TaskId = TaskId,
                StudentId = CurUserId,
                Answer = StudentTaskInfo.StudentAnswer
            };

            if (!string.IsNullOrEmpty(TaskInfo.Files))
            {
                Files = Newtonsoft.Json.JsonConvert.DeserializeObject<List<dynamic>>(TaskInfo.Files);
            }
           
            return Page();
        }

        //答题
        public async Task<IActionResult> OnPostAsync(SetStudentAnswerDto setStudentAnswerModel)
        {
            if (!TryValidateModel(setStudentAnswerModel))
            {
                return BadRequest(ModelState);
            }

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(setStudentAnswerModel.Answer);
            var imgs = htmlDoc.DocumentNode.SelectNodes("//img");
            if (imgs != null)
            {
                foreach (var img in imgs)
                {
                    img.AddClass("img-fluid d-block mx-auto  img-responsive center-block");
                    img.SetAttributeValue("style", "width:90%");
                }
                setStudentAnswerModel.Answer = htmlDoc.DocumentNode.OuterHtml;
            }


            var result = await TaskService.SetStudentAnswerAsync(setStudentAnswerModel);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return BadRequest(ModelState);
            }

            return Content("OK");
        }
    }
}