using UNLIMITED_EVENT_API.Extensions;
using AutoMapper;
using UNLIMITED_EVENT_API;
using UNLIMITED_EVENT_API.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Entities.Helpers;
using DinkToPdf.Contracts;
using DinkToPdf;

namespace UNLIMITED_EVENT_API
{
    public class Startup
    {
        public Startup(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            LogManager.LoadConfiguration(Path.Combine(webHostEnvironment.WebRootPath, "logfiles", "nlog.config"));
            Configuration = configuration;

            new CustomAssemblyLoadContext().LoadUnmanagedLibrary(Path.Combine(webHostEnvironment.WebRootPath, "lib", "dink-to-pdf", "64 bit", "libwkhtmltox.dll"));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.ConfigureLoggerService();
            services.ConfigureRepositoryContext(Configuration);
            services.AddHttpContextAccessor();
            services.ConfigureRepositoryWrapper();
            services.ConfigureJWTAuthenticationService(Configuration);
            services.ConfigureClaimPolicy();
            services.ConfigureNewtonsoftJson();
            services.ConfigureMailService(Configuration);

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddControllers();
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UNLIMITED EVENT API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", OpenApiOperationFilter.SecuritySchema);

                c.OperationFilter<OpenApiOperationFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UNLIMITED_EVENT_API v1"));
            }
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UNLIMITED_EVENT_API v1"));

            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //Global execption handling
            app.ConfigureCustomExceptionMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
