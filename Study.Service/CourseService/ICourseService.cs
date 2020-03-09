using Study.Service._Configure;
using Study.Service.CourseService.Req;
using Study.Service.CourseService.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study.Services.CourseService
{
    /// <summary>
    /// 课程服务接口
    /// </summary>
    [AutoInject(typeof(CourseService))]
    public interface ICourseService
    {
        /// <summary>
        /// 获取所有课程
        /// </summary>
        /// <returns></returns>
        IQueryable<CourseDto> GetCourses();

        /// <summary>
        /// 获取课程
        /// </summary>
        /// <returns></returns>
        Task<CourseDto> GetCourseByIdAsync(string courseId);


        /// <summary>
        /// 添加或者更新课程
        /// </summary>
        /// <param name="dto"></param>
        Task<Result> AddOrUpdateCourseAsync(AddOrUpdateCourseDto dto);


        ///// <summary>
        ///// 更新课程
        ///// </summary>
        ///// <param name="courseId"></param>
        ///// <param name="dto"></param>
        ///// <returns></returns>
        //Task<Result> UpdateCourseAsync(string courseId, AddOrUpdateCourseDto dto);


        /// <summary>
        /// 删除课程
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        Task<Result> DeleteCourseAsync(string courseId);
    }
}
