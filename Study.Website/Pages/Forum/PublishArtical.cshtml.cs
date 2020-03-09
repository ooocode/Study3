using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Study.Services;
using Study.Services.ArticalService;
using Study.Services.ArticalService.Req;
using Study.Services.ArticalService.Res;
using Study.Services.UserService;
using Study.WebApp.Data;
using Study.Website.Pages;
using Utility;

namespace Study.WebApp.Pages.Forum
{
    [Authorize]
    public class PublishArticalModel : AppPageModel
    {
        /// <summary>
        /// 文章id, Id不为空时，修改文章
        /// </summary>

        [FromRoute(Name = "Id")]
        public long? Id { get; set; }

        /// <summary>
        /// 文章分类
        /// </summary>
        public List<ArticalClassificationDto> ArticalClassifications { get; set; }


        public class AddOrUpdateArticalModel
        {
            /// <summary>
            ///分类Id
            /// </summary>
            [Required]
            public long ClassificationId { get; set; }



            /// <summary>
            /// 文章标题
            /// </summary>
            [Required(ErrorMessage = "文章标题不能空")]
            [StringLength(50,MinimumLength = 3,ErrorMessage = "标题长度是3~50个字符")]
            public string Title { get; set; }


            /// <summary>
            /// 文章内容
            /// </summary>
            [Required]
            public string Content { get; set; }
        }

        public AddOrUpdateArticalModel AddOrUpdateArtical { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            //修改
            if (Id.HasValue)
            {
                var artical = await ArticalService.GetArticalByIdAsync(Id.Value).ConfigureAwait(false);
                //更新文章
                if (artical == null)
                {
                    return NotFound();
                }

                //禁止修改别人的文章
                if (artical.UserId != CurUserId)
                {
                    return Forbid();
                }

                AddOrUpdateArtical = new AddOrUpdateArticalModel
                {
                    ClassificationId = artical.ClassificationId,
                    Content = artical.Content,
                    Title = artical.Title
                };
            }

            //有发布系统公告权限？
            if ((await AuthorizationService.AuthorizeAsync(User, Permissons.Permisson.Article_System)
                .ConfigureAwait(false)).Succeeded)
            {
                ArticalClassifications = await ArticalService.GetArticalClassificationsAsync().ConfigureAwait(false);
            }
            else
            {
                ArticalClassifications = (await ArticalService.GetArticalClassificationsAsync()).Where(e => e.Name != "系统公告").ToList();
            }

            return Page();
        }


        /// <summary>
        /// 移除所有匹配到的xpath对于的节点
        /// </summary>
        /// <param name="htmlDocument"></param>
        /// <param name="xpaths"></param>
        /// <returns></returns>
        private HtmlDocument removeNodes(HtmlDocument htmlDocument,string[] xpaths)
        {
            foreach(var xpath in xpaths)
            {
                var iframes = htmlDocument.DocumentNode.SelectNodes(xpath);
                foreach (var iframe in iframes)
                {
                    iframe.Remove();
                }
            }
            return htmlDocument;
        }


        //添加/更新文章
        public async Task<IActionResult> OnPostAsync(AddOrUpdateArticalModel AddOrUpdateArtical)
        {
            do
            {
                //bool isRunning = cac.Get<bool>($"OnPostAddArticalAsync");
                //if (isRunning)
                //{
                //    ModelState.AddModelError("", "两次发帖时间间隔最短为30秒");
                //    break;
                //}

                //cache.Set($"OnPostAddArticalAsync", true, TimeSpan.FromSeconds(30));

                if (!TryValidateModel(AddOrUpdateArtical))
                {
                    break;
                }

                //给图片添加响应式和水平居中
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(AddOrUpdateArtical.Content);


                //htmlDoc = removeNodes(htmlDoc,new string[] { "//iframe", "//ins" });

                var imgs = htmlDoc.DocumentNode.SelectNodes("//img");
                if (imgs != null)
                {
                    foreach (var img in imgs)
                    {
                     
                        var src = img.Attributes.FirstOrDefault(e=>e.Name == "src");

                        //移除所有属性
                        img.Attributes.RemoveAll();


                        img.AddClass("img-fluid d-block mx-auto");
                        img.SetAttributeValue("style", "width:90%");
                        img.SetAttributeValue("src", src.Value);
                    }

                    AddOrUpdateArtical.Content = htmlDoc.DocumentNode.OuterHtml;
                }


                if (!Id.HasValue)
                {
                    Result<long> respone = await ArticalService.AddArticalAsync(new AddArticalDto
                    {
                        UserId = CurUserId,
                        ClassificationId = AddOrUpdateArtical.ClassificationId,
                        Content = AddOrUpdateArtical.Content,
                        Title = AddOrUpdateArtical.Title.Trim()
                    }).ConfigureAwait(false);


                    if (!respone.Succeeded)
                    {
                        ModelState.AddModelError("", respone.ErrorMessage);
                        break;
                    }
                    return Content($"/Forum/ArticalDetail/{respone.Data}");
                }
                else
                {
                    var respone = await ArticalService.UpdateArticalAsync(Id.Value, new UpdateArticalDto
                    {
                        ArticalId = Id.Value,
                        Content = AddOrUpdateArtical.Content,
                        ClassificationId = AddOrUpdateArtical.ClassificationId,
                        Title = AddOrUpdateArtical.Title.Trim()
                    }).ConfigureAwait(false);

                    if (!respone.Succeeded)
                    {
                        ModelState.AddModelError("", respone.ErrorMessage);
                        break;
                    }
                    return Content($"/Forum/ArticalDetail/{Id}");
                }
            } while (false);

            return BadRequest(ModelState);
        }
    }
}