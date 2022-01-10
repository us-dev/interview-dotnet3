using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using AutoMapper;
using GroceryStore.Services;
using GroceryStoreAPI.Bootstrap;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace GroceryStoreAPI
{
    /// <summary>
    /// Startup class that creates the DI Container and configures the app.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Default constructor for <see cref="Startup"/>
        /// </summary>
        /// <param name="configuration">An instance of <see cref="IConfiguration"/></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// An implementation of Configuration object.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        ///  This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">An instance of <see cref="IServiceCollection"/></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();

            services.AddOptions();
            services.AddControllers();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "GroceryStore API",
                    Version = "V1",
                    Description = "Provides api endpoints for GroceryStore Service"
                });

                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            var autoMapperProfiles = new List<Profile>()
            {
                new ControllerAutoMapperProfile()
            };

            services.AddAutoMapper(config =>
            {
                config.AddProfiles(autoMapperProfiles);
            });

            services.AddServicesToServiceCollection(this.Configuration);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">An instance of <see cref="IApplicationBuilder"/></param>
        /// <param name="env">An instance of <see cref="IWebHostEnvironment"/></param>
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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
        }
    }
}
