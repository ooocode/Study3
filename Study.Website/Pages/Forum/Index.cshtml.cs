using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Study.Services.ArticalService.Res;
using Study.Website.Pages;

namespace Study.WebApplication.Pages.Forum
{
    public class IndexModel : AppPageModel
    {
        [FromRoute(Name = "ClassificationId")]
        public long? ClassificationId { get; set; }

        /// <summary>
        /// 分类信息
        /// </summary>
        public ArticalClassificationDto ArticalClassificationInfo { get; set; }

        public int TakeCount = 5;

        public List<ArticalDto> mostVisitArtical { get; set; }


        public async Task OnGetAsync()
        {
            if (!ClassificationId.HasValue)
            {
                //mostVisitArtical = articalService.GetArticlesAsync()
                //.OrderByDescending(e => e.VisitCount)
                //.Take(5);
                mostVisitArtical = await ArticalService.GetArticlesAsync(func: a => { return a.OrderByDescending(e => e.VisitCount); }, take: 5).ConfigureAwait(false);
            }
            else
            {
                ArticalClassificationInfo = await ArticalService.GetArticalClassificationByIdAsync(ClassificationId.Value).ConfigureAwait(false);

                mostVisitArtical = await ArticalService.GetArticlesAsync(func: a =>
                {
                    return a.Where(e => e.ClassificationId == ClassificationId).OrderByDescending(e => e.VisitCount);
                }, take: 5).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// 获取文章
        /// </summary>
        /// <param name="skipCount"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetLoadArticalsAsync(int skipCount)
        {
            //using (MiniProfiler.Current.Step("使用从数据库中查询的数据，进行Http请求"))
            {
                //  using (MiniProfiler.Current.CustomTiming("HTTP", "GET "))
                {
                    //await Task.Delay(2000);
                    List<ArticalDto> articals = null;
                    if (!ClassificationId.HasValue)
                    {
                        //没有分类
                        articals = await ArticalService.GetArticlesAsync(arg =>
                        {
                            return arg.OrderByDescending(e => e.IsSetTop)
                              .ThenByDescending(e => e.SetTopDateTime)
                              .ThenByDescending(e => e.PublishTime);
                        }, skip: skipCount, take: TakeCount).ConfigureAwait(false);
                    }
                    else
                    {
                        //有分类
                        articals = await ArticalService.GetArticlesAsync(arg =>
                        {
                            return arg.Where(e => e.ClassificationId == ClassificationId)
                            .OrderByDescending(e => e.IsSetTop)
                            .ThenByDescending(e => e.SetTopDateTime)
                            .ThenByDescending(e => e.PublishTime);
                        }, skip: skipCount, take: TakeCount).ConfigureAwait(false);
                    }

                    return new PartialViewResult
                    {
                        ViewName = "_ArticalListPartial",
                        ViewData = new ViewDataDictionary<IList<ArticalDto>>(ViewData, articals)
                    };
                }
            }
        }
    }
}