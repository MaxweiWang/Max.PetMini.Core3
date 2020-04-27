using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Max.PetMini.WebAPI.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using NAutowired;
using NAutowired.Core.Extensions;
using Newtonsoft.Json.Serialization;
using Snowflake.Core;

namespace Max.PetMini.WebAPI
{
    /// <summary>
    /// ����
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()//ϵͳĬ�ϵ�
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver() { NamingStrategy = new DefaultNamingStrategy() };
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
                }).AddControllersAsServices();

            services.Replace(ServiceDescriptor.Transient<IControllerActivator, NAutowiredControllerActivator>());
            services.AutoRegisterDependency(new List<string> { "Max.PetMini.WebAPI", "Max.PetMini.DAL", "Max.PetMini.BLL" });

            //���SwaggerUI
            services.AddSwaggerGen(options =>
            {
                //TODO Configure Enum to String
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Max.PetMini.WebAPI API", Version = "v1" });
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var webAppXmlPath = Path.Combine(basePath, "Max.PetMini.WebAPI.xml");
                var bllXmlPath = Path.Combine(basePath, "Max.PetMini.BLL.xml");
                var dalXmlPath = Path.Combine(basePath, "Max.PetMini.DAL.xml");
                var extensionsXmlPath = Path.Combine(basePath, "Max.PetMini.Extension.xml");
                options.IncludeXmlComments(webAppXmlPath);
                options.IncludeXmlComments(bllXmlPath);
                options.IncludeXmlComments(dalXmlPath);
                options.IncludeXmlComments(extensionsXmlPath);
            });
            services.AddSwaggerGenNewtonsoftSupport();
            ConfigureConfigurations(services);
        }

        /// <summary>git 
        /// �����ļ���ȡ
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureConfigurations(IServiceCollection services)
        {
            //���ݿ��������ö�ȡ
            services.Configure<DAL.DbConnectionConfig>(Configuration.GetSection("ConnectionStrings"));
            services.AddSingleton(item => new IdWorker(1, Configuration.GetValue<int>("ServerNodeNo")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //ʹ���м�������쳣��Ϣ
            app.UseMiddleware<ExceptionHandler>();

            app.UseRouting();

            //��̬�ļ�
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthorization();

            //ʹ��swagger
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();
                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Max.PetMini.WebAPI v1");
                });
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
