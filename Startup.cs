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
using Microsoft.EntityFrameworkCore;
using NewProjectSimulation.Data;

namespace NewProjectSimulation
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NewProjectSimulation", Version = "v1" });
            });

            //services.AddDbContext<NewProjectSimulationContext>(options =>
            //        options.UseSqlServer(Configuration.GetConnectionString("NewProjectSimulationContext")));
            //keyvault
            services.AddDbContext<NewProjectSimulationContext>(options =>
            options.UseSqlServer(Configuration["NewProjectSimulationContext"]));
            //redis
            services.AddDistributedRedisCache(options =>
            {
                //keep redis connection string
                options.Configuration = "testredisananya.redis.cache.windows.net:6380,password=3by5xjQsfhVqBxmUnm7NYH9sNgCLlZTtpAzCaKKYsfA=,ssl=True,abortConnect=False";
                options.InstanceName = "master";
            });
            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NewProjectSimulation v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
