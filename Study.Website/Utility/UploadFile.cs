using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Study.Website.Utility
{
    public class UploadFile
    {

        public static async Task<string> SavaFileAsync(HttpContext httpContext, IWebHostEnvironment env)
        {
            IFormFile file = httpContext.Request.Form.Files[0];
            if (file != null)
            {
                if (file.Length > 50 * 1024 * 1024)
                {
                    //errorMsg = "文件大小超过50MB";
                    return null;
                }

                ///获取扩展名
                var provider = new FileExtensionContentTypeProvider();
                var extName = provider.Mappings.FirstOrDefault(e => e.Value == file.ContentType).Key;
                string filename = DateTime.Now.Ticks.ToString() + "_" + Guid.NewGuid().ToString("N") + extName;

                var saveDir = System.IO.Path.Combine(env.WebRootPath, "UploadFiles");

                string savePath = System.IO.Path.Combine(saveDir, filename);
                using (var fileStream = System.IO.File.Create(savePath))
                {
                    await file.OpenReadStream().CopyToAsync(fileStream);
                }

                if (System.IO.File.Exists(savePath))
                {
                    return $"/UploadFiles/{filename}";
                    //return Ok(new { link = $"/UploadFiles/{filename}" });
                }
            }
            return null;
        }
    }
}
