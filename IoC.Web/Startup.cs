using System;
using System.Collections.Generic;
using IoC.Interfaces;
using IoC.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Autofac.Extensions.DependencyInjection;
using Autofac;

using DryIoc.Microsoft.DependencyInjection;
using DryIoc;

namespace IoC.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //public void ConfigureServices(IServiceCollection services)
        //{

        //    services.Configure<CookiePolicyOptions>(options =>
        //    {
        //        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        //        options.CheckConsentNeeded = context => true;
        //        options.MinimumSameSitePolicy = SameSiteMode.None;
        //    });


        //    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        //}

        // Autofac Register
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<HotelServices>().As<IHotelServices>();
            builder.RegisterType<DataServices>().As<IDataServices>();
        }


        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().AddControllersAsServices();

            //using DryIoc.Microsoft.DependencyInjection;
            //using DryIoc;

            // DryIOC            
            var container = new Container().WithDependencyInjectionAdapter(services);
            container.Register<IHotelServices, HotelServices>();
            container.Register<IDataServices, DataServices>();
            return container.Resolve<IServiceProvider>();
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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
