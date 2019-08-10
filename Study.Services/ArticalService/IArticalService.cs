using Study.Dto;
using Study.Dto.ArticalDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Study.Services.ArticalService
{
    public interface IArticalService
    {
        #region 文章分类
        /// <summary>
        /// 添加文章分类
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<Respone> AddArticalClassification(AddArticalClassificationDto dto);


        /// <summary>
        /// 获取文章分类
        /// </summary>
        /// <returns></returns>
        Task<List<ArticalClassificationDto>> GetArticalClassificationsAsync();


        /// <summary>
        /// 通过分类id获取分类信息
        /// </summary>
        /// <returns></returns>
        Task<ArticalClassificationDto> GetArticalClassificationByIdAsync(string classificationId);
        #endregion



        #region 文章
        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<Respone<ArticalDto>> AddArticalAsync(AddArticalDto dto);

        /// <summary>
        /// 获取所有文章（不含内容）
        /// </summary>
        /// <returns></returns>
        IQueryable<ArticalDto> GetArticals();

        /// <summary>
        /// 获取一篇文章（包含内容）
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns></returns>
        Task<ArticalDto> GetArticalByIdAsync(string id);


        /// <summary>
        /// 删除一篇文章
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns></returns>
        Task<Respone> DeleteArticalAsync(string id);


        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<Respone<ArticalDto>> UpdateArticalAsync(string id, UpdateArticalDto dto);
        #endregion



        #region 文章评论
        /// <summary>
        /// 添加文章评论
        /// </summary>
        /// <param name="id">文章id</param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<Respone> AddArticalCommentAsync(AddArticalCommentDto dto);



        /// <summary>
        /// 获取一篇文章评论
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns></returns>
        IQueryable<ArticalCommentDto> GetArticalComments(string id);
        #endregion
    }
}
