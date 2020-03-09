using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserGrpcService;
using Utility;

namespace Study.Website.Pages.Teacher
{
    [Authorize(Policy = Permissons.Permisson.Teacher_Helpers)]
    public class TeacherFriendModel : AppPageModel
    {
        /// <summary>
        /// 助手表
        /// </summary>
        public List<string> UserFriends { get; set; }

        /// <summary>
        /// 其他可以添加的教师
        /// </summary>
        public IEnumerable<UserReply> OtherTeachers { get; set; }


        public async Task OnGetAsync()
        {
            var curUserId = CurUserId;
            UserFriends = await UserService.GetUserFriends(curUserId).ToListAsync().ConfigureAwait(false);

            //不是我 且不在好友表中
            var otherTeachers = await UserClient.GetUsersByRoleNameAsync(new UserGrpcService.RoleNameReq
            {
                RoleName = ConstStrings.Role_Teacher,
                Skip = 0,
                Take = 1000
            });

            OtherTeachers = otherTeachers.Users.Where(e => !UserFriends.Any(ee => ee == e.Id) && e.Id!=CurUserId);
        }


        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="freindId"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAddFriendAsync(string freindId)
        {
            if (string.IsNullOrEmpty(freindId))
            {
                return BadRequest();
            }

            var result = await UserService.AddUserFriendAsync(CurUserId, freindId).ConfigureAwait(true);
            if (result.Succeeded)
            {
                return Content("Ok");
            }

            ModelState.AddModelError(string.Empty, result.ErrorMessage);
            return BadRequest(ModelState);
        }

        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="freindId"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostDeleteFriendAsync(string freindId)
        {
            if (string.IsNullOrEmpty(freindId))
            {
                return BadRequest();
            }

            var result = await UserService.DeleteUserFriendAsync(CurUserId, freindId).ConfigureAwait(false);
            if (result.Succeeded)
            {
                return Content("Ok");
            }

            ModelState.AddModelError(string.Empty, result.ErrorMessage);
            return BadRequest(ModelState);
        }
    }
}