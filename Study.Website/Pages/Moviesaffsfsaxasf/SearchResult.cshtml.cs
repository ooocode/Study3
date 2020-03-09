using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Study.Database.VideoDb;
using Study.Service;
using Study.Service.MovieService;


namespace Study.Website.Pages.Movie
{
    public class SearchResultModel : PageModel
    {
        private readonly VideoDbContext videoDbContext;
        private readonly IMovieService movieService;
        private readonly IMemoryCache memoryCache;
        private readonly CacheService cacheService;

        public SearchResultModel(VideoDbContext videoDbContext,
            IMovieService movieService,
            IMemoryCache memoryCache)
        {
            this.videoDbContext = videoDbContext;
            this.movieService = movieService;
            this.memoryCache = memoryCache;
        }

        public string Search { get; private set; }


        public void OnGet(string search)
        {
            this.Search = search;
        }

        public async Task<List<Video>> GetVideosAsync()
        {
            List<Video> videos = null;
            if(!memoryCache.TryGetValue("videoSearch",out videos))
            {
                //获取所有视频分类
                var videoTypes = await movieService.GetVideoTypesAsync().ConfigureAwait(false);
                //获取可以显示的视频分类id   有些分类id是不能显示的  比如说福利片！
                var enableVideoTypeIds = videoTypes.Select(e => e.Id);


                videos = await videoDbContext.Videos
                    .Where(e => e.ShowInPage == true && enableVideoTypeIds.Contains(e.VideoTypeId))
                    .ToListAsync().ConfigureAwait(false);

                memoryCache.Set("videoSearch", videos,TimeSpan.FromMinutes(30));
            }
            return videos;
        }


        public async Task<IActionResult> OnGetSearchMoviesAsync(string search,int skip)
        {
            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim();


                ////获取所有视频分类
                //var videoTypes = await movieService.GetVideoTypesAsync().ConfigureAwait(false);
                ////获取可以显示的视频分类id   有些分类id是不能显示的  比如说福利片！
                //var enableVideoTypeIds = videoTypes.Select(e=>e.Id);


                //var videos = videoDbContext.Videos
                //    .Where(e =>e.ShowInPage == true && enableVideoTypeIds.Contains(e.VideoTypeId));

                var videos = await GetVideosAsync();

                List<Video> result =  videos.Where(e => e.Name.Contains(search)
                                                        || e.Actor.Contains(search)
                                                        || e.Director.Contains(search)).OrderByDescending(e => e.LastUpdateTime)
                                                        .Skip(skip)
                                                        .Take(12).ToList();


                //现在的
                //IEnumerable<Video> videos = (await movieService.GetVideosAsync().ConfigureAwait(false))
                //    .Where(e => enableVideoTypeIds.Contains(e.VideoTypeId));


                //videos =  videos.Where(e => e.Name.Contains(search)
                //                                        || e.Actor.Contains(search)
                //                                        || e.Director.Contains(search))
                //                                        .Skip(skip)
                //                                        .Take(8);



                //var movies = await movieService.GetVideosAsync(null, skip, 8, search).ConfigureAwait(false);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                return Content(json);
            }
            return NotFound();
        }
    }
}