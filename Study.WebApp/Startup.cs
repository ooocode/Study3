using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Study.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Study.Services.ArticalService;
using System.Security.Claims;
using Study.WebApp.Data;
using IdentityServer4.Models;

namespace Study.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(e =>
            {
                e.LoginPath = "/Account/Login";
                e.LogoutPath = "/Account/Logout";
                e.AccessDeniedPath = "/Denied";
                e.SlidingExpiration = true;
            });


            services.AddHttpsRedirection(e => { e.HttpsPort = 443; e.RedirectStatusCode = 307; });

            services.AddDbContext<AppDatabaseContext>(options =>
                options.UseSqlite("Data Source=database.db", b => b.MigrationsAssembly("Study.WebApp")));



            services.AddDbContext<UserDbContext>(options =>
               options.UseSqlite("Data Source=user.db", b => b.MigrationsAssembly("Study.WebApp")));

            services.AddIdentity<IdentityUserEx, IdentityRole>()
                .AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(e =>
            {
                e.Password.RequireUppercase = false;
                e.Password.RequireNonAlphanumeric = false;
                e.Password.RequireLowercase = false;
                e.Password.RequireDigit = false;

                e.User.RequireUniqueEmail = true;
            });

            services.AddMemoryCache();


            services.AddScoped<IArticalService, ArticalService>();



            services.AddMvc().AddRazorPagesOptions(e=> { }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();


            app.UseAuthentication();

            //app.Use(async (context, next) =>
            //{
            //    if (context.User.Identity.IsAuthenticated)
            //    {
            //        //RoleHelper result = context.RequestServices.GetService<RoleHelper>();
            //        var sid = context.User.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Sid);
            //        if (sid != null && !string.IsNullOrEmpty(sid.Value))
            //        {
            //            context.Request.Headers.Add(ClaimTypes.Sid, sid.Value);
            //        }

            //        var role = context.User.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Role);
            //        if (role != null && !string.IsNullOrEmpty(role.Value))
            //        {
            //            context.Request.Headers.Add(ClaimTypes.Role, role.Value);
            //        }
            //    }

            //    await next.Invoke();
            //});


            app.UseStatusCodePagesWithRedirects("~/StatusCode/{0}");

            app.UseMvc();
        }
    }
}
