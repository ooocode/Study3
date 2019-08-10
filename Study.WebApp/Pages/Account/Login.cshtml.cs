using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Study.WebApp.Data;

namespace Study.WebApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly UserManager<IdentityUserEx> userManager;
        private readonly SignInManager<IdentityUserEx> signInManager;

        public class UserLoginModel
        {
            [Required]
            public string UserName { get; set; }


            [Required]
            public string Password { get; set; }
        }


        public LoginModel(UserManager<IdentityUserEx> userManager, SignInManager<IdentityUserEx> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public void OnGet()
        {

        }

        [BindProperty]
        public UserLoginModel loginModel { get; set; }

        public async Task<IActionResult> OnPostAsync(string ReturnUrl = null)
        {
            ReturnUrl = ReturnUrl ?? "/Index";

            if (ModelState.IsValid)
            {

                var result = await signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password,
                    isPersistent: true, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToPage(ReturnUrl);
                }

                ModelState.AddModelError(string.Empty, "检查账号密码是否一致");
            }
            return Page();
        }


        public async Task<IActionResult> OnGetLogout()
        {
            await signInManager.SignOutAsync();
            return RedirectToPage("/Index");
        }
    }
}