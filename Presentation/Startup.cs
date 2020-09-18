using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation.ConstModal;
using Presentation.Utiliity;
using Presentation.Utiliity.Interface;
using Presentation.Utility;
using Presentation.Utility.Interface;
using Presentation.ViewModels;
using System;
using NeoSmart.Caching.Sqlite;

namespace Presentation
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
            services.AddSqliteCache(options => {
                options.CachePath = @"Utility\CacheSession\session.db";
            });
            services.AddDetection();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(op=> {
                op.LoginPath = "/";
                op.LogoutPath = "/Logout";
                op.ExpireTimeSpan = TimeSpan.FromDays(30);
            });

            services.Configure<RouteConstModel>(Configuration.GetSection("ApiRoutes"));
            services.Configure<MenuMapperModel>(Configuration.GetSection("SubMenuMapper"));
            services.AddSession(s => s.IdleTimeout = TimeSpan.FromMinutes(30));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IHttpClientHelper, HttpHelper>();
            services.AddHttpClient<HttpHelper>();
            services.AddScoped<IExport<WorkOrderDetail>, Export<WorkOrderDetail>>();
            services.AddScoped<IExport<AllWOExport>, Export<AllWOExport>>();
            services.AddScoped<IExport<AllWOExportRecurring>, Export<AllWOExportRecurring>>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseSession();
            //app.UseHttpsRedirection();
            var cookiePolicyOptions = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                CheckConsentNeeded = context => false,
            };
            app.UseCookiePolicy(cookiePolicyOptions);
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}