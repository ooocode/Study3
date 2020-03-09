//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.StaticFiles;

//namespace Study.WebApp.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class FileController : ControllerBase
//    {
//        private static string saveDir = null;
//        public FileController(IWebHostEnvironment env)
//        {
//            if(saveDir == null)
//            {
//                saveDir = System.IO.Path.Combine(env.WebRootPath,"UploadFiles");
//                if (!System.IO.Directory.Exists(saveDir))
//                {
//                    System.IO.Directory.CreateDirectory(saveDir);
//                }
//            }
//        }



//        // POST: api/File
//        [HttpPost]
//        //[Consumes("image/bmp", "image/cis-cod", "image/gif", "image/ief", "image/jpeg", "image/jpeg", "", "",)]
//        public async Task<IActionResult> Post()
//        {
//            IFormFile file = HttpContext.Request.Form.Files[0];
//            if (file != null)
//            {
//                if (file.Length > 50 * 1024 * 1024)
//                {
//                    return BadRequest("文件大小超过50MB");
//                }

//                ///获取扩展名
//                var provider = new FileExtensionContentTypeProvider();
//                var extName = provider.Mappings.FirstOrDefault(e => e.Value == file.ContentType).Key;
//                string filename = DateTime.Now.Ticks.ToString() + "_" + Guid.NewGuid().ToString("N") + extName;

//                string savePath = System.IO.Path.Combine(saveDir, filename);
//                using (var fileStream = System.IO.File.Create(savePath))
//                {
//                    await file.OpenReadStream().CopyToAsync(fileStream);
//                }

//                if (System.IO.File.Exists(savePath))
//                {
//                    return Ok(new {link= $"/UploadFiles/{filename}" });
//                }
//            }
//            return BadRequest();
//            //return Ok();
//        }

//        // PUT: api/File/5
//        [HttpPut("{id}")]
//        public void Put(int id, [FromBody] string value)
//        {
//        }

//        // DELETE: api/ApiWithActions/5
//        [HttpDelete("{id}")]
//        public void Delete(int id)
//        {
//        }

//        //private readonly ;
//    }
//}


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Study.Website.Controllers;
using Utility;

namespace Study.WebApp.Controllers
{
    [IgnoreAntiforgeryToken]
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private static string saveDir = null;
        public FileController(IWebHostEnvironment env, IConfiguration configuration)
        {
            if (saveDir == null)
            {
                saveDir = configuration["FileSavePhysicPath"];
            }
        }

        /// <summary>
        /// 获取真实下载文件名
        /// 1224264840974045191_xx.x
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string getRealDownloadName(string name)
        {
            try
            {
                var remove = $"{name.Split('_').ElementAtOrDefault(0) ?? string.Empty}_";
                var realName = name.Replace(remove, string.Empty);
                return realName;
            }
            catch (Exception ex)
            {
                return name;
            }
        }


        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="name">文件全名称（必填）</param>
        /// <param name="downloadFileName">下载文件名 可选</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get([FromQuery]string name, [FromQuery]string downloadFileName)
        {
            if (string.IsNullOrEmpty(name))
            {
                return NotFound();
            }

            var fullName = System.IO.Path.Combine(saveDir, name);
            if (!System.IO.File.Exists(fullName))
            {
                return NotFound();
            }

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fullName, out string contentType))
            {
                contentType = "application/octet-stream";
            }

            var stream = System.IO.File.OpenRead(fullName);

            if (string.IsNullOrEmpty(downloadFileName))
            {
                downloadFileName = getRealDownloadName(name);
            }

            return File(stream, contentType, downloadFileName, true);
        }



        private string MakeUniqueFileName(IFormFile file)
        {
            long id = GuidEx.NewGuid();

            ///获取扩展名
            var provider = new FileExtensionContentTypeProvider();
            var extName = provider.Mappings.FirstOrDefault(e => e.Value == file.ContentType).Key;
            string filename = $"{id}_{System.IO.Path.GetFileName(file.FileName)}";

            return filename;
        }


        /// <summary>
        /// 上传文件 成功后返回路径 /api/File?name=
        /// </summary>
        /// <returns></returns>
        // POST: api/File
        [HttpPost]
        [ProducesResponseType(typeof(UpLoadFileInfo), StatusCodes.Status200OK)]
        //[Consumes("image/bmp", "image/cis-cod", "image/gif", "image/ief", "image/jpeg", "image/jpeg", "", "",)]
        public async Task<IActionResult> Post()
        {
            IFormFile file = HttpContext.Request.Form.Files[0];
            if (file != null)
            {
                if (file.Length > 50 * 1024 * 1024)
                {
                    return BadRequest("文件大小超过50MB");
                }

                string filename = MakeUniqueFileName(file);

                string savePath = System.IO.Path.Combine(saveDir, filename);
                using (var fileStream = System.IO.File.Create(savePath))
                {
                    await file.OpenReadStream().CopyToAsync(fileStream);
                }

                if (System.IO.File.Exists(savePath))
                {
                    return Ok(new UpLoadFileInfo
                    {
                        Link = $"/api/File?name={System.Net.WebUtility.UrlEncode(filename)}",
                        FileName = file.FileName,
                        Size = file.Length
                    });
                }
            }
            return BadRequest();
        }
    }
}
