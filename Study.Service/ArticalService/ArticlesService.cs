using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Profiling;
using Study.Database;
using Study.Database.Entity;
using Study.Database.Entity.ArticalEntities;
using Study.Service;
using Study.Services.ArticalService.Req;
using Study.Services.ArticalService.Res;
using Study.Services.UserService;
using Study.Services.UserService.Res;
using Utility;
using Z.EntityFramework.Plus;


namespace Study.Services.ArticalService
{
    /// <summary>
    /// 文章服务类
    /// </summary>
    public class ArticlesService : IArticalService
    {
        private readonly UserDatabaseContext db;
       // private readonly IUserService userService;
        private readonly CacheService cacheService;

        private const string CacheStringArticlesTypes = "CacheStringArticlesTypes";

        public ArticlesService(UserDatabaseContext db,
            //IUserService userService,
            CacheService cacheService)
        {
            this.db = db;
           // this.userService = userService;
            this.cacheService = cacheService;
        }

        /// <summary>
        /// 添加文章分类
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<Result> AddArticalClassificationAsync(AddArticalClassificationDto dto)
        {
            Result respone = new Result();
            do
            {
                //using(var step= MiniProfiler.Current.Step("添加文章分类"))
                {
                    var exist = await db.ArticalClassifications.AnyAsync(e => e.Name == dto.Name);
                    if (exist)
                    {
                        respone.ErrorMessage = "已经存在相同名称分类";
                        break;
                    }

                    await db.ArticalClassifications.AddAsync(new ArticleClassification {
                        Id = GuidEx.NewGuid(),
                        Name = dto.Name,
                    });

                    await db.SaveChangesAsync();

                    //移除cache
                    await cacheService.RemoveAsync(CacheStringArticlesTypes);

                    respone.Succeeded = true;
                }
            } while (false);
            return respone;
        }


        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <returns></returns>
        public async Task<List<ArticalClassificationDto>> GetArticalClassificationsAsync()
        {
            var result = await cacheService.GetAsync<List<ArticalClassificationDto>>(CacheStringArticlesTypes);
            if (result == null)
            {
                result = await db.ArticalClassifications
                    .Select(e => new ArticalClassificationDto {
                        Id = e.Id,
                        Name = e.Name
                    })
                    .ToListAsync();

               await cacheService.SetAsync(CacheStringArticlesTypes, result, TimeSpan.FromDays(1));
            }
            return result;
        }


        /// <summary>
        /// 通过分类id获取分类信息
        /// </summary>
        /// <returns></returns>
        public async Task<ArticalClassificationDto> GetArticalClassificationByIdAsync(long classificationId)
        {
            return (await GetArticalClassificationsAsync()).FirstOrDefault(e => e.Id == classificationId);
        }



        /// <summary>
        /// 更新文章分类
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<Result> UpdateArticalClassificationAsync(long id, UpdateArticalClassificationDto dto)
        {
            Result respone = new Result();
            do
            {
                if (id != dto.Id)
                {
                    respone.ErrorMessage = "id和dto的id不一致";
                    break;
                }


                var classification = await db.ArticalClassifications.FirstOrDefaultAsync(e => e.Id == id);
                if (classification == null)
                {
                    respone.ErrorMessage = "分类不存在";
                    break;
                }

                classification.Name = dto.Name;
                await db.SaveChangesAsync();

                await cacheService.RemoveAsync(CacheStringArticlesTypes);

                respone.Succeeded = true;

            } while (false);
            return respone;
        }


        /// <summary>
        /// 删除文章分类
        /// </summary>
        /// <param name="id">分类id</param>
        /// <returns></returns>
        public async Task<Result> DeleteArticalClassificationAsync(long id)
        {
            Result respone = new Result();
            do
            {
                var c = await db.ArticalClassifications.FirstOrDefaultAsync(e => e.Id == id);
                if (c == null)
                {
                    respone.ErrorMessage = "分类不存在";
                    break;
                }


                //分类下的文章数量==0 就可以删除
                var count = await db.Articals.CountAsync(e => e.ClassificationId == id);
                if (count > 0)
                {
                    respone.ErrorMessage = "分类下面还有文章，不能删除改分类";
                    break;
                }


                db.ArticalClassifications.Remove(c);

                await db.SaveChangesAsync();

                await cacheService.RemoveAsync(CacheStringArticlesTypes);

                respone.Succeeded = true;

            } while (false);
            return respone;
        }


        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<Result<long>> AddArticalAsync(AddArticalDto dto)
        {
            Result<long> respone = new Result<long>();
            do
            {
                var existClassification = await db.ArticalClassifications.AnyAsync(e => e.Id == dto.ClassificationId);
                if (!existClassification)
                {
                    respone.ErrorMessage = "分类不存在";
                    break;
                }

                var id = GuidEx.NewGuid();

                await db.Articals.AddAsync(new Article {
                    Id = id,
                    ClassificationId = dto.ClassificationId,
                    Content = dto.Content,
                    PublishTime = DateTime.Now,
                    Title = dto.Title,
                    UserId = dto.UserId,
                    VisitCount = 0
                });

                await db.SaveChangesAsync();

                respone.Data = id;

                respone.Succeeded = true;

            } while (false);
            return respone;
        }


        private IQueryable<ArticalDto> GetArticals()
        {
            return db.Articals.Select(e =>
            new ArticalDto {
                Id = e.Id,
                Content = e.Content,
                PublishTime = e.PublishTime,
                Title = e.Title,
                VisitCount = e.VisitCount,
                ClassificationId = e.ClassificationId,
                UserId = e.UserId,
                IsSetTop = e.IsSetTop,
                SetTopDateTime = e.SetTopDateTime
            });
        }


        /// <summary>
        /// 获取文章集合(不包含内容)
        /// </summary>
        /// <param name="ClassificationId">分类id</param>
        /// <param name="skip">跳过N条</param>
        /// <param name="take">取N条</param>
        /// <returns></returns>
        public async Task<List<ArticalDto>> GetArticlesAsync(Func<IQueryable<ArticalDto>, IQueryable<ArticalDto>> func = null,
                                                                int? skip = 0,
                                                                int? take = 10)
        {
            var articles = func?.Invoke(GetArticals());

            skip = skip ?? 0;
            take = take ?? 10;

            articles = articles.Skip(skip.Value).Take(take.Value);

            return await articles.ToListAsync();
        }


        /// <summary>
        /// 根据条件判断文章是否存在
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<bool> ArticleExistAsync(Expression<Func<ArticalDto, bool>> expression)
        {
            return await GetArticals().AnyAsync(expression);
        }

        /// <summary>
        /// 根据条件获取文章数量
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<int> ArticleCountAsync(Expression<Func<ArticalDto, bool>> expression)
        {
            return await GetArticals().CountAsync(expression);
        }

        /// <summary>
        /// 获取一篇文章
        /// </summary>
        /// <param name="articalId"></param>
        /// <returns></returns>
        public async Task<ArticalDto> GetArticalByIdAsync(long articalId)
        {
            var query = await GetArticals()
                .Where(e => e.Id == articalId)
                .FirstOrDefaultAsync();
            return query;
        }


        /// <summary>
        /// 设置文章置顶
        /// </summary>
        /// <param name="articalId"></param>
        /// <returns></returns>
        public async Task<Result> SetArticleTopAsync(long articalId, bool isSetTop)
        {
            Result result = new Result();
            do
            {
                var artical = await db.Articals.FirstOrDefaultAsync(e => e.Id == articalId);
                if (artical == null)
                {
                    result.ErrorMessage = ErrorMessages.NotFound;
                    break;
                }

                artical.IsSetTop = isSetTop;
                if (isSetTop)
                {
                    artical.SetTopDateTime = DateTimeOffset.Now;
                }
                else
                {
                    artical.SetTopDateTime = DateTimeOffset.MinValue;
                }

                await db.SaveChangesAsync();

                result.Succeeded = true;
            } while (false);
            return result;
        }

        /// <summary>
        /// 更新一篇文章
        /// </summary>
        /// <param name="id">文章id</param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<Result> UpdateArticalAsync(long id, UpdateArticalDto dto)
        {
            Result respone = new Result();
            do
            {
                if (id != dto.ArticalId)
                {
                    respone.ErrorMessage = "id和模型id不一致";
                    break;
                }

                var artical = await db.Articals.FirstOrDefaultAsync(e => e.Id == dto.ArticalId);
                if (artical == null)
                {
                    respone.ErrorMessage = "文章不存在";
                    break;
                }

                artical.Title = dto.Title;
                artical.ClassificationId = dto.ClassificationId;
                artical.Content = dto.Content;
                artical.VisitCount = dto.VisitCount;

                await db.SaveChangesAsync();
                respone.Succeeded = true;
            } while (false);
            return respone;
        }


        /// <summary>
        /// 删除一篇文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result> DeleteArticalAsync(long id)
        {
            Result respone = new Result();
            do
            {
                var artical = await db.Articals.FirstOrDefaultAsync(e => e.Id == id);
                if (artical == null)
                {
                    respone.ErrorMessage = "文章不存在";
                    break;
                }

                //获取这篇文章的评论 先删除评论
                var comments = db.ArticalComments.Where(e => e.ArticalId == artical.Id);
                db.ArticalComments.RemoveRange(comments);

                //再删除文章
                db.Articals.Remove(artical);

                await db.SaveChangesAsync();
                respone.Succeeded = true;
            } while (false);
            return respone;
        }


        /// <summary>
        /// 添加文章评论
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<Result> AddArticalCommentAsync(AddArticalCommentDto dto)
        {
            Result respone = new Result();
            do
            {
                var existArtical = await db.Articals.AnyAsync(e => e.Id == dto.ArticalId);
                if (!existArtical)
                {
                    respone.ErrorMessage = "文章不存在";
                    break;
                }


                await db.ArticalComments.AddAsync(new ArticleComment {
                    ArticalId = dto.ArticalId,
                    CommentContent = dto.CommentContent,
                    CommenterId = dto.CommenterId,
                    CommentTime = DateTime.Now,
                    Id = GuidEx.NewGuid()
                });

                await db.SaveChangesAsync();

                respone.Succeeded = true;
            } while (false);
            return respone;
        }


        /// <summary>
        /// 获取一篇文章评论
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns></returns>
        public IQueryable<ArticalCommentDto> GetArticalComments(long articalId)
        {
            return db.ArticalComments
                .Where(e => e.ArticalId == articalId)
                .Select(e =>
                new ArticalCommentDto {
                    Id = e.Id,
                    ArticalId = e.ArticalId,
                    CommentContent = e.CommentContent,
                    CommentTime = e.CommentTime,
                    CommenterId = e.CommenterId
                });
        }

        /// <summary>
        /// 删除文章评论
        /// </summary>
        public async Task<Result> DeleteArticalCommentAsync(long commentId)
        {
            Result result = new Result();
            do
            {
                var comment = await db.ArticalComments.FirstOrDefaultAsync(e => e.Id == commentId);
                if (comment == null)
                {
                    result.ErrorMessage = ErrorMessages.NotFound;
                    break;
                }

                db.Remove(comment);
                await db.SaveChangesAsync();

                result.Succeeded = true;
            } while (false);
            return result;
        }
    }
}
