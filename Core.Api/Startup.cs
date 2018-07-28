using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Core.Api.DB;
using Core.Api.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace Core.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy => policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
            });

            services.AddMvc(opt =>
            {
                opt.Filters.Add(new ProducesAttribute("application/json"));
                opt.Filters.Add(new ConsumesAttribute("application/json"));
            });

            var sqlConnectionString = Configuration.GetConnectionString("CoreConnectionString");
            services.AddEntityFrameworkNpgsql();
            services.AddDbContext<CoreDbContext>(options => options.UseNpgsql(sqlConnectionString));

            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICsvService, CsvService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "REST Service using Asp.Net Core 2.0 & EF Core (postgresql)",
                    Contact = new Contact()
                    {
                        Name = "Akbar Shaikh",
                        Email = "aashaikh55@gmail.com",
                        Url = "https://github.com/AkbaraliShaikh/"
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("AllowAll");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(swagger =>
            {
                swagger.SwaggerEndpoint("/swagger/v1/swagger.json", "My App");
            });
        }
    }
}
