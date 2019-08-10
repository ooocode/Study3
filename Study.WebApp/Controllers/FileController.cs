using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Study.WebApp.Controllers
{
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        private readonly IHostingEnvironment _env;

        public FileController(IHostingEnvironment env)
        {
            _env = env;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Json(new
            {
                Id = "001",
                Name = "张三"
            });
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            try
            {
                var file = HttpContext.Request.Form.Files[0];
                if (file != null)
                {
                    
                    string filename = DateTime.Now.Ticks.ToString() + "_" + Guid.NewGuid().ToString("N") + "_" + System.IO.Path.GetExtension(file.FileName);

                    string saveDir = System.IO.Path.Combine(_env.WebRootPath, "UploadFiles");
                    if (!System.IO.Directory.Exists(saveDir))
                    {
                        System.IO.Directory.CreateDirectory(saveDir);
                    }

                    string savePath = System.IO.Path.Combine(saveDir, filename);
                    using (var fileStream = System.IO.File.Create(savePath))
                    {
                        await file.OpenReadStream().CopyToAsync(fileStream);
                    }

                    if (System.IO.File.Exists(savePath))
                    {
                        return Json(new { link = $"https://{HttpContext.Request.Host.Value}/UploadFiles/{filename}" });
                    }
                }
            }
            catch (Exception ex)
            {
                ///return Json(ex);
            }
            return BadRequest();
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
