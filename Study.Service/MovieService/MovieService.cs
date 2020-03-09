using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Study.Database;
using Study.Database.Entity;
using Study.Database.VideoDb;
using Study.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study.Service.MovieService
{
    public class MovieService : IMovieService
    {
        private readonly VideoDbContext videoDbContext;
        private readonly UserDatabaseContext context;
        private readonly CacheService cacheService;
        private readonly IDataProtector protector;

        public MovieService(UserDatabaseContext context,
            IDataProtectionProvider provider,
            CacheService cacheService,
            VideoDbContext videoDbContext)
        {
            this.videoDbContext = videoDbContext;
            this.context = context;
            this.cacheService = cacheService;
            protector = provider.CreateProtector("movie");
        }



        /// <summary>
        /// 获取所有视频分类
        /// </summary>
        /// <returns></returns>
       // [Log(ActionName = "GetVideoTypesAsync")]
        public async Task<List<VideoType>> GetVideoTypesAsync()
        {
            var types = await cacheService.GetAsync<List<VideoType>>("GetMovieClassifications");
            if (types == null)
            {
                types = await videoDbContext.VideoTypes.Where(e => e.ShowInPage == true).ToListAsync();
                await cacheService.SetAsync("GetMovieClassifications", types, TimeSpan.FromDays(1));
            }

            return types;
        }

        /// <summary>
        /// 通过id获取视频分类
        /// </summary>
        /// <param name="videoTypeId"></param>
        /// <returns></returns>
        public async Task<VideoType> GetVideoTypeByIdAsync(int videoTypeId)
        {
            return (await GetVideoTypesAsync()).FirstOrDefault(e => e.Id == videoTypeId);
        }





        /// <summary>
        /// 删除电影
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result> DeleteMovieAsync(string id)
        {
            Result result = new Result();
            try
            {
                //do
                //{
                //    var rowId = protector.Unprotect(id);
                //    if (!long.TryParse(rowId, out long longId))
                //    {
                //        result.ErrorMessage = ErrorMessages.ParseIdFaild;
                //        break;
                //    }

                //    var movie = await context.Movies.FirstOrDefaultAsync(e => e.Id == longId);
                //    if (movie == null)
                //    {
                //        result.ErrorMessage = ErrorMessages.NotFound;
                //        break;
                //    }

                //    context.Movies.Remove(movie);
                //    await context.SaveChangesAsync();
                //    result.Succeeded = true;
                //} while (false);
            }
            catch (Exception ex)
            {

            }

            return result;
        }


        /// <summary>
        ///  分页获取视频
        /// </summary>
        /// <param name="typeIds">分类id数组 可以null</param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public async Task<List<Video>> GetVideosAsync(int[] typeIds, int skip,int take)
        {
            string key = string.Empty;
            if(typeIds == null || typeIds.Length == 0)
            {
                key = $"GetVideos_{skip}_{take}";
            }
            else
            {
                key = $"GetVideos_{string.Join("_", typeIds)}_{skip}_{take}";
            }
            List<Video> videos = await cacheService.GetAsync<List<Video>>(key);
            if (videos == null)
            {
                IQueryable<Video> query = videoDbContext.Videos.Where(e => e.ShowInPage == true);

                if (typeIds != null)
                {
                    if (typeIds.Length == 1)
                    {
                        query = query.Where(e => e.VideoTypeId == typeIds[0]);
                    }
                    else
                    {
                        query = query.Where(e => typeIds.Contains(e.VideoTypeId));
                    }
                    
                }

                videos = await query
                .OrderByDescending(e => e.LastUpdateTime)
                .ThenByDescending(e => e.Year)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

                await cacheService.SetAsync(key, videos, TimeSpan.FromMinutes(10));
            }

            return videos;
        }

        /// <summary>
        /// 获取视频
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        public async Task<Video> GetVideoByIdAsync(int videoId)
        {
            var key = $"GetVideoByIdAsync_{videoId}";
            Video video = await cacheService.GetAsync<Video>(key);
            if(video == null)
            {
                video = await videoDbContext.Videos.FirstOrDefaultAsync(e => e.Id == videoId);
                await cacheService.SetAsync(key, video, TimeSpan.FromMinutes(10));
            }

            //if (video != null && video.LastUpdateTime.HasValue)
            //{
            //    video.LastUpdateTime = DateTimeOffset.Parse(video.LastUpdateTime.Value.LocalDateTime.ToString());
            //}
            return video;
        }


        ///// <summary>
        ///// 根据条件查询视频
        ///// </summary>
        ///// <param name="SubClassificationId"></param>
        ///// <param name="skip"></param>
        ///// <param name="take"></param>
        ///// <param name="search"></param>
        ///// <returns></returns>
        //public async Task<List<MovieDto>> GetVideosAsync(long? SubClassificationId, int? skip, int? take, string search = null)
        //{
        //    var movies = context.Videos.Select(video => new MovieDto {
        //        VideoId = protector.Protect(video.VideoId.ToString()),
        //        Nation = video.Nation,
        //        Name = video.Name,
        //        ImageUrl = video.ImageUrl,
        //        SubClassificationId = video.SubClassificationId,
        //        BigClassificationId = video.BigClassificationId,
        //        DateTime = video.DateTime,
        //        Desc = video.Desc,
        //        Director = video.Director,
        //        StarringActor = video.StarringActor,
        //        UploadDateTime = video.UploadDateTime
        //    });

        //    //根据分类id查询
        //    if (SubClassificationId.HasValue)
        //    {
        //        movies = movies.Where(e => e.SubClassificationId == SubClassificationId);
        //    }

        //    //根据关键词查询
        //    if(!string.IsNullOrEmpty(search))
        //    {
        //        movies = movies.Where(e => e.Name.Contains(search)
        //                                                || e.Director.Contains(search)
        //                                                || e.StarringActor.Contains(search));
        //    }

        //    //时间降序
        //    movies = movies.OrderByDescending(e => e.UploadDateTime)
        //            .ThenByDescending(e => e.DateTime)
        //            .Skip(skip??0)
        //            .Take(take ?? 0);

        //    return await movies.ToListAsync();
        //}


        ///// <summary>
        ///// 获取某部电影
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public async Task<MovieDto> GetMovieByIdAsync(string id)
        //{
        //    try
        //    {
        //        var rawId = protector.Unprotect(id);
        //        if (int.TryParse(rawId, out int rawIdLong))
        //        {
        //            return await context.Videos.Where(e => e.VideoId == rawIdLong).Select(video => new MovieDto {
        //                VideoId = protector.Protect(video.VideoId.ToString()),
        //                Nation = video.Nation,
        //                Name = video.Name,
        //                ImageUrl = video.ImageUrl,
        //                SubClassificationId = video.SubClassificationId,
        //                BigClassificationId = video.BigClassificationId,
        //                DateTime = video.DateTime,
        //                Desc = video.Desc,
        //                Director = video.Director,
        //                StarringActor = video.StarringActor,
        //                UploadDateTime = video.UploadDateTime
        //            }).FirstOrDefaultAsync();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return null;
        //}
    }
}
