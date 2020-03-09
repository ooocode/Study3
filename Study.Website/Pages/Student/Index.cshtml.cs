using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Study.Services.TaskService;
using Study.Services.TaskService.Res;
using Study.Services.UserService;
using Study.WebApp.Data;
using Utility;

namespace Study.Website.Pages.Student
{
    [Authorize(Roles = ConstStrings.Role_Student)]
    public class IndexModel : AppPageModel
    {
        private readonly IUserService userService;
        private readonly ITaskService taskService;

        public IndexModel(IUserService userService,ITaskService taskService)
        {
            this.userService = userService;
            this.taskService = taskService;
        }

        /// <summary>
        /// 已完成的作业列表
        /// </summary>
        public IEnumerable<StudentTaskDto> CompletedTaskList { get; set; }


        /// <summary>
        /// 未完成的作业列表
        /// </summary>
        public IEnumerable<StudentTaskDto> UnCompletedTaskList { get; set; }



        public async Task<IActionResult> OnGetAsync()
        {
            //获取我的作业列表
            var taskList = await taskService.GetStudentTasks().Where(e => e.StudentId == CurUserId).ToListAsync().ConfigureAwait(false);

            CompletedTaskList = taskList.Where(e => e.IsAlreadyAnswered == true);
            UnCompletedTaskList = taskList.Where(e => e.IsAlreadyAnswered == false);

            return Page();
        }
    }
}