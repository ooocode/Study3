using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Study.Service.MovieService;

namespace Study.Website.Pages.Movie.Admin
{
    public class TypesManagerModel : PageModel
    {
        private readonly IMovieService movieService;

        public TypesManagerModel(IMovieService movieService)
        {
            this.movieService = movieService;
        }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnGetLoadTypesAsync()
        {
            var types = await movieService.GetVideoTypesAsync().ConfigureAwait(false);
            List<dynamic> ls = new List<dynamic>();
            foreach (var item in types)
            {
                var obj = new
                {
                    value = item.Name,
                    id = item.Id,
                    opened = true
                };
                ls.Add(obj);
            }
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(ls));
        }

        public class Item
        {
            public string value  { get; set; }
            public string id { get; set; }
        }

        public async Task<IActionResult> OnGetLoadTypes1Async()
        {
            var types = await movieService.GetVideoTypesAsync().ConfigureAwait(false);
            List<dynamic> ls = new List<dynamic>();
            foreach (var item in types)
            {
                var obj = new
                {
                    value = item.Name,
                    id = item.Id,
                    items = new List<Item>() { new Item { id = "",value = ""} }
                };
                ls.Add(obj);
            }
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(ls));
        }
    }
}