using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Study.Dto.ArticalDto;
using Study.Services.ArticalService;
using Study.WebApp.Data;

namespace Study.WebApp.Pages.Forum
{
    [Authorize]
    public class PublishArticalModel : PageModel
    {
        private readonly UserManager<IdentityUserEx> userManager;
        private readonly IArticalService articalService;
        private readonly IMemoryCache cache;

        public PublishArticalModel(UserManager<IdentityUserEx> userManager, IArticalService articalService, IMemoryCache cache)
        {
            this.userManager = userManager;
            this.articalService = articalService;
            this.cache = cache;
        }


        /// <summary>
        /// 文章id, Id不为空时，修改文章
        /// </summary>

        [FromRoute(Name = "Id")]
        public string Id { get; set; }

        /// <summary>
        /// 添加/更新文章模型
        /// </summary>
        [BindProperty]
        public AddArticalDto AddArticalDto  { get; set; }


        //添加文章
        public async Task<IActionResult> OnPostAddArticalAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var curUserId = userManager.GetUserId(User);
            bool isRunning = cache.Get<bool>($"OnPostAddArticalAsync{curUserId}");
            if (isRunning)
            {
                ModelState.AddModelError("", "两次发帖时间间隔最短为60秒");
                return Page();
            }

            cache.Set($"OnPostAddArticalAsync{curUserId}", true, TimeSpan.FromSeconds(60));


            var respone = await articalService.AddArticalAsync(AddArticalDto);
            if (respone.IsOk)
            {
                return RedirectToPage("/Forum/ArticalDetail", new { ArticalId = respone.Data.Id });
            }
            ModelState.AddModelError("", respone.ErrorMessage);

            return Page();
        }
    }
}