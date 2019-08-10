using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Study.WebApp.Data;

namespace Study.WebApp.Pages.Account
{
    [Authorize]
    public class UpdateInfoModel : PageModel
    {
        private readonly IHostingEnvironment _env;
        private readonly UserManager<IdentityUserEx> userManager;
        private readonly SignInManager<IdentityUserEx> signInManager;

        public UpdateInfoModel(IHostingEnvironment env,
            UserManager<IdentityUserEx> userManager,
            SignInManager<IdentityUserEx> signInManager)
        {
            _env = env;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        public class UpdateModel
        {
            [Required]
            public string Name { get; set; }

            [Range(0, 1)]
            public byte Sex { get; set; }


            public string Desc { get; set; }
        }


        /// <summary>
        /// 更新信息模型
        /// </summary>
        [BindProperty]
        public UpdateModel updateModel { get; set; }


        public async Task<IActionResult> OnPostUploadPhoto()
        {
            IFormFile file = HttpContext.Request.Form.Files[0];
            if (file != null)
            {
                string filename = DateTime.Now.Ticks.ToString() + "_" + Guid.NewGuid().ToString("N") + "_" + System.IO.Path.GetExtension(file.FileName);

                string saveDir = System.IO.Path.Combine(_env.WebRootPath, "UploadFiles");
                if (!System.IO.Directory.Exists(saveDir))
                {
                    System.IO.Directory.CreateDirectory(saveDir);
                }

                string savePath = System.IO.Path.Combine(saveDir, filename);
                using (var fileStream = System.IO.File.Create(savePath))
                {
                    await file.OpenReadStream().CopyToAsync(fileStream);
                }

                if (System.IO.File.Exists(savePath))
                {
                    var user= await userManager.GetUserAsync(User);
                    user.Photo = filename;
                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToPage();
                    }
                }
            }
            return Page();
        }


        /// <summary>
        /// 更新信息
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                user.Name = updateModel.Name;
                user.Sex = updateModel.Sex;
                user.Desc = updateModel.Desc;

                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToPage();
                }

                result.Errors.ToList().ForEach(e => ModelState.AddModelError(string.Empty, e.Description));
            }
            return Page();
        }
    }
}