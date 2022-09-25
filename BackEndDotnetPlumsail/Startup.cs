using BackEndDotnetPlumsail.Data.Common;
using BackEndDotnetPlumsail.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndDotnetPlumsail
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile($"appsettings.{env.EnvironmentName.Trim()}.json")
                .AddConfiguration(configuration);

            Configuration = configurationBuilder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        readonly string MyCorsPolicy = "CorsPolicy";
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DietDbContext>(options => options.UseSqlServer(connection));

            services.AddCors(o => o.AddPolicy(MyCorsPolicy, builder => {
                builder
                .SetIsOriginAllowed(_ => true)
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()                
                ;
            }));
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                ); ;
            services.AddMvc(o => {
                o.Filters.Add(new ResponseCacheAttribute
                {
                    NoStore = true,
                    Location = ResponseCacheLocation.None
                });
            });

            services.AddScoped<IRationRepository, RationRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(MyCorsPolicy);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
