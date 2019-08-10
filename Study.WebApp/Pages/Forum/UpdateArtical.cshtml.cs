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
    public class UpdateArticalModel : PageModel
    {
        private readonly UserManager<IdentityUserEx> userManager;
        private readonly IArticalService _articalService;
        private readonly IMemoryCache _cache;

        public UpdateArticalModel(UserManager<IdentityUserEx> userManager,
            IArticalService articalService, 
            IMemoryCache cache)
        {
            this.userManager = userManager;
            _articalService = articalService;
            _cache = cache;
        }


        /// <summary>
        /// 文章id
        /// </summary>
        [FromRoute(Name = "Id")]
        public string Id { get; set; }

        /// <summary>
        /// 添加/更新文章模型
        /// </summary>
        [BindProperty]
        public UpdateArticalDto UpdateArticalDto { get; set; }



        public async Task<IActionResult> OnGetAsync()
        {
            if (!string.IsNullOrEmpty(Id))
            {
                var artical = await _articalService.GetArticalByIdAsync(id: Id);
                //更新文章
                if (artical != null)
                {
                    UpdateArticalDto = new UpdateArticalDto
                    {
                        ArticalId = artical.Id,
                        ClassificationId = artical.ClassificationId,
                        Content = artical.Content,
                        Title = artical.Title
                    };
                    return Page();
                }
            }
            return NotFound();
        }


        /// <summary>
        /// 更新文章
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostUpdateArticalAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var curUserId = userManager.GetUserId(User);
       
            bool isRunning = _cache.Get<bool>($"OnPostUpdateArticalAsync{curUserId}");
            if (isRunning)
            {
                ModelState.AddModelError("", "两次修改时间间隔最短为60秒");
                return Page();
            }

            _cache.Set($"OnPostUpdateArticalAsync{curUserId}", true, TimeSpan.FromSeconds(60));



            var respone = await _articalService.UpdateArticalAsync(Id, UpdateArticalDto);
            if (respone.IsOk)
            {
                return RedirectToPage("/Forum/ArticalDetail", new { ArticalId = Id });
            }
            ModelState.AddModelError("", respone.ErrorMessage);

            return Page();
        }
    }
}