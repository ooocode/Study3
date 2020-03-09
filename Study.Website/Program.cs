using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Study.Website
{
    public class Program
    {
        public static void CreateSerilog()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
            .WriteTo.File($"C:/StudyLog/.log",
            rollingInterval: RollingInterval.Day,
            outputTemplate: "{Timestamp:HH:mm} || {Level} || {SourceContext:l} || {Message} || {Exception} ||end {NewLine}")

         //.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
            .CreateLogger();
        }

        public static void Main(string[] args)
        {
#if DEBUG
            //Host.CreateDefaultBuilder().ConfigureWebHostDefaults(webBuilder =>
            //{
            //    webBuilder.UseHttpSys(options =>
            //    {
            //        options.AllowSynchronousIO = false;
            //        options.Authentication.Schemes = Microsoft.AspNetCore.Server.HttpSys.AuthenticationSchemes.None;
            //        options.Authentication.AllowAnonymous = true;
            //        options.MaxConnections = null;
            //        options.MaxRequestBodySize = 30000000;
            //        options.UrlPrefixes.Add("http://localhost:6000/qq");
            //        options.UrlPrefixes.Add("https://localhost:6001/qq");
            //    });

            //    webBuilder.UseStartup<Startup>();
            //}).Build().Run();


            CreateHostBuilderDebug(args).Build().Run();
#else
            CreateSerilog();
            CreateHostBuilder(args).Build().Run();
#endif
        }


        internal static IHostBuilder CreateHostBuilderDebug(string[] args) =>
                    Host.CreateDefaultBuilder(args)
                        .ConfigureWebHostDefaults(webBuilder =>
                        {
                            webBuilder.UseKestrel(opt =>
                            {
                                opt.ListenAnyIP(6001,ee=> { ee.UseHttps(); });
                                opt.Limits.MaxRequestBodySize = long.MaxValue;
                            });
                            webBuilder.UseStartup<Startup>();
                        });


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSerilog();
                    webBuilder.UseKestrel(e =>
                    {
                        e.ListenAnyIP(80, ee => ee.Protocols = HttpProtocols.Http1AndHttp2);
                        e.ListenAnyIP(443, ee =>
                        {
                            ee.Protocols = HttpProtocols.Http1AndHttp2;
                            ee.UseHttps("SHA256withRSA_zwovo.xyz.pfx", "651398");
                            // , ee =>
                            //{
                            //    ee.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                            //});
                        });
                    });


                    webBuilder.UseStartup<Startup>();
                });
    }
}
