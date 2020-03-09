using Study.Service._Configure;
using Study.Services.TaskService.Req;
using Study.Services.TaskService.Res;
using System.Linq;
using System.Threading.Tasks;

namespace Study.Services.TaskService
{
    [AutoInject(typeof(TaskService))]
    public interface ITaskService
    {
        /// <summary>
        /// 添加任务同时发布给学生  
        /// </summary>
        /// <param name="model"></param>
        /// <returns>成功返回课程Id</returns>
        Task<Result<string>> AddOrUpdateTeacherTaskAsync(AddOrUpdateTeacherTaskDto dto);


        /// <summary>
        /// 获取所有的教师发布的作业
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        IQueryable<TeacherTaskDto> GetTeacherTasks();


        /// <summary>
        /// 通过id获取教师发布的作业
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        Task<TeacherTaskDto> GetTeacherTaskByIdAsync(string taskId);



        /// <summary>
        /// 获取所有学生的所有任务
        /// </summary>
        /// <returns></returns>
        IQueryable<StudentTaskDto> GetStudentTasks();


        /// <summary>
        /// 学生答题
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="taskAnswer"></param>
        /// <returns></returns>
        Task<Result> SetStudentAnswerAsync(SetStudentAnswerDto model);


        /// <summary>
        /// 设置学生成绩
        /// </summary>
        /// <returns></returns>
        Task<Result> SetStudentGradeAsync(SetStudentGradeDto model);


        /// <summary>
        /// 删除教师任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        Task<Result> DeleteTaskAsync(string teacherId, string taskId);
    }
}
