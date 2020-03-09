using Study.Service._Configure;
using Study.Services.ArticalService.Req;
using Study.Services.ArticalService.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Study.Services.ArticalService
{
    /// <summary>
    /// 文章服务接口 自动注入
    /// </summary>
    [AutoInject(typeof(ArticlesService))]
    public interface IArticalService
    {
        #region 文章分类
        /// <summary>
        /// 添加文章分类
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<Result> AddArticalClassificationAsync(AddArticalClassificationDto dto);


        /// <summary>
        /// 获取文章分类
        /// </summary>
        /// <returns></returns>
        Task<List<ArticalClassificationDto>> GetArticalClassificationsAsync();


        /// <summary>
        /// 通过分类id获取分类信息
        /// </summary>
        /// <returns></returns>
        Task<ArticalClassificationDto> GetArticalClassificationByIdAsync(long classificationId);



        /// <summary>
        /// 更新文章分类
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<Result> UpdateArticalClassificationAsync(long id, UpdateArticalClassificationDto dto);


        /// <summary>
        /// 删除文章分类
        /// </summary>
        /// <param name="id">分类id</param>
        /// <returns></returns>
        Task<Result> DeleteArticalClassificationAsync(long id);
        #endregion



        #region 文章
        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>返回文章id</returns>
        Task<Result<long>> AddArticalAsync(AddArticalDto dto);


        /// <summary>
        /// 获取文章集合(不包含内容)
        /// </summary>
        /// <param name="ClassificationId">分类id</param>
        /// <param name="skip">跳过N条</param>
        /// <param name="take">取N条</param>
        /// <returns></returns>
        Task<List<ArticalDto>> GetArticlesAsync(Func<IQueryable<ArticalDto>, IQueryable<ArticalDto>> func = null,
                                                                int? skip = 0,
                                                                int? take = 10);

        /// <summary>
        /// 根据条件判断文章是否存在
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<bool> ArticleExistAsync(Expression<Func<ArticalDto, bool>> expression);

        /// <summary>
        /// 根据条件获取文章数量
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<int> ArticleCountAsync(Expression<Func<ArticalDto, bool>> expression);


        /// <summary>
        /// 获取一篇文章（包含内容）
        /// </summary>
        /// <param name="articalId"></param>
        /// <returns></returns>
        Task<ArticalDto> GetArticalByIdAsync(long articalId);


        /// <summary>
        /// 删除一篇文章
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns></returns>
        Task<Result> DeleteArticalAsync(long id);


        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<Result> UpdateArticalAsync(long id, UpdateArticalDto dto);


        /// <summary>
        /// 设置文章置顶
        /// </summary>
        /// <param name="articalId"></param>
        /// <param name="isSetTop"></param>
        /// <returns></returns>
        Task<Result> SetArticleTopAsync(long articalId, bool isSetTop);
        #endregion



        #region 文章评论
        /// <summary>
        /// 添加文章评论
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<Result> AddArticalCommentAsync(AddArticalCommentDto dto);



        /// <summary>
        /// 获取一篇文章所有评论
        /// </summary>
        /// <param name="articalId"></param>
        /// <returns></returns>
        IQueryable<ArticalCommentDto> GetArticalComments(long articalId);


        /// <summary>
        /// 删除文章评论
        /// </summary>
        Task<Result> DeleteArticalCommentAsync(long commentId);
        #endregion
    }
}
