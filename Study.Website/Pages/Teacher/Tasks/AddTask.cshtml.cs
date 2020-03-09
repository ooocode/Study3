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
using Study.Services.UserService;
using Study.Services.UserService.Res;
using Study.WebApp.Data;
using Study.Website.Pages;
using Utility;

namespace Study.WebApp.Pages.Teacher
{
    [Authorize(Policy = Permissons.Permisson.Teacher_Tasks_Create)]
    public class AddTaskModel : AppPageModel
    {
        /// <summary>
        /// 作业id 如果id不为空则是修改
        /// </summary>
        [FromRoute(Name = "TaskId")]
        public string TaskId { get; set; }

        /// <summary>
        /// 添加任务模型
        /// </summary>
        public AddOrUpdateTeacherTaskDto AddOrUpdateTeacherTaskDto { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 教师已经添加的的班级
        /// </summary>
        public List<string> TeacherClasses { get; set; }


        public List<UserDto> OthorTeacher { get; set; }

        /// <summary>
        /// 教师的课程
        /// </summary>
        public List<CourseDto> TeacherCourses { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            //表明这是新建的
            if (string.IsNullOrEmpty(TaskId))
            {
                var now = DateTime.Now.ToString();

                AddOrUpdateTeacherTaskDto = new AddOrUpdateTeacherTaskDto
                {
                    TaskStartTime = now,
                    TaskEndTime = now
                };
            }
            else
            {
                var taskInfo = await TaskService.GetTeacherTaskByIdAsync(TaskId).ConfigureAwait(false);
                if (taskInfo == null)
                {
                    return NotFound();
                }

                AddOrUpdateTeacherTaskDto = new AddOrUpdateTeacherTaskDto
                {
                    CourseId = taskInfo.CourseId,
                    HelperId = taskInfo.HelperId,
                    TaskContent = taskInfo.TaskContent,
                    TaskEndTime = taskInfo.TaskEndTime.ToString(),
                    TaskName = taskInfo.TaskName,
                    TaskStartTime = taskInfo.TaskStartTime.ToString(),
                    TaskId = taskInfo.Id,
                    Files = taskInfo.Files
                };
            }


            //获取教师的课程
            TeacherCourses = await CourseService.GetCourses()
                .Where(e => e.UserId == CurUserId)
                .ToListAsync()
                .ConfigureAwait(false);


            //获取教师的班级
            TeacherClasses = await UserDatabaseContext.TeacherClasses.Where(e => e.TeacherId == CurUserId)
                .Select(e => e.ClassId)
                .ToListAsync()
                .ConfigureAwait(false);

            return Page();
        }


        /// <summary>
        /// 发布作业
        /// </summary>
        /// <param name="addTeacherTaskModel"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPost(AddOrUpdateTeacherTaskDto AddOrUpdateTeacherTaskDto)
        {
            if (!TryValidateModel(AddOrUpdateTeacherTaskDto))
            {
                return Page();
            }


            //新建的时候才能选择学生
            if (string.IsNullOrEmpty(AddOrUpdateTeacherTaskDto.TaskId) && AddOrUpdateTeacherTaskDto.StudentIds != null)
            {
                List<string> studentIds = new List<string>();
                //StudentIds存放的是班级名称  曲线救国啊  本来是学生id的

                var classNames = AddOrUpdateTeacherTaskDto.StudentIds.FirstOrDefault()?.Split(',');
                if (classNames == null || classNames.Length == 0)
                {
                    ModelState.AddModelError(string.Empty, "没有选择班级");
                    return BadRequest(ModelState);
                }

                foreach (var className in classNames)
                {
                    if (!string.IsNullOrEmpty(className))
                    {
                        var schoolclass = await UserClient.GetSchoolClassByNameAsync(new UserGrpcService.NameReq { Name = className });
                        if (schoolclass != null)
                        {
                            var result = await UserClient.GetUsersByClassIdAsync(new UserGrpcService.UserNameReq
                            {
                                UserName = schoolclass.Id
                            });
                            studentIds.AddRange(result.Users.Select(e => e.Id));
                        }
                    }
                }

                AddOrUpdateTeacherTaskDto.StudentIds = studentIds;
            }

            AddOrUpdateTeacherTaskDto = ModifyContent(AddOrUpdateTeacherTaskDto);
            var res = await TaskService.AddOrUpdateTeacherTaskAsync(AddOrUpdateTeacherTaskDto).ConfigureAwait(false);
            if (res.Succeeded)
            {
                return Content($"/Teacher/Tasks/TaskDetail/{res.Data}");
            }
            else
            {
                ModelState.AddModelError("", res.ErrorMessage);
                return BadRequest(ModelState);
            }
        }


        /// <summary>
        /// 内容添加图片自适应
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private AddOrUpdateTeacherTaskDto ModifyContent(AddOrUpdateTeacherTaskDto dto)
        {
            if (!string.IsNullOrEmpty(dto.TaskContent))
            {
                //给图片添加响应式(同时支持bootstrap3 和 4 ！！！) 4 == img-fluid d-block mx-auto   3==img-responsive center-block
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(dto.TaskContent);
                var imgs = htmlDoc.DocumentNode.SelectNodes("//img");
                if (imgs != null)
                {
                    foreach (var img in imgs)
                    {
                        img.AddClass("img-fluid d-block mx-auto  img-responsive center-block");
                        img.SetAttributeValue("style", "width:90%");
                    }
                    dto.TaskContent = htmlDoc.DocumentNode.OuterHtml;
                }
            }

            return dto;
        }
    }
}