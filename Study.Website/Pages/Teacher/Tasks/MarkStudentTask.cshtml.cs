using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Study.Services.TaskService.Req;
using Study.Services.TaskService.Res;
using UserGrpcService;
using Utility;

namespace Study.Website.Pages.Teacher.Tasks
{
    /// <summary>
    /// 批改学生作业
    /// </summary>
    [Authorize(Roles = ConstStrings.Role_Teacher)]
    public class MarkStudentTaskModel : AppPageModel
    {
        [FromRoute(Name = "StudentId")]
        public string StudentId { get; set; }

        [FromRoute(Name = "TaskId")]
        public string TaskId { get; set; }

        /// <summary>
        /// 学生信息
        /// </summary>
        public UserReply StudentInfo { get; set; }

        /// <summary>
        /// 学生作业信息
        /// </summary>
        public StudentTaskDto StudentTask { get; set; }

        /// <summary>
        /// 教师发布的这份作业信息
        /// </summary>
        public TeacherTaskDto TaskInfo { get; set; }




        /// <summary>
        /// 批改作业模型
        /// </summary>
        public SetStudentGradeDto setStudentGradeModel { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(StudentId) || string.IsNullOrEmpty(TaskId))
            {
                return BadRequest();
            }

            //根据作业id和学生id获取学生作业
            StudentTask = await TaskService.GetStudentTasks()
                      .FirstOrDefaultAsync(e => e.TaskId == TaskId && e.StudentId == StudentId).ConfigureAwait(false);


            if (StudentTask == null)
            {
                return NotFound();
            }
        
            //教师的作业信息
            TaskInfo = await TaskService.GetTeacherTaskByIdAsync(TaskId).ConfigureAwait(false);
            if(TaskInfo == null)
            {
                return NotFound();
            }

            //学生信息
            StudentInfo = await UserClient.FindByIdAsync(new UserIdReq { UserId = StudentId });
            if(StudentInfo == null)
            {
                return NotFound();
            }

            setStudentGradeModel = new SetStudentGradeDto
            {
                Grade = (int)StudentTask.Grade,
                StudentId = StudentId,
                TaskId = TaskId,
                TeacherReply = StudentTask.TeacherReply
            };

            return Page();
        }


        /// <summary>
        /// 设置分数
        /// </summary>
        /// <param name="setStudentGradeModel"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPost(SetStudentGradeDto setStudentGradeModel)
        {
            if (!TryValidateModel(setStudentGradeModel))
            {
                return BadRequest(ModelState);
            }

            var result = await TaskService.SetStudentGradeAsync(setStudentGradeModel).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return BadRequest();
            }

            return Content($"/Teacher/Tasks/TaskDetail/{setStudentGradeModel.TaskId}");
        }
    }
}