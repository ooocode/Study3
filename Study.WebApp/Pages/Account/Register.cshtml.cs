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

    public class RegisterModel : PageModel
    {
        public class UserRegistModel
        {
            [Required(ErrorMessage = "邮箱不能为空")]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(20, MinimumLength = 6)]
            public string Password { get; set; }
        }


        [BindProperty]
        public UserRegistModel registModel { get; set; }


        private readonly UserManager<IdentityUserEx> userManager;
        private readonly SignInManager<IdentityUserEx> signInManager;

        public RegisterModel(UserManager<IdentityUserEx> userManager, SignInManager<IdentityUserEx> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUserEx
                {
                    Email = registModel.Email,
                    UserName = registModel.Email
                };

                var result = await userManager.CreateAsync(user, registModel.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, true);
                    return RedirectToPage("/Index");
                }

                result.Errors.ToList().ForEach(e => ModelState.AddModelError(string.Empty, e.Description));
            }
            return Page();
        }
    }
}