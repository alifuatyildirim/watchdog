using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WatchDog;
using WatchDog.src.Enums;

namespace WatchDogLogging
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

            services.AddControllers();
            services.AddWatchDogServices(opt =>
            {
                opt.ClearTimeSchedule = WatchDogAutoClearScheduleEnum.Monthly;
                opt.IsAutoClear = true;
                opt.SqlDriverOption = WatchDogSqlDriverEnum.MSSQL;
                opt.SetExternalDbConnString = "Server=localhost\\MSSQLSERVER01;Database=master;Trusted_Connection=True;";
            });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WatchDogLogging", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WatchDogLogging v1"));
            }
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseWatchDogExceptionLogger();
            app.UseWatchDog(opt =>
            {
                opt.WatchPageUsername = "ali";
                opt.WatchPagePassword = "ali";
            });
            

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
