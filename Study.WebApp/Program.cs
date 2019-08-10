using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Study.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //MovieParser parser = new MovieParser();
            //parser.Start().Wait();

            CreateWebHostBuilder(args).Build().Run();
        }

#if DEBUG
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
          WebHost.CreateDefaultBuilder(args)
              .UseStartup<Startup>();
#else
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
          WebHost.CreateDefaultBuilder(args)
         
                .UseKestrel(options =>
                    {
                        options.ListenAnyIP(80);

                        //options.Limits.MaxRequestBodySize = 209715200;
                        //加入https证书
                        options.ListenAnyIP(443, e =>
                        {
                           
                          //e.Protocols = HttpProtocols.Http1AndHttp2;
                            e.UseHttps("SHA256withRSA_zwovo.xyz.pfx", "651398",ee=> { ee.SslProtocols = System.Security.Authentication.SslProtocols.Tls12; });
                        });
                    })
                .UseStartup<Startup>();
#endif

    }
}
