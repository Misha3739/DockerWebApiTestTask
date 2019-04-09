using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebApi1
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var redirectSection = Configuration.GetSection("RedirectTo").Get<RedirectConfigSection>();

            app.MapWhen(
                context => context.Request.Path.Value.StartsWith("/api/") && !context.Request.IsHttps,
                builder => builder.RunProxy(new ProxyOptions
            {
                Scheme = "http",
                Host = redirectSection.Http.Host,
                Port = redirectSection.Http.Port,
            }));
            
            app.MapWhen(
                context => context.Request.Path.Value.StartsWith("/api/")&& context.Request.IsHttps,
                builder => builder.RunProxy(new ProxyOptions
                {
                    Scheme = "https",
                    Host = redirectSection.Https.Host,
                    Port = redirectSection.Https.Port,
                }));
        }
    }
}
