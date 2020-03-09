using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Study.Database;
using Study.Service;
using Study.Services.ArticalService;
using Study.Services.CourseService;
using Study.Services.TaskService;
using Study.Services.UserService;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Study.Website.Pages
{
    public class AppPageModel : PageModel
    {
        protected UserGrpcService.User.UserClient UserClient
        {
            get
            {
                return HttpContext.RequestServices.GetService(typeof(UserGrpcService.User.UserClient)) as UserGrpcService.User.UserClient;
            }
        }

        protected UserDatabaseContext UserDatabaseContext
        {
            get
            {
                return HttpContext.RequestServices.GetService(typeof(UserDatabaseContext)) as UserDatabaseContext;
            }
        }

        //protected SignInManager<ApplicationUser> SignInManager
        //{
        //    get
        //    {
        //        return HttpContext.RequestServices.GetService(typeof(SignInManager<ApplicationUser>)) as SignInManager<ApplicationUser>;
        //    }
        //}
        protected CacheService CacheService
        {
            get
            {
                return HttpContext.RequestServices.GetService(typeof(CacheService)) as CacheService;
            }
        }


        protected IArticalService ArticalService
        {
            get
            {
                return HttpContext.RequestServices.GetService(typeof(IArticalService)) as IArticalService;
            }
        }

        protected IUserService UserService
        {
            get
            {
                return HttpContext.RequestServices.GetService(typeof(IUserService)) as IUserService;
            }
        }



        protected ITaskService TaskService
        {
            get
            {
                return HttpContext.RequestServices.GetService(typeof(ITaskService)) as ITaskService;
            }
        }


        protected ICourseService CourseService
        {
            get
            {
                return HttpContext.RequestServices.GetService(typeof(ICourseService)) as ICourseService;
            }
        }

        protected IHostingEnvironment _env
        {
            get
            {
                return HttpContext.RequestServices.GetService(typeof(IHostingEnvironment)) as IHostingEnvironment;
            }
        }

        public AdvertisementService AdvertisementService
        {
            get
            {
                return HttpContext.RequestServices.GetService(typeof(AdvertisementService)) as AdvertisementService;
            }
        }

        /// <summary>
        /// 授权服务
        /// </summary>
        public IAuthorizationService AuthorizationService
        {
            get
            {
                return HttpContext.RequestServices.GetService(typeof(IAuthorizationService)) as IAuthorizationService;
            }
        }

        /// <summary>
        /// 添加错误
        /// </summary>
        /// <param name="message"></param>
        public void AddModelError(string message)
        {
            ModelState.AddModelError(string.Empty, message);
        }
        //public AppPageModel()
        //{
        //    UserManager = HttpContext.RequestServices.GetService(typeof(UserManager<ApplicationUser>)) as UserManager<ApplicationUser>;
        //    RoleManager = HttpContext.RequestServices.GetService(typeof(RoleManager<ApplicationUser>)) as RoleManager<ApplicationUser>;
        //    SignInManager = HttpContext.RequestServices.GetService(typeof(SignInManager<ApplicationUser>)) as SignInManager<ApplicationUser>;

        //    CacheService = HttpContext.RequestServices.GetService(typeof(CacheService)) as CacheService;

        //    ArticalService = HttpContext.RequestServices.GetService(typeof(IArticalService)) as IArticalService;
        //    UserService = HttpContext.RequestServices.GetService(typeof(IUserService)) as IUserService;
        //    TaskService = HttpContext.RequestServices.GetService(typeof(ITaskService)) as ITaskService;
        //    CourseService = HttpContext.RequestServices.GetService(typeof(ICourseService)) as ICourseService;

        //    _env = HttpContext.RequestServices.GetService(typeof(IHostingEnvironment)) as IHostingEnvironment;
        //}

        //public AppPageModel()
        //{

        //}

        //public AppPageModel(UserManager<ApplicationUser> userManager,
        //    RoleManager<ApplicationUser> roleManager,
        //    SignInManager<ApplicationUser> signInManager,
        //    CacheService cacheService,
        //    IArticalService articalService)
        //{
        //    UserManager = userManager;
        //    RoleManager = roleManager;
        //    SignInManager = signInManager;
        //    CacheService = cacheService;
        //    ArticalService = articalService;
        //}

        /// <summary>
        /// 获取当前用户id
        /// </summary>
        /// <returns></returns>
        public string CurUserId
        {
            get
            {
                if (IsLogin)
                {
                    return User.Claims.FirstOrDefault(e => e.Type == "sub").Value;
                }
                return string.Empty;
            }
        }


        /// <summary>
        /// 头像
        /// </summary>
        public string Picture
        {
            get
            {
                if (IsLogin)
                {
                    Claim picture = User.Claims.FirstOrDefault(e => e.Type == JwtClaimTypes.Picture);
                    if (picture != null)
                    {
                        return picture.Value;
                    }
                }
                return string.Empty;
            }
        }


        /// <summary>
        /// 角色
        /// </summary>
        public IEnumerable<string> Roles
        {
            get
            {
                if (IsLogin)
                {
                    return User.Claims.Where(e => e.Type == JwtClaimTypes.Role).Select(e => e.Value);
                }
                return new List<string>();
            }
        }


        /// <summary>
        /// 是否登录
        /// </summary>
        /// <returns></returns>
        public bool IsLogin => User.Identity != null && User.Identity.IsAuthenticated;
    }
}
