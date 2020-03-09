using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Study.Database;
using Study.Database.VideoDb;
using Study.Service.MovieService;


namespace Study.Website.Pages.Movie
{
    public class DetailModel : PageModel
    {
        private readonly IMovieService movieService;
        private readonly VideoDbContext videoDbContext;
        public readonly AdvertisementService advertisementService;

        /// <summary>
        /// 路由传递的视频id
        /// </summary>
        [FromRoute(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// 路由传递的视频集数  第几集？
        /// </summary>
        [FromRoute(Name = "Index")]
        public int? Index { get; set; }


        /// <summary>
        /// 视频信息
        /// </summary>
        public Video Video { get; set; }


        /// <summary>
        /// 视频类型信息
        /// </summary>
        public VideoType VideoType { get; set; }



        public DetailModel(IMovieService movieService, VideoDbContext videoDbContext, AdvertisementService advertisementService)
        {
            this.movieService = movieService;
            this.videoDbContext = videoDbContext;
            this.advertisementService = advertisementService;
        }

        /// <summary>
        /// 获取视频集数对应的播放链接  返回文字描述 + 播放链接 + 总集数
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="index">集数下标 从1开始</param>
        public async Task<(string str, string url,int count)> GetVideoUrlAsync(int videoId, int? index)
        {
            index = index ?? 1;
            index = index - 1;

            //暂时只选择m3u8格式的
            var videoUrl = await videoDbContext.VideoUrls
                .FirstOrDefaultAsync(e => e.VideoId == videoId && e.Flag == "ckm3u8")
                .ConfigureAwait(false);

            if (videoUrl == null)
            {
               videoUrl = await videoDbContext.VideoUrls
                .FirstOrDefaultAsync(e => e.VideoId == videoId)
                .ConfigureAwait(false);

                if(videoUrl == null)
                {
                    throw new Exception("找不到视频播放链接");
                }
            }

            var arr = videoUrl.Url.Split('#');

            try
            {
                var indexStr = arr[index.Value];

                var result = indexStr.Split('$');

                return (result[0], result[1], arr.Length);
            }
            catch(Exception ex)
            {
                
            }
            return ("", "", arr.Length);
        }

        public (string str, string url, int count) IndexInfo;


        /// <summary>
        /// 页面加载时
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync()
        {
            //根据id查找视频
            Video = await movieService.GetVideoByIdAsync(Id).ConfigureAwait(false);
            if (Video == null)
            {
                return NotFound();
            }

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(Video.Desc);
            Video.Desc = htmlDoc.DocumentNode.InnerText;

            htmlDoc.LoadHtml(Video.Actor);
            Video.Actor = htmlDoc.DocumentNode.InnerText;

            htmlDoc.LoadHtml(Video.Director);
            Video.Director = htmlDoc.DocumentNode.InnerText;

            htmlDoc.LoadHtml(Video.Area);
            Video.Area = htmlDoc.DocumentNode.InnerText;


            //查找这个视频的分类信息
            VideoType = await movieService.GetVideoTypeByIdAsync(Video.VideoTypeId).ConfigureAwait(false);

            Index = Index ?? 1;

            //集数信息
            IndexInfo = await GetVideoUrlAsync(Id, Index).ConfigureAwait(false);

         
            return Page();
        }


        /// <summary>
        /// 删除视频
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostDeleteMovieAsync(string Id)
        {
            //var result = await movieService.DeleteMovieAsync(Id).ConfigureAwait(false);
            //if (result.Succeeded)
            //{
            //    return RedirectToPage("/Movie/Index");
            //}

            return Page();
            //return BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// 获取短链接
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetShortLinkAsync(string url)
        {
            //http://suolink.cn/api.html
            var targetUrl = "http://api.suolink.cn/api.php?url="
                + url
                + "&key=5d9b0897b1a9c71c9e570d06@a08825609e8999eaa18a385cd0bf095b&expireDate=2030-03-31";

            using (HttpClient client = new HttpClient())
            {
                var result = await client.GetStringAsync(targetUrl).ConfigureAwait(false);
                return Content(result);
            }
        }
    }
}