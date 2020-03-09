using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Study.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Study.Website
{
    /// <summary>
    /// 广告服务
    /// </summary>
    public class AdvertisementService
    {
        private readonly CacheService cacheService;
        private readonly IWebHostEnvironment environment;
        private readonly string fileName= null;

        private const string keyAddOrUpdateAdvertisement = "keyAddOrUpdateAdvertisement";

        public AdvertisementService(CacheService cacheService,IWebHostEnvironment environment)
        {
            this.cacheService = cacheService;
            this.environment = environment;

            fileName = Path.Combine(environment.WebRootPath, "广告代码.html");
        }

        /// <summary>
        /// 获取广告内容 
        /// </summary>
        /// <returns>如果不存在返回null</returns>
        public async Task<string> GetAdvertisementAsync()
        {
            if (environment.IsDevelopment())
            {
                return string.Empty;
            }
            var text = await cacheService.GetAsync<string>(keyAddOrUpdateAdvertisement).ConfigureAwait(false);
            if (string.IsNullOrEmpty(text))
            {
                if (File.Exists(fileName))
                {
                    text = File.ReadAllText(fileName);
                }
            }
            return text;
        }

        /// <summary>
        /// 添加或者更新广告
        /// </summary>
        /// <param name="addvertisementText">广告内容</param>
        public async Task<bool> AddOrUpdateAdvertisementAsync(string addvertisementText)
        {
            if (!string.IsNullOrEmpty(addvertisementText))
            {
                File.WriteAllText(fileName, addvertisementText);

                await cacheService.SetAsync(keyAddOrUpdateAdvertisement, addvertisementText,TimeSpan.FromDays(15)).ConfigureAwait(false);
                return true;
            }
            return false;
        }
    }
}
