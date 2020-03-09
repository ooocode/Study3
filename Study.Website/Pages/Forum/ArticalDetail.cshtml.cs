using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Study.Services.ArticalService.Req;
using Study.Services.ArticalService.Res;
using Study.Website.Pages;
using UserGrpcService;

namespace Study.WebApp.Pages.Forum
{
    public class ArticalDetailModel : AppPageModel
    {
        [FromRoute(Name = "ArticalId")]
        public long ArticalId { get; set; }



        /// <summary>
        /// 添加文章评论模型
        /// </summary>
        public AddArticalCommentDto addArticalCommentDto { get; set; }

        /// <summary>
        /// 文章
        /// </summary>
        public ArticalDto Artical { get; set; }


        /// <summary>
        /// 文章的分类信息
        /// </summary>
        public ArticalClassificationDto ClassificationInfo { get; set; }

        /// <summary>
        /// 作者信息
        /// </summary>
        public UserReply Author { get; set; }


        /// <summary>
        /// 评论
        /// </summary>
        public IQueryable<ArticalCommentDto> Comments { get; set; }

        /// <summary>
        /// 评论数量
        /// </summary>
        public int CommentsCount { get; set; } = 0;

        /// <summary>
        /// 作者其他文章
        /// </summary>
        public List<ArticalDto> AuthorOtherArticals { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            //获取这篇文章
            Artical = await ArticalService.GetArticalByIdAsync(ArticalId).ConfigureAwait(false);
            if (Artical == null)
            {
                return NotFound();
            }

            //作者信息
            Author = await UserClient.FindByIdAsync(new UserIdReq { UserId = Artical.UserId });

            //文章分类信息
            ClassificationInfo = await ArticalService.GetArticalClassificationByIdAsync(Artical.ClassificationId).ConfigureAwait(false);

            //阅读量+1
            await ArticalService.UpdateArticalAsync(ArticalId, new UpdateArticalDto
            {
                ArticalId = ArticalId,
                VisitCount = Artical.VisitCount + 1,
                ClassificationId = Artical.ClassificationId,
                Content = Artical.Content,
                Title = Artical.Title
            }).ConfigureAwait(false);


            //评论列表
            Comments = ArticalService.GetArticalComments(Artical.Id).OrderByDescending(e => e.CommentTime);

            //评论数量
            CommentsCount = await Comments.CountAsync().ConfigureAwait(false);

            //获取作者其他文章且不包含这篇文章
            AuthorOtherArticals = await ArticalService.GetArticlesAsync(
                articles =>
                {
                    return articles.Where(e => e.Id != ArticalId).OrderByDescending(e => e.PublishTime);
                }, take: 5).ConfigureAwait(false);

            return Page();
        }



        /// <summary>
        /// 添加评论
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(AddArticalCommentDto addArticalCommentDto)
        {
            do
            {
                if (!TryValidateModel(addArticalCommentDto))
                {
                    break;
                }

                var respone = await ArticalService.AddArticalCommentAsync(addArticalCommentDto).ConfigureAwait(false);
                if (respone.Succeeded)
                {
                    return Content(addArticalCommentDto.CommentContent);
                }
            } while (false);
            return BadRequest(ModelState);
        }


        //删除文章
        public async Task<IActionResult> OnPostDeleteAsync(long articalId)
        {
            if (articalId > 0)
            {
                var artical = await ArticalService.GetArticalByIdAsync(articalId).ConfigureAwait(false);
                if (artical == null)
                {
                    return NotFound();
                }

                //是否被授权
                AuthorizationResult authorizationResult = await AuthorizationService.AuthorizeAsync(
                    User, Permissons.Permisson.Article_Delete).ConfigureAwait(false);
                if (!authorizationResult.Succeeded)
                {
                    return Forbid();
                }

                //删除文章
                var resone = await ArticalService.DeleteArticalAsync(articalId).ConfigureAwait(false);
                if (resone.Succeeded)
                {
                    return new NoContentResult();
                }
                ModelState.AddModelError(string.Empty, resone.ErrorMessage);
            }
            return BadRequest(ModelState);
        }


        /// <summary>
        /// 删除文章评论
        /// </summary>
        /// <param name="articalId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostDeleteArticalComment(long commentId)
        {
            if (commentId == 0)
            {
                ModelState.AddModelError(string.Empty, "无效Id");
                return BadRequest(ModelState);
            }

            //是否被授权
            AuthorizationResult authorizationResult = await AuthorizationService.AuthorizeAsync(
                User,Permissons.Permisson.Article_Comment_Delete).ConfigureAwait(false);

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var result = await ArticalService.DeleteArticalCommentAsync(commentId).ConfigureAwait(false);
            if (result.Succeeded)
            {
                return Content("ok");
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return BadRequest();
            }
        }


        /// <summary>
        /// 设置文章置顶
        /// </summary>
        /// <param name="articalId"></param>
        /// <param name="isSetTop"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostSetTopAsync(long articalId, bool isSetTop)
        {
            var artical = await ArticalService.GetArticalByIdAsync(articalId).ConfigureAwait(false);
            if (artical == null)
            {
                return NotFound();
            }

            //是否被授权
            AuthorizationResult authorizationResult = await AuthorizationService.AuthorizeAsync(
                User, Permissons.Permisson.Article_SetTop).ConfigureAwait(false);

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            //文章置顶
            var result = await ArticalService.SetArticleTopAsync(articalId, isSetTop).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                AddModelError(result.ErrorMessage);
                return BadRequest(ModelState);
            }
            return this.Content(string.Empty);
        }
    }
}