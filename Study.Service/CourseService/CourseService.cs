using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Study.Database;
using Study.Database.Entity.CourseEntities;
using Study.Service.CourseService.Req;
using Study.Service.CourseService.Res;

namespace Study.Services.CourseService
{
    public class CourseService : ICourseService
    {
        private readonly UserDatabaseContext db;
        private readonly IMemoryCache cache;

        public CourseService(UserDatabaseContext db, IMemoryCache cache)
        {
            this.db = db;
            this.cache = cache;
        }


        /// <summary>
        /// 获取所有课程
        /// </summary>
        /// <returns></returns>
        public IQueryable<CourseDto> GetCourses()
        {
            return db.Courses.Select(e => new CourseDto {
                Id = e.Id,
                AddDateTime = e.AddDateTime,
                Desc = e.Desc,
                LastModifyDatetime = e.LastModifyDatetime,
                Name = e.Name,
                UserId = e.UserId
            });
        }


        /// <summary>
        /// 获取课程
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<CourseDto> GetCourseByIdAsync(string courseId)
        {
            return await GetCourses().FirstOrDefaultAsync(e => e.Id == courseId);
        }

        private async Task<(bool sucess, string errMessage)> AddCourseAsync(AddOrUpdateCourseDto dto)
        {
            //先判断用户是不是存在了课程名称
            bool existCourse = await db.Courses.AnyAsync(e => e.UserId == dto.UserId && e.Name == dto.Name);
            if (existCourse)
            {
                return (false, "添加课程失败，已经存在了该课程");
            }

            var now = DateTime.UtcNow;
            var id = Guid.NewGuid().ToString("N");
            await db.Courses.AddAsync(new Course {
                Id = id,
                Desc = dto.Desc,
                AddDateTime = now,
                UserId = dto.UserId,
                Name = dto.Name,
                LastModifyDatetime = now
            });

            int rows = await db.SaveChangesAsync();
            return (rows > 0, null);
        }

        /// <summary>
        /// 更新课程
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<(bool sucess, string errMessage)> UpdateCourseAsync(AddOrUpdateCourseDto dto)
        {

            //先判断用户是不是存在了课程
            var course = await db.Courses.FirstOrDefaultAsync(e => e.Id == dto.Id && e.UserId == dto.UserId);
            if (course == null)
            {
                return (false, "不存在该课程");
            }

            course.Desc = dto.Desc;
            course.Name = dto.Name;
            course.LastModifyDatetime = DateTime.UtcNow;

            try
            {
                int row = await db.SaveChangesAsync();
                return (row > 0, null);
            }
            catch(Exception ex)
            {
                return (false, ex.InnerException?.Message??ex.Message);
            }
        }


        /// <summary>
        /// 添加或者更新课程
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<Result> AddOrUpdateCourseAsync(AddOrUpdateCourseDto dto)
        {
            (bool sucess, string errMessage) result;

            if (string.IsNullOrEmpty(dto.Id))
            {
                result = await AddCourseAsync(dto);
            }
            else
            {
                result = await UpdateCourseAsync(dto);
            }
            return new Result { Succeeded = result.sucess, ErrorMessage = result.errMessage };
        }







        /// <summary>
        /// 删除课程
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<Result> DeleteCourseAsync(string courseId)
        {
            Result respone = new Result();
            do
            {
                //先判断用户是不是存在了课程名称
                var course = await db.Courses.FirstOrDefaultAsync(e => e.Id == courseId);
                if (course == null)
                {
                    respone.ErrorMessage = "不存在课程";
                    break;
                }

                db.Courses.Remove(course);
                await db.SaveChangesAsync();

                respone.Succeeded = true;
            } while (false);

            return respone;
        }
    }
}
