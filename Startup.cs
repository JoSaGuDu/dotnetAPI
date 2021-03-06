//Container of the app. Responsible of configuration and Dependency injection(Pattern) which uses an object (container) to initialize objects and provide required dependenciesis toh the objects. You register services in this container and the container is responsible of provid instances of these services to objects that demands them. Dependency Injection is a specialization of the Inversion of control which consist on delegation the the selection of a concrete implementation type for any dependency of a class.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace dotnet_asp_API
{
    public class Startup//Entry point fot the app
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container to be injected.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddLogging(loggingBuilder =>
            {
                //I need to log in the console.
                loggingBuilder.AddConsole();//Program.cs will take care of privide this service
                //I need to log in the debugger console.
                loggingBuilder.AddDebug();//Program.cs will take care of privide this service
            });

            //Add MVC mildware to handle HTTP requests
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline. how to respond to individual HTTP requests
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            //support status code display on http response otional
            app.UseStatusCodePages();

            app.UseMvc();//Now we can handle HTTP requests
        }
    }
}