using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Study.Database;
using Study.Database.VideoDb;
using Study.Service;

using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Logging;
using LogDashboard;
using Microsoft.Extensions.Hosting;
using Grpc.Net.Client;
using Grpc.Core;
using Microsoft.AspNetCore.Http.Features;
using tusdotnet;
using tusdotnet.Models;
using tusdotnet.Stores;
using tusdotnet.Models.Configuration;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using System;

namespace Study.Website
{
    public class Startup
    {
        private IWebHostEnvironment env;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            this.env = environment;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

     

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            TimeSpan userPermissionCacheTimeSpan = TimeSpan.Zero;
            if (env.IsDevelopment())
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                userPermissionCacheTimeSpan = TimeSpan.FromMinutes(2);
                //services.AddDistributedMemoryCache();
                services.AddDistributedRedisCache(e => e.Configuration = "127.0.0.1:6379");
            }

            services.AddIdentityServerCenterConnect(new IdentityServerCenterConnect.ConfigurationOptions
            {
                AuthorityUrl = Configuration["Authority"],
                ClientId = Configuration["ClientId"],
                ClientSecret = "",
                GRpcUrl = Configuration["RpcUserClientUri"],
                Scopes = new System.Collections.Generic.List<string> { "openid", "profile" }
            }, userPermissionCacheTimeSpan);


            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(@"C:\temp-keys\"));

            services.AddDbContextPool<UserDatabaseContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("StudyUserDatabaseContext"),ctx=>ctx.EnableRetryOnFailure());
            });


            services.AddDbContextPool<VideoDbContext>(options =>
               options.UseMySql(Configuration.GetConnectionString("StudyVideo")));

            services.AddMemoryCache();

            services.AddHttpContextAccessor();

            services.AddSingleton<CacheService>();
            var config11 = new ConfigCustomService(services);

            services.AddSingleton<AdvertisementService>();

            //services.AddOData();
            services.AddCors();

            services.AddMiniProfiler().AddEntityFramework();

            services.AddMvc().AddRazorPagesOptions(options =>
            {
                //options.Conventions.AddPageRoute("/Movie/Index", "");
                options.Conventions.AddPageRoute("/Forum/Index", "");
            }).AddRazorRuntimeCompilation();


            services.AddHealthChecks();

            services.AddControllersWithViews();
            services.AddLogDashboard(option =>
            {
                option.RootPath = "C:/StudyLog";
            });


            //上传的文件保存路径
            var fileSavePhysicPath = Configuration["FileSavePhysicPath"];
            if (!System.IO.Directory.Exists(fileSavePhysicPath))
            {
                System.IO.Directory.CreateDirectory(fileSavePhysicPath);
            }

            fileSavePhysicPath = Configuration["TusFileSavePhysicPath"];
            if (!System.IO.Directory.Exists(fileSavePhysicPath))
            {
                System.IO.Directory.CreateDirectory(fileSavePhysicPath);
            }


            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = long.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });

            services.AddProgressiveWebApp();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            using(var scope = app.ApplicationServices.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<UserDatabaseContext>().Database.EnsureCreated();
                scope.ServiceProvider.GetRequiredService<VideoDbContext>().Database.EnsureCreated();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMiniProfiler();
                //使用数据库异常页
                // app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //https重定向
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(policy =>
            {
                policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            //必须加上UseCookiePolicy  否则手机qq浏览器访问不了cookie
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseAuthorization();


            //app.Use((context, next) =>
            //{
            //    var ip = context.Connection.RemoteIpAddress.ToString();
            //    var url = context.Request.Path;

            //    logger.LogDebug($"访问记录： {ip} {url}");
            //    return next.Invoke();
            //});




            app.UseTus(httpContext => new DefaultTusConfiguration
            {
                Store = new TusDiskStore(Configuration["TusFileSavePhysicPath"]),
                // On what url should we listen for uploads?
                UrlPath = "/TusFiles",
                //允许上传大小
                MaxAllowedUploadSizeInBytes = int.Parse(Configuration["TusFileSizeOfM"]) * 1024 * 1024,
                MaxAllowedUploadSizeInBytesLong = long.Parse(Configuration["TusFileSizeOfM"]) * 1024 * 1024,
                Events = new Events
                {
                    OnAuthorizeAsync = async eventContext =>
                    {
                        bool? isAuthenticated = eventContext.HttpContext?.User?.Identity?.IsAuthenticated;
                        if (!isAuthenticated.HasValue || isAuthenticated.Value == false)
                        {
                            eventContext.FailRequest(System.Net.HttpStatusCode.Unauthorized);
                        }
                    }
                },
            });


            app.UseEndpoints(configure =>
            {
                configure.MapControllers();
                configure.MapRazorPages();
            });

            app.UseLogDashboard();
        }
    }
}