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
using DryIoc.MefAttributedModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ioc.Repository.Repositories.Models;

namespace IoC.Web
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
            services.AddDbContext<EF_DEMOContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            // services.AddScoped<Ioc.Repository.Repositories.Models.EF_DEMOContext>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // Autofac Register
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<HotelServices>().As<IHotelServices>();
            builder.RegisterType<DataServices>().As<IDataServices>();
        }


        //public IServiceProvider ConfigureServices(IServiceCollection services)
        //{
        //    services.Configure<CookiePolicyOptions>(options =>
        //    {
        //        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        //        options.CheckConsentNeeded = context => true;
        //        options.MinimumSameSitePolicy = SameSiteMode.None;
        //    });

        //    services.AddMvc().AddControllersAsServices();

        //    // DryIOC            
        //    // var container = new Container().WithDependencyInjectionAdapter(services);
        //    // container.Register<IHotelServices, HotelServices>();
        //    // container.Register<IDataServices, DataServices>();
        //    // return container.Resolve<IServiceProvider>();

        //    return new Container()
        //      // optional: to support MEF attributed services discovery
        //      .WithMef()
        //      // setup DI adapter
        //      .WithDependencyInjectionAdapter(services,
        //          // optional: propagate exception if specified types are not resolved, and prevent fallback to default Asp resolution
        //          throwIfUnresolved: type => type.Name.EndsWith("Controller", StringComparison.CurrentCulture))
        //      // add registrations from CompositionRoot classs
        //      .ConfigureServiceProvider<CompositionRoot>();
        //}

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

    public class CompositionRoot
    {
        // If you need the whole container then change parameter type from IRegistrator to IContainer
        public CompositionRoot(IRegistrator r)
        {
            r.Register<IHotelServices, HotelServices>(Reuse.Singleton);
            r.Register<IDataServices, DataServices>(Reuse.Transient);
            // r.Register<IScopedService, ScopedService>(Reuse.InCurrentScope);

            var assemblies = new[] { typeof(DataServices).GetAssembly() };
            r.RegisterExports(assemblies);
        }
    }
}
