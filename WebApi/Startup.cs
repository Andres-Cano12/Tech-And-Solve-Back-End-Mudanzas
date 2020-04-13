using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AccessData.Context;
using Microsoft.EntityFrameworkCore;
using BusinnesLogic.Services;
using AccessData.Repository;
using AutoMapper;
using App.Config.Dependencies;
using App.Common.Classes.Base.Repositories;
using AccessData.Entities;
using System.Runtime.Caching.Hosting;
using App.Common.Classes.Cache;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using App.Common.Classes.Validator;
using Microsoft.Extensions.DependencyModel;
using AccessData.Repository.IMoveDetailRepository;
using FluentValidation;
using Common.Classes.BussinesLogic;

namespace WebApi
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
            services.AddDbContext<DBContext>(options =>
                                       options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMemoryCache();
            
            var configMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            var mapper = configMapper.CreateMapper();

            services.AddSingleton(mapper);

            // RollBar
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #region Repository
            services.AddScoped<IBaseCRUDRepository<Move>, MoveRepository>();
            services.AddScoped<IBaseCRUDRepository<MoveDetail>, MoveDetailRepository>();
            #endregion

            #region Interface
            services.AddScoped<IMoveRepository, MoveRepository>();
            services.AddScoped<IMoveDetailRepository, MoveDetailRepository>();
            #endregion

            #region Services
            services.AddScoped<IMoveService, MoveService>();
            services.AddScoped<IMoveDetailService, MoveDetailService>();
            #endregion

            services.AddScoped<IServiceValidator<MoveDetail>, MoveDetailResourceValidator>();
            #region Others
            services.AddSingleton<App.Common.Classes.Cache.IMemoryCacheManager, MemoryCacheManager>();
            #endregion
            services.AddScoped<IValidator<MoveDTO>, ValidatorMoveService>();


            services.Configure<RequestLocalizationOptions>(opts =>
            {
                string english = "en-US";
                string spanish = "es";
                string spanishColombia = "es-CO";

                var supportedCultures = new List<CultureInfo> {
                    new CultureInfo(english),
                    new CultureInfo(spanish),
                    new CultureInfo(spanishColombia)
                };
                opts.DefaultRequestCulture = new RequestCulture(culture: english, uiCulture: english);
                opts.SupportedCultures = supportedCultures;
                opts.SupportedUICultures = supportedCultures;

            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:4200")
                    );
            });

             services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseStaticFiles();
        }
    }
}
