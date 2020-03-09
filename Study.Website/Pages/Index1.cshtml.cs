using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Study.Website.Pages
{
    [Microsoft.AspNetCore.Authorization.Authorize("indexView")]
    public class Index1Model : PageModel
    {
        public void OnGet()
        {

        }
    }
}
