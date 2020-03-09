using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Study.Services;
using Study.Services.ArticalService;
using Study.Services.ArticalService.Req;
using Study.Services.ArticalService.Res;

namespace Study.Website.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticalService articalService;

        public ArticlesController(IArticalService articalService)
        {
            this.articalService = articalService;
        }


        /// <summary>
        /// 获取文章分类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ArticalClassificationDto>> GetArticalClassifications()
        {
            return await articalService.GetArticalClassificationsAsync();
        }

        /// <summary>
        /// 添加文章分类
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> AddArticalClassificationAsync(AddArticalClassificationDto dto)
        {
            return await articalService.AddArticalClassificationAsync(dto);
        }

        /// <summary>
        /// 获取文章
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ArticalDto>> GetArticlesAsync(int skip, int count)
        {
            return await articalService.GetArticlesAsync(skip:skip,take:count).ConfigureAwait(false);
        }

        /// <summary>
        /// 通过id获取文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ArticalDto> GetArticleById(long id)
        {
            return await articalService.GetArticalByIdAsync(id);
        }
    }
}
