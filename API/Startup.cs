using API.Authentication;
using API.Authentication.Interfaces;
using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repository;
using DataEntity;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Models;
using NeoSmart.Caching.Sqlite;
using Serilog;
using System.IO;
using System.Linq;
using System.Text;
using Utilities;
using Utilities.Interface;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSqliteCache(options => {
                options.CachePath = @"Cache\cache.db";
            });
            services.AddMvc(op =>
            {
                op.EnableEndpointRouting = false;
            }).ConfigureApiBehaviorOptions(op =>
            {
                op.InvalidModelStateResponseFactory = action =>
                {
                    return new BadRequestObjectResult(action.ModelState.Where(model => model.Value.Errors.Count > 0).Select(modelError => modelError.Value.Errors.FirstOrDefault().ErrorMessage));
                };
            });

            services.AddAutoMapper(typeof(Startup));
            services.AddDbContextPool<AppDBContext>(op => op.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, ApplicationRole>(op =>
            {
                op.User.RequireUniqueEmail = true;
                op.Password.RequireDigit = true;
                op.Password.RequireNonAlphanumeric = false;
                op.Password.RequireUppercase = true;
                op.Password.RequiredLength = 8;
                op.Tokens.AuthenticatorIssuer = Configuration.GetSection("token").GetSection("issuer").Value;
            }).AddUserValidator<ApplicationUserPhoneValidator>()
           .AddEntityFrameworkStores<AppDBContext>().AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               options.SaveToken = true;
               options.RequireHttpsMetadata = false;
               options.TokenValidationParameters = new TokenValidationParameters()
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidIssuer = Configuration.GetSection("token").GetSection("issuer").Value,
                   ValidAudience = Configuration.GetSection("token").GetSection("audience").Value,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("token").GetSection("key").Value))
               };
           });
            services.AddCors(x => x.AddPolicy("MyCors", op =>
            {
                op.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));
            services.Configure<EmailConfigurationModel>(Configuration.GetSection("EmailConfig"));
            services.AddScoped(typeof(IRepo<>), typeof(Repo<>));
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IUserManager, User>();
            services.AddScoped<IUserBL, UserBL>();
            services.AddScoped<IConfigBL, ConfigBL>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IImageUploadInFile, ImageUploadInFile>();
            services.AddScoped<IPropertyBL, PropertyBL>();
            services.AddScoped<IWorkOrderBL, WorkOrderBL>();
            services.AddScoped<INotifier, Notifications>();
            services.AddScoped<IRecurringWorkOrderJob,RecurringWorkOrderJob>();
            services.AddScoped<IRecurringBL, RecurringBL>();
            services.AddScoped<IHomeBL, HomeBL>();
            services.AddScoped<IInspect, Inspect>();
            services.AddScoped<IRecurringInspection, RecurringInspection>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseExceptionHandler(options =>
            {
                options.UseMiddleware<ExceptionHandlerMiddlerwarecs>();
            });
            
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "ImageFileStore")),
                RequestPath = "/ImageFileStore"
            });

            app.UseCors("MyCors");
            app.UseAuthentication();
            app.UseMiddleware<BlackListCheckMiddleware>();
            app.UseRouting();
            app.UseAuthorization();
            app.UseSerilogRequestLogging();
            app.UseHangfireServer();
            app.UseHangfireDashboard();
            
            
            app.UseMvc();
           
        }
    }

   
}