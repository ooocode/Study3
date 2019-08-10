using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Study.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Study.WebApp
{
    public class MovieInfo
    {
        public string Title { get; set; }
        public string Url { get; set; }

        public void Log()
        {
            Console.WriteLine($"【{Title}】 --》 {Url}");
        }
    }

    public class MovieParser
    {
        private readonly HtmlWeb web;
        private  AppDatabaseContext _context;
        public MovieParser()
        {
            _context = new AppDatabaseContext();
            web = new HtmlWeb();

            HtmlWeb.PreRequestHandler handler = delegate (HttpWebRequest request)
            {
                request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.CookieContainer = new System.Net.CookieContainer();
                return true;
            };
            web.PreRequest += handler;
        }

        List<MovieInfo> movies = new List<MovieInfo>();


        //(1~100000数据)

        //public async Task Start()
        //{
        //    await ThreadFunc(startPage, endPage);

        //}

        public async Task ThreadFunc(int startIndex, int endIndex)
        {
            for (int i = startIndex; i <= endIndex; i++)
            {
           // http:///index.php/vod/play/id/35624/sid/1/nid/1.html
                // From Web
                var url = $"http://ly.flsjbq.vip/index.php/vod/play/id/{i}/sid/1/nid/1.html";

                var info = GetMovieInfoAsync(url);
                if (info != null)
                {
                    info.Log();
                    var exist = await _context.Movies.AnyAsync(e => e.Url == info.Url);
                    if (!exist)
                    {
                        await _context.Movies.AddAsync(new Database.Entity.Movie
                        {
                            Id = Study.Dto.Utility.NewGuid(),
                            Title = info.Title,
                            Url = info.Url
                        });

                        await _context.SaveChangesAsync();
                    }
                }
            }
            
        }

        MovieInfo GetMovieInfoAsync(string url)
        {
            try
            {

                var doc = web.Load(url);

               // Console.WriteLine(doc.ParsedText);
                var title = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/div[1]/div[1]/div[2]/h1/a").InnerText;

                var script = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/div[1]/div[1]/div[1]/script[1]");
                var videoUrl = (string)Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(script.InnerText.Replace("var player_data=", "")).url;

                if(!string.IsNullOrEmpty(videoUrl) && !string.IsNullOrEmpty(title))
                {
                    return new MovieInfo
                    {
                        Title = title,
                        Url = videoUrl
                    };
                }
            
            }
            catch (Exception ex)
            {
                //Console.WriteLine("GetMovieInfoAsync "+ex.Message);
            }
            return null;
        }
    }
}
