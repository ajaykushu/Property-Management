﻿using Microsoft.AspNetCore.Builder;
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
using WebEssentials.AspNetCore.Pwa;

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
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddSession(opts =>
            {
                opts.IdleTimeout = TimeSpan.FromMinutes(5);
            });

            services.Configure<RouteConstModel>(Configuration.GetSection("ApiRoutes"));
            services.Configure<MenuMapperModel>(Configuration.GetSection("SubMenuMapper"));
            services.AddSession(s => s.IdleTimeout = TimeSpan.FromMinutes(30));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IHttpClientHelper, HttpHelper>();
            services.AddHttpClient<HttpHelper>();
            services.AddScoped<IExport<WorkOrderDetail>, Export<WorkOrderDetail>>();
            services.AddScoped<IExport<AllWOExport>, Export<AllWOExport>>();
            services.AddSingleton<ISessionStorage, SessionStorage>();
            services.AddServiceWorker(new PwaOptions
            {

                CacheId = "Worker 1.3",
                AllowHttp = true,
                RoutesToPreCache = "/Login/Index,/Home/Privacy",
                RegisterServiceWorker = true,
                RegisterWebmanifest = true,
                Strategy = ServiceWorkerStrategy.CacheFirstSafe,

            }) ;
            
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
            app.UseCookiePolicy();
            app.UseRouting();
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