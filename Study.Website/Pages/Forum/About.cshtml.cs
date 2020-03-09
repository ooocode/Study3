using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Study.WebApp.Data;

namespace Study.Website.Pages.Forum
{
    public class AboutModel : PageModel
    {
        private readonly IMemoryCache cache;

        public AboutModel(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public class HistoryItem
        {
            public string Timestamp { get; set; }
            public string Content { get; set; }
        }

        /// <summary>
        /// 获取发布历史
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetItemsAsync()
        {
            var items = await Task.FromResult(new List<HistoryItem>
            {
                 new HistoryItem{ Timestamp = "2020/3/6", Content = "PWA应用支持"},
                 new HistoryItem{ Timestamp = "2020/2/19", Content = "加入云盘"},
                 new HistoryItem{ Timestamp = "2019/12/31", Content = "累计优化更新"},
                 new HistoryItem{ Timestamp = "2019/12/9", Content = "累计优化更新，更换OIDC登录方式"},
                 new HistoryItem{ Timestamp = "2019/11/12", Content = "更新电影页面UI"},
                 new HistoryItem{ Timestamp = "2019/11/5", Content = "安全性增强，限制访问频率，1秒5次，1分钟30次"},
                 new HistoryItem{ Timestamp = "2019/10/23", Content = "调整视频播放页面"},
                 new HistoryItem{ Timestamp = "2019/10/10", Content = "调整UI，提升浏览体验,视频Id加密保护"},
                 new HistoryItem{ Timestamp = "2019/10/4", Content = "添加视频分类，后续继续完善中..."},
                 new HistoryItem{ Timestamp = "2019/9/22", Content = "数据库迁移到mysql"},
                 new HistoryItem{ Timestamp = "2019/5/1", Content = "初始化项目"},
            }).ConfigureAwait(false);

            return new JsonResult(items);
        }
    }
}