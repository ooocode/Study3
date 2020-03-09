using Study.Database.Entity.UserEntities;
using Study.Service._Configure;
using Study.Service.UserService.Req;
using Study.Service.UserService.Res;
using Study.Services.UserService.Req;
using Study.Services.UserService.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study.Services.UserService
{
    [AutoInject(typeof(UserService))]
    public interface IUserService
    {
        /// <summary>
        /// 添加用户朋友
        /// </summary>
        /// <param name="friendId"></param>
        /// <returns></returns>
        Task<Result> AddUserFriendAsync(string userId, string friendId);


        /// <summary>
        /// 删除用户朋友
        /// </summary>
        /// <param name="friendId"></param>
        /// <returns></returns>
        Task<Result> DeleteUserFriendAsync(string userId, string friendId);

        /// <summary>
        /// 获取用户朋友列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IQueryable<string> GetUserFriends(string userId);

        /// <summary>
        /// 获取当前用户数量
        /// </summary>
        /// <returns></returns>
        Task<int> GetCurUserFriendsCountAsync();
        Task<int> GetCurTeacherClassCountAsync();



        ///// <summary>
        ///// 添加教师班级
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //Task<Result> AddTeacherClassAsync(AddTeacherClassDto model);


        ///// <summary>
        ///// 删除教师班级
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //Task<Result> DeleteTeacherClassAsync(string teacherId, string classId);


        ///// <summary>
        ///// 获取教师班级
        ///// </summary>
        ///// <param name="teacherId"></param>
        ///// <returns></returns>
        //IQueryable<TeacherClassDto> GetTeacherClasses(string teacherId);
    }
}
