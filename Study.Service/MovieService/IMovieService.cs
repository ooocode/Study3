using Study.Database.VideoDb;
using Study.Service._Configure;
using Study.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study.Service.MovieService
{
    /// <summary>
    /// 视频接口
    /// </summary>
    [AutoInject(typeof(MovieService))]
    public interface IMovieService
    {
        #region 电影分类

        /// <summary>
        /// 获取所有视频分类
        /// </summary>
        /// <returns></returns>
        Task<List<VideoType>> GetVideoTypesAsync();


        /// <summary>
        /// 通过id获取视频分类
        /// </summary>
        /// <param name="videoTypeId"></param>
        /// <returns></returns>
        Task<VideoType> GetVideoTypeByIdAsync(int videoTypeId);

        #endregion


        Task<List<Video>> GetVideosAsync(int[] typeIds, int skip,int take = 12);


        Task<Video> GetVideoByIdAsync(int videoId);

        ///// <summary>
        ///// 根据条件查询视频
        ///// </summary>
        ///// <param name="SubClassificationId"></param>
        ///// <param name="skip"></param>
        ///// <param name="take"></param>
        ///// <param name="search"></param>
        ///// <returns></returns>
        //Task<List<MovieDto>> GetVideosAsync(long? SubClassificationId, int? skip, int? take, string search = null);


        ///// <summary>
        ///// 获取某部电影
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //Task<MovieDto> GetMovieByIdAsync(string id);


        ///// <summary>
        ///// 删除电影
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //Task<Result> DeleteMovieAsync(string id);

        ///// <summary>
        ///// 获取某部视频的集数信息
        ///// </summary>
        ///// <param name="videoId"></param>
        ///// <returns></returns>
        //Task<List<VideoIndexDto>> GetVideoIndicesAsync(string videoId);
    }
}