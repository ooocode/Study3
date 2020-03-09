using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Study.Services.ArticalService;
using Study.Services.ArticalService.Req;
using Study.WebApp.Data;
using Utility;

namespace Study.Website.Pages.Forum
{
    [Authorize(Permissons.Permisson.Article_ManagerTypes)]
    public class ArticalClassificationManageModel : PageModel
    {
        private readonly IArticalService articalService;

        public ArticalClassificationManageModel(IArticalService articalService)
        {
            this.articalService = articalService;
        }

        public void OnGet()
        {

        }

        /// <summary>
        /// 添加文章分类
        /// </summary>
        [BindProperty]
        public AddArticalClassificationDto AddArticalClassificationDto { get; set; }

        public async Task<IActionResult> OnPostAddClassificationAsync()
        {
            if (ModelState.IsValid)
            {
                var respone = await articalService.AddArticalClassificationAsync(AddArticalClassificationDto).ConfigureAwait(false);
                if (respone.Succeeded)
                {
                    return RedirectToPage();
                }
            }
            return Page();
        }


        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostDeleteClassificationAsync(long id)
        {
            if (id > 0)
            {
                var respone = await articalService.DeleteArticalClassificationAsync(id).ConfigureAwait(false);
                if (respone.Succeeded)
                {
                    return RedirectToPage();
                }
            }
            return Page();
        }


        /// <summary>
        /// 更新文章分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostUpdateArticalClassificationAsync(long id)
        {
            if (ModelState.IsValid)
            {
                var respone = await articalService.UpdateArticalClassificationAsync(id
                    , new UpdateArticalClassificationDto { Id = id, Name = AddArticalClassificationDto.Name }).ConfigureAwait(false);
                if (respone.Succeeded)
                {
                    //Alert.SetSuccess("更新了文章分类");
                    return RedirectToPage();
                }
                //Alert.SetError(respone.ErrorMessage);
            }
            return Page();
        }
    }
}