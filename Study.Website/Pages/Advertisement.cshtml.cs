using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Permissons;
using Study.Website.Pages;
using Utility;

namespace Study.Website
{
    [Authorize(Permisson.Adv)]

    public class AdvertisementModel : AppPageModel
    {
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync(string text)
        {
            var isOk = await AdvertisementService.AddOrUpdateAdvertisementAsync(text).ConfigureAwait(false);
            if (isOk)
            {
                return RedirectToPage();
            }
            return Page();
        }
    }
}