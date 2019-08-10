using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Study.Database;
using Study.Database.Entity;
using Study.Database.Entity.ArticalEntities;
using Study.Dto;
using Study.Dto.ArticalDto;
using Z.EntityFramework.Plus;

namespace Study.Services.ArticalService
{
    public class ArticalService : IArticalService
    {
        private readonly AppDatabaseContext db;
        private readonly IMemoryCache _cache;

        public ArticalService(AppDatabaseContext db, IMemoryCache cache)
        {
            this.db = db;
            _cache = cache;
        }


        public async Task<Respone> AddArticalClassification(AddArticalClassificationDto dto)
        {
            Respone respone = new Respone();
            do
            {
                var exist = await db.ArticalClassifications.AnyAsync(e => e.Name == dto.Name);
                if (exist)
                {
                    respone.ErrorMessage = "已经存在相同名称分类";
                    break;
                }

                await db.ArticalClassifications.AddAsync(new ArticalClassification
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = dto.Name,
                    AddDateTime = DateTime.Now
                });

                await db.SaveChangesAsync();

                respone.IsOk = true;

            } while (false);
            return respone;
        }


        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <returns></returns>
        public async Task<List<ArticalClassificationDto>> GetArticalClassificationsAsync()
        {
            var result = _cache.Get<List<ArticalClassificationDto>>("GetArticalClassifications");
            if (result == null)
            {
                result = await db.ArticalClassifications.Select(e => new ArticalClassificationDto
                {
                    Id = e.Id,
                    AddDateTime = e.AddDateTime,
                    Name = e.Name
                }).ToListAsync();

                _cache.Set("GetArticalClassifications", result);
            }
            return result;
        }


        /// <summary>
        /// 通过分类id获取分类信息
        /// </summary>
        /// <returns></returns>
        public async Task<ArticalClassificationDto> GetArticalClassificationByIdAsync(string classificationId)
        {
            return (await GetArticalClassificationsAsync()).FirstOrDefault(e => e.Id == classificationId);
        }


        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<Respone<ArticalDto>> AddArticalAsync(AddArticalDto dto)
        {
            Respone<ArticalDto> respone = new Respone<ArticalDto>();
            do
            {
                var existClassification = await db.ArticalClassifications.AnyAsync(e => e.Id == dto.ClassificationId);
                if (!existClassification)
                {
                    respone.ErrorMessage = "分类不存在";
                    break;
                }

                var artical = new Artical
                {
                    Id = Utility.NewGuid(),
                    ClassificationId = dto.ClassificationId,
                    Content = dto.Content,
                    PublishTime = DateTime.Now,
                    Title = dto.Title,
                    UserId = dto.UserId
                };

                await db.Articals.AddAsync(artical);

                await db.SaveChangesAsync();

                respone.Data = new ArticalDto
                {
                    Id = artical.Id,
                    ClassificationId = artical.ClassificationId,
                    UserId = artical.UserId,
                    Content = artical.Content,
                    PublishTime = artical.PublishTime,
                    Title = artical.Title
                };

                respone.IsOk = true;

            } while (false);
            return respone;
        }

        /// <summary>
        /// 获取所有文章
        /// </summary>
        /// <returns></returns>
        public IQueryable<ArticalDto> GetArticals()
        {
            return db.Articals.Select(e => new ArticalDto
            {
                Id = e.Id,
                ClassificationId = e.ClassificationId,
                Content = null,
                PublishTime = e.PublishTime,
                Title = e.Title,
                UserId = e.UserId,
                VisitCount = e.VisitCount
            });
        }

        /// <summary>
        /// 获取一篇文章
        /// </summary>
        /// <param name="articalId"></param>
        /// <returns></returns>
        public async Task<ArticalDto> GetArticalByIdAsync(string id)
        {
            return await db.Articals.
               Where(e => e.Id == id)
               .Select(e => new ArticalDto
               {
                   Id = e.Id,
                   ClassificationId = e.ClassificationId,
                   Content = e.Content,
                   PublishTime = e.PublishTime,
                   Title = e.Title,
                   UserId = e.UserId,
                   VisitCount = e.VisitCount
               }).FirstOrDefaultAsync();
        }


        /// <summary>
        /// 更新一篇文章
        /// </summary>
        /// <param name="id">文章id</param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<Respone<ArticalDto>> UpdateArticalAsync(string id, UpdateArticalDto dto)
        {
            Respone<ArticalDto> respone = new Respone<ArticalDto>();
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
                respone.Data = new ArticalDto
                {
                    Id = artical.Id,
                    ClassificationId = artical.ClassificationId,
                    UserId = artical.UserId,
                    Content = artical.Content,
                    PublishTime = artical.PublishTime,
                    Title = artical.Title,
                    VisitCount = artical.VisitCount
                };
                respone.IsOk = true;
            } while (false);
            return respone;
        }


        /// <summary>
        /// 删除一篇文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Respone> DeleteArticalAsync(string id)
        {
            Respone respone = new Respone();
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
                respone.IsOk = true;
            } while (false);
            return respone;
        }


        /// <summary>
        /// 添加文章评论
        /// </summary>
        /// <param name="id">文章id</param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<Respone> AddArticalCommentAsync(AddArticalCommentDto dto)
        {
            Respone respone = new Respone();
            do
            {
                var existArtical = await db.Articals.AnyAsync(e => e.Id == dto.ArticalId);
                if (!existArtical)
                {
                    respone.ErrorMessage = "文章不存在";
                    break;
                }

                await db.ArticalComments.AddAsync(new ArticalComment
                {
                    Id = Utility.NewGuid(),
                    ArticalId = dto.ArticalId,
                    CommenterId = dto.CommenterId,
                    CommentTime = DateTime.Now,
                    CommentContent = dto.CommentContent
                });

                await db.SaveChangesAsync();

                respone.IsOk = true;
            } while (false);
            return respone;
        }


        /// <summary>
        /// 获取一篇文章评论
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns></returns>
        public IQueryable<ArticalCommentDto> GetArticalComments(string id)
        {
            return db.ArticalComments
                .Where(e => e.ArticalId == id)
                .Select(e => new ArticalCommentDto
                {
                    ArticalId = e.ArticalId,
                    CommentContent = e.CommentContent,
                    CommenterId = e.CommenterId,
                    CommentTime = e.CommentTime,
                    Id = e.Id
                });
        }
    }
}
