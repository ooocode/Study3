using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Study.Database;
using Study.Database.VideoDb;
using Study.Service.MovieService;
using Study.WebApp.Data;

namespace Study.Website.Pages.Movie
{
    public class IndexModel : PageModel
    {
        private readonly IMovieService movieService;
        private readonly VideoDbContext videoDbContext;

        public readonly AdvertisementService advertisementService;

        /// <summary>
        /// 视频分类id
        /// </summary>
        [FromRoute(Name = "Id")]
        public int? Id { get; set; }

        /// <summary>
        /// 当前分类信息
        /// </summary>
        public VideoType CurVideoType { get; set; }


        public IndexModel(IMovieService movieService,VideoDbContext videoDbContext, AdvertisementService advertisementService)
        {
            this.movieService = movieService;
            this.videoDbContext = videoDbContext;
            this.advertisementService = advertisementService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!Id.HasValue)
            {
                var classificationDto = (await movieService.GetVideoTypesAsync().ConfigureAwait(false)).FirstOrDefault();
                if (classificationDto == null)
                {
                    return RedirectToPage("/Forum/Index");
                }
                Id = classificationDto.Id;
            }

            CurVideoType = await movieService.GetVideoTypeByIdAsync(Id.Value).ConfigureAwait(false);
            return Page();
        }

        /// <summary>
        /// 获取电影
        /// </summary>
        /// <param name="skipCount"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetLoadMoviesAsync(int skipCount)
        {
            var typeInfo = await movieService.GetVideoTypeByIdAsync(Id.Value).ConfigureAwait(false);
            int[] subTypeIds = { typeInfo.Id };

            //没有父目录 说明这是电影 电视剧 综艺 动漫等  需要找出他的子目录
            if (!typeInfo.ParentTypeId.HasValue)
            {
                subTypeIds = (await movieService.GetVideoTypesAsync().ConfigureAwait(false)).Where(e => e.ParentTypeId == Id).Select(e => e.Id).ToArray();
            }

            List<Video> result = await movieService.GetVideosAsync(subTypeIds, skip: skipCount).ConfigureAwait(false);

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            return Content(json, "application/json");
        }
    }
}