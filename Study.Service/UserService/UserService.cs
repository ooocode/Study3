using Microsoft.EntityFrameworkCore;
using Study.Database;
using Study.Database.Entity.UserEntities;
using Study.Services.UserService.Res;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Study.Services.UserService.Req;
using Microsoft.AspNetCore.Http;
using Study.Service.UserService.Res;
using Study.Service.UserService.Req;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System;
using Study.Service;
using Z.EntityFramework.Plus;
namespace Study.Services.UserService
{
    public class UserService : ServiceBase,IUserService
    {
        private readonly UserDatabaseContext context;
        private readonly CacheService cacheService;

        public UserService(UserDatabaseContext context, CacheService cacheService, IHttpContextAccessor httpContextAccessor)
            :base(httpContextAccessor)
        {
            this.context = context;
            this.cacheService = cacheService;
        }

        /// <summary>
        /// 添加用户朋友
        /// </summary>
        /// <param name="friendId"></param>
        /// <returns></returns>
        public async Task<Result> AddUserFriendAsync(string userId, string friendId)
        {
            Result result = new Result();
            do
            {
                var existFriend = await context.UserFriends.AnyAsync(e => e.UserId == userId && e.FriendId == friendId);
                if (existFriend)
                {
                    result.ErrorMessage = "已经存在";
                    break;
                }

                await context.UserFriends.AddAsync(new UserFriend { UserId = userId, FriendId = friendId });

                await context.SaveChangesAsync();

                result.Succeeded = true;
            } while (false);
            return result;
        }


        /// <summary>
        /// 删除用户朋友
        /// </summary>
        /// <param name="friendId"></param>
        /// <returns></returns>
        public async Task<Result> DeleteUserFriendAsync(string userId, string friendId)
        {
            Result result = new Result();
            do
            {
                var friend = await context.UserFriends.FirstOrDefaultAsync(e => e.UserId == userId && e.FriendId == friendId);
                if (friend == null)
                {
                    result.ErrorMessage = "不存在";
                    break;
                }

                context.UserFriends.Remove(friend);

                await context.SaveChangesAsync();

                result.Succeeded = true;
            } while (false);
            return result;
        }

        /// <summary>
        /// 获取用户朋友列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IQueryable<string> GetUserFriends(string userId)
        {
            return context.UserFriends.Where(e => e.UserId == userId).Select(e => e.FriendId);
        }


        /// <summary>
        /// 获取用户朋友数量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<int> GetCurUserFriendsCountAsync()
        {
            return context.UserFriends.CountAsync(e => e.UserId == CurUserId);
        }


        #region 教师班级

        /// <summary>
        /// 获取当前教师班级数量
        /// </summary>
        /// <returns></returns>
        public Task<int> GetCurTeacherClassCountAsync()
        {
            return context.TeacherClasses.Where(e => e.TeacherId == CurUserId).CountAsync();
        }

        /// <summary>
        /// 添加教师班级
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //public async Task<Result> AddTeacherClassAsync(AddTeacherClassDto model)
        //{
        //    Result respone = new Result();
        //    do
        //    {
        //        var existSchoolClass = await context.SchoolClasses.AnyAsync(e => e.Id == model.ClassId).ConfigureAwait(false);
        //        if (!existSchoolClass)
        //        {
        //            respone.ErrorMessage = "不存在学校班级【AddTeacherClassAsync】";
        //            break;
        //        }

        //        var hadAdded = await context.TeacherClasses
        //            .AnyAsync(e => e.TeacherId == model.TeacherId && e.ClassId == model.ClassId).ConfigureAwait(false);
        //        if (hadAdded)
        //        {
        //            respone.ErrorMessage = "已经存在教师班级【AddTeacherClassAsync】";
        //            break;
        //        }

        //        await context.TeacherClasses.AddAsync(new TeacherClass {
        //            ClassId = model.ClassId,
        //            TeacherId = model.TeacherId
        //        });

        //        await context.SaveChangesAsync().ConfigureAwait(false);

        //        respone.Succeeded = true;
        //    } while (false);
        //    return respone;
        //}


        /// <summary>
        /// 删除教师班级
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //public async Task<Result> DeleteTeacherClassAsync(string teacherId, string classId)
        //{
        //    Result respone = new Result();
        //    do
        //    {
        //        TeacherClass c = await context.TeacherClasses
        //            .FirstOrDefaultAsync(e => e.TeacherId == teacherId && e.ClassId == classId)
        //            .ConfigureAwait(false);
        //        if (c == null)
        //        {
        //            respone.ErrorMessage = "不存在这样的班级【DeleteTeacherClassAsync】";
        //            break;
        //        }

        //        context.TeacherClasses.Remove(c);
        //        await context.SaveChangesAsync().ConfigureAwait(false);

        //        respone.Succeeded = true;
        //    } while (false);
        //    return respone;
        //}


        ///// <summary>
        ///// 获取教师班级
        ///// </summary>
        ///// <param name="teacherId"></param>
        ///// <returns></returns>
        //public IQueryable<TeacherClassDto> GetTeacherClasses(string teacherId)
        //{
        //    //教师班级表和学校班级表连接查询
        //    return context.TeacherClasses
        //        .Where(e => e.TeacherId == teacherId)
        //        .Join(context.SchoolClasses, e => e.ClassId, ee => ee.Id,
        //        (teacher, school) => new TeacherClassDto {
        //            ClassId = teacher.ClassId,
        //            ClassName = school.Name
        //        });
        //}
        #endregion
    }
}
