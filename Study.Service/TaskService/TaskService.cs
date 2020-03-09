using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Study.Database;
using Study.Database.Entity.TaskEntities;
using Study.Service;
using Study.Services.CourseService;
using Study.Services.TaskService.Req;
using Study.Services.TaskService.Res;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace Study.Services.TaskService
{
    /// <summary>
    /// 作业服务
    /// </summary>
    public class TaskService : ServiceBase, ITaskService
    {
        private readonly UserDatabaseContext context;
        //private readonly IUserService userService;
        private readonly ICourseService courseService;

        public TaskService(UserDatabaseContext context,
                           ICourseService courseService,
                           IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            this.context = context;
            this.courseService = courseService;
        }


        /// <summary>
        /// 添加或者更新教师任务同时发布给学生  
        /// </summary>
        public async Task<Result<string>> AddOrUpdateTeacherTaskAsync(AddOrUpdateTeacherTaskDto dto)
        {
            Result<string> response = new Result<string>();

            do
            {
                DateTime startDateTime = DateTime.Parse(dto.TaskStartTime);
                DateTime endDateTime = DateTime.Parse(dto.TaskEndTime);
                if (startDateTime >= endDateTime)
                {
                    response.ErrorMessage = "输入时间错误，开始时间不能快于结束时间";
                    break;
                }

                if (endDateTime <= DateTime.Now)
                {
                    response.ErrorMessage = "输入时间错误，结束时间不能早于现在";
                    break;
                }



                if (string.IsNullOrEmpty(dto.TaskId))
                {
                    //判断是否存在这个任务，不存在就添加
                    //教师下面的课程下的作业不能同名
                    var existTask = await context.TeacherTasks.AnyAsync(e => e.TeacherId == CurUserId
                                                    && e.CourseId == dto.CourseId
                                                    && e.TaskName == dto.TaskName);
                    if (existTask)
                    {
                        response.ErrorMessage = "任务名称已经存在";
                        break;
                    }

                    if (dto.StudentIds == null || dto.StudentIds.Count == 0)
                    {
                        response.ErrorMessage = "没有选择学生或班级";
                        break;
                    }

                    string taskId = Guid.NewGuid().ToString();

                    var entity = new TeacherTask {
                        Id = taskId,
                        CourseId = dto.CourseId,
                        TeacherId = CurUserId,
                        TaskName = dto.TaskName.Trim(),
                        TaskContent = dto.TaskContent.Trim(),
                        TaskWriteTime = DateTime.Now,
                        TaskStartTime = startDateTime,
                        TaskEndTime = endDateTime,
                        HelperId = dto.HelperId,
                        Files = dto.Files
                    };

                    await context.TeacherTasks.AddAsync(entity);

                    //任务推送给学生
                    foreach (var sid in dto.StudentIds)
                    {
                        await context.StudentTasks.AddAsync(new StudentTask {
                            TaskId = taskId,
                            StudentId = sid,
                            Grade = 0,
                            StudentAnswer = "",
                            DateTime = DateTime.MinValue,
                            IsAlreadyAnswered = false,
                            IsTeacherModified = false,
                            TeacherReply = "",
                        });
                    }
                    response.Data = taskId;
                }
                else
                {
                    var task = await context.TeacherTasks.FirstOrDefaultAsync(e => e.Id == dto.TaskId && e.TeacherId == CurUserId);
                    if (task == null)
                    {
                        response.ErrorMessage = "作业不存在，无法修改";
                        break;
                    }

                    task.TaskContent = dto.TaskContent;
                    task.TaskName = dto.TaskName;
                    task.CourseId = dto.CourseId;
                    task.HelperId = dto.HelperId;
                    task.TaskStartTime = startDateTime;
                    task.TaskEndTime = endDateTime;
                    task.Files = dto.Files;

                    response.Data = task.Id;
                }

                try
                {
                    await context.SaveChangesAsync();
                    response.Succeeded = true;
                }
                catch (Exception ex)
                {
                    response.ErrorMessage = ex.GetEntityFrameworkErrorMsg();
                }
            } while (false);

            return response;
        }


        /// <summary>
        /// 获取教师发布的作业
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public IQueryable<TeacherTaskDto> GetTeacherTasks()
        {
            return context.TeacherTasks.Select(e => new TeacherTaskDto {
                CourseId = e.CourseId,
                HelperId = e.HelperId,
                Id = e.Id,
                TaskContent = e.TaskContent,
                TaskEndTime = e.TaskEndTime,
                TaskName = e.TaskName,
                TaskStartTime = e.TaskStartTime,
                TaskWriteTime = e.TaskWriteTime,
                TeacherId = e.TeacherId,
                Files = e.Files
            });
        }


        /// <summary>
        /// 通过id获取教师发布的作业
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task<TeacherTaskDto> GetTeacherTaskByIdAsync(string taskId)
        {
            return await GetTeacherTasks().FirstOrDefaultAsync(e => e.Id == taskId);
        }


        /// <summary>
        /// 获取所有学生的所有作业情况
        /// </summary>
        /// <returns></returns>
        public IQueryable<StudentTaskDto> GetStudentTasks()
        {
            return context.StudentTasks.Select(e => new StudentTaskDto {
                DateTime = e.DateTime,
                Grade = e.Grade,
                IsAlreadyAnswered = e.IsAlreadyAnswered,
                IsTeacherModified = e.IsTeacherModified,
                StudentAnswer = e.StudentAnswer,
                StudentId = e.StudentId,
                TaskId = e.TaskId,
                TeacherReply = e.TeacherReply
            });
            //return context.StudentTasks.Select(studentTask => mapper.Map<StudentTaskDto>(studentTask));
        }

        /// <summary>
        /// 学生答题
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="taskAnswer"></param>
        /// <returns></returns>
        public async Task<Result> SetStudentAnswerAsync(SetStudentAnswerDto model)
        {
            Result response = new Result();
            do
            {
                //学生任务表查找这个任务
                var studentTask = await context.StudentTasks.FirstOrDefaultAsync(e => e.TaskId == model.TaskId && e.StudentId == model.StudentId);
                if (studentTask == null)
                {
                    response.ErrorMessage = "学生表不存在任务";
                    break;
                }

                if (studentTask.IsTeacherModified)
                {
                    response.ErrorMessage = "教师已经批改，不能答题了";
                    break;
                }

                //教师表查找这个任务
                var teacherTask = await context.TeacherTasks.FirstOrDefaultAsync(e => e.Id == model.TaskId);
                if (teacherTask == null)
                {
                    response.ErrorMessage = "教师表不存在任务";
                    break;
                }

                ///超过答题时间了
                if (DateTime.Now > teacherTask.TaskEndTime || DateTime.Now < teacherTask.TaskStartTime)
                {
                    response.ErrorMessage = "不在答题时间内，不能答题";
                    break;
                }


                //已经答过了 不能重复答题
                //if (task.DateTime != DateTime.MinValue)
                //{
                //    response.ErrorMessage = "已经答过了，不能重复答题";
                //    break;
                //}

                studentTask.StudentAnswer = model.Answer.Trim();  //答案
                studentTask.DateTime = DateTime.Now;              //答题时间
                studentTask.IsAlreadyAnswered = true;             //设置已经答题

                await context.SaveChangesAsync();
                response.Succeeded = true;
            } while (false);
            return response;
        }


        /// <summary>
        /// 设置学生成绩
        /// </summary>
        /// <returns></returns>
        public async Task<Result> SetStudentGradeAsync(SetStudentGradeDto model)
        {
            Result response = new Result();
            do
            {
                //先在学生表中获取这个任务
                var studentTask = await context.StudentTasks.FirstOrDefaultAsync(e =>
                        e.StudentId == model.StudentId && e.TaskId == model.TaskId);
                if (studentTask == null)
                {
                    response.ErrorMessage = "不存在作业";
                    break;
                }

                studentTask.Grade = model.Grade;  //设置成绩
                studentTask.TeacherReply = model.TeacherReply; //设置回复
                studentTask.IsTeacherModified = true; //已批改

                await context.SaveChangesAsync();
                response.Succeeded = true;

            } while (false);

            return response;
        }



        /// <summary>
        /// 删除教师任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task<Result> DeleteTaskAsync(string teacherId, string taskId)
        {
            Result response = new Result();
            do
            {
                //删除学生列表的任务
                var studentTasks = context.StudentTasks.Where(e => e.TaskId == taskId);
                context.StudentTasks.RemoveRange(studentTasks);


                //删除教师列表任务
                var teacherTasks = context.TeacherTasks.Where(e => e.Id == taskId && e.TeacherId == teacherId);
                context.TeacherTasks.RemoveRange(teacherTasks);

                await context.SaveChangesAsync();
                response.Succeeded = true;
            } while (false);
            return response;
        }
    }
}
