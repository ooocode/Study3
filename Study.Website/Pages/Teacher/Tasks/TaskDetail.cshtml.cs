using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Study.Service.CourseService.Res;
using Study.Services.CourseService;
using Study.Services.TaskService;
using Study.Services.TaskService.Res;
using Study.Services.UserService;
using Study.WebApp.Data;
using Utility;

namespace Study.Website.Pages.Teacher
{
    public class StudentTaskViewModel
    {
        public string StudentId { get; set; }

        public bool IsAlreadyAnswered { get; set; }

        public bool IsTeacherModified { get; set; }

        public decimal Grade { get; set; }
    }


    [Authorize(Roles = ConstStrings.Role_Teacher)]
    public class TaskDetailModel : AppPageModel
    {
        [FromRoute(Name = "TaskId")]
        public string TaskId { get; set; }



        /// <summary>
        /// 作业信息
        /// </summary>
        public TeacherTaskDto TaskInfo { get; set; }

        /// <summary>
        /// 课程信息
        /// </summary>
        public CourseDto CourseInfo { get; set; }

        /// <summary>
        /// 完成未批改
        /// </summary>
        public IEnumerable<StudentTaskViewModel> CompletedNoMark { get; set; }


        /// <summary>
        /// 未完成
        /// </summary>
        public IEnumerable<StudentTaskViewModel> UnCompleted { get; set; }


        /// <summary>
        /// 已完成已批改
        /// </summary>
        public IEnumerable<StudentTaskViewModel> CompletedMark { get; set; }



        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(TaskId))
            {
                return BadRequest();
            }

            TaskInfo = await TaskService.GetTeacherTaskByIdAsync(TaskId).ConfigureAwait(false);
            if (TaskInfo == null)
            {
                return NotFound();
            }

            CourseInfo = await CourseService.GetCourseByIdAsync(TaskInfo.CourseId).ConfigureAwait(false);
            if (CourseInfo == null)
            {
                return NotFound();
            }



            //获取这个作业任务下的所有学生作业情况
            var StudentTasks = await TaskService.GetStudentTasks().Where(e => e.TaskId == TaskId).Select(e =>
                new StudentTaskViewModel
                {
                    StudentId = e.StudentId,
                    IsAlreadyAnswered = e.IsAlreadyAnswered,
                    IsTeacherModified = e.IsTeacherModified,
                    Grade = e.Grade
                }).AsNoTracking().ToListAsync().ConfigureAwait(false);

            //完成未批改
            CompletedNoMark =  StudentTasks.Where(e => e.IsAlreadyAnswered && !e.IsTeacherModified);

            //未完成
            UnCompleted =  StudentTasks.Where(e => !e.IsAlreadyAnswered);

            //已完成已批改
            CompletedMark =  StudentTasks.Where(e => e.IsAlreadyAnswered && e.IsTeacherModified);

            return Page();
        }

        /// <summary>
        /// 删除教师作业
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostDeleteTaskAsync(string taskId)
        {
            if (string.IsNullOrEmpty(taskId))
            {
                return BadRequest(ModelState);
            }

            var result = await TaskService.DeleteTaskAsync(CurUserId, taskId).ConfigureAwait(false);
            if (result.Succeeded)
            {
                return Content("/Teacher/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetExportDataAsync(string taskId)
        {
            //获取教师作业
            var teacherTask = await TaskService.GetTeacherTaskByIdAsync(taskId).ConfigureAwait(false);
            if (teacherTask == null)
            {
                return NotFound();
            }

            using DataTable dataTable = new DataTable("Users");

            dataTable.Columns.Add("班级", typeof(string));
            dataTable.Columns.Add("学号", typeof(string));
            dataTable.Columns.Add("姓名", typeof(string));
            dataTable.Columns.Add("成绩", typeof(int));
            dataTable.Columns.Add("答题时间", typeof(string));

            //获取学生作业
            var studentsTasks = await TaskService.GetStudentTasks()
                .Where(e => e.TaskId == taskId)
                .ToListAsync()
                .ConfigureAwait(false);

            foreach (var studentTask in studentsTasks)
            {
                var row = dataTable.NewRow();
                var studentInfo = await UserClient.FindByIdAsync(new UserGrpcService.UserIdReq { UserId = studentTask.StudentId });

                var classInfo = await UserClient.GetSchoolClassByIdAsync(new UserGrpcService.IdReq { Id = studentInfo.ClassId });

                row["班级"] = classInfo.Name;
                row["学号"] = studentInfo.UserName;
                row["姓名"] = studentInfo.Name;
                row["成绩"] = (int)studentTask.Grade;
                row["答题时间"] = studentTask.DateTime == DateTime.MinValue ?
                    "未完成" : studentTask.DateTime.ToString("yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);

                dataTable.Rows.Add(row);
            }

            using var newDataTable = dataTable.Rows.Cast<DataRow>()
                .OrderBy(e => e["班级"])
                .ThenBy(e => e["学号"])
                .CopyToDataTable();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(teacherTask.TaskName);
                var excelBase = worksheet.Cells["A1"].LoadFromDataTable(newDataTable, PrintHeaders: true);

                //for (var row = 1; row <= excelBase.Rows; row++)
                //{
                //    var rowCollection = dataTable.Rows[row];

                //    if (!rowCollection.IsNull("答题时间") && rowCollection["答题时间"].ToString() == "未完成")
                //    {
                //        worksheet.Row(row).Style.Font.Color.SetColor(System.Drawing.Color.Red);
                //    }
                //}

                worksheet.Cells.Where(e => e.Text.Contains("未完成")).ToList()
                    .ForEach(e =>
                    {
                        worksheet.Row(e.End.Row).Style.Font.Color.SetColor(System.Drawing.Color.Red);
                        //e.Style.Font.Color.SetColor(System.Drawing.Color.Red);
                    });

                for (var col = 1; col < newDataTable.Columns.Count + 1; col++)
                {
                    worksheet.Column(col).AutoFit();
                }

                return File(package.GetAsByteArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: $"{teacherTask.TaskName}.xlsx");
            }
        }
    }
}