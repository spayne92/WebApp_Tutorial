using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BaseFileServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /*---- Organizing Order of Middleware for Application ----*/

            // Detects "index.html" as a default file for landing page and serves it as root.
            app.UseDefaultFiles();

            // Static file serving, but only from wwwroot directory - which had to be created.
            app.UseStaticFiles();
        }
    }
}
