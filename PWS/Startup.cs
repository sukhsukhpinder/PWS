using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PingOne.Core.Configuration;
using PingOne.Core.Configuration.Extensions;
using PWS.DAL.Models;

namespace PWS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddScoped<PWSContext>();
            services.AddAuthorization();
            services.AddCors(options =>
            {
                options.AddPolicy(name: "CorsPolicy",
                            builder =>
                            {
                                builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                            });
            });

            services.AddDbContext<PWSContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:MSSQL"]));

            services.AddPingOneAuthentication("PingOne", Configuration.GetSection("PingOne:Authentication")
                .Get<PingOneConfigurationAuthentication>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{state?}");
            });
        }
    }
}
