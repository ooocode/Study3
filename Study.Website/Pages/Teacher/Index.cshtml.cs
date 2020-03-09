using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Study.WebApp.Data;
using Utility;

namespace Study.Website.Pages.Teacher
{
    [Authorize(Roles = ConstStrings.Role_Teacher)]
    public class IndexModel : AppPageModel
    {
        public void OnGet()
        {

        }
    }
}