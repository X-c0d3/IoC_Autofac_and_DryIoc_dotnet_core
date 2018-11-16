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
using Swashbuckle.AspNetCore.Swagger;

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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "API (.net core)",
                    Description = "ASP.NET Core Web API",
                });
            });
            services.AddMiniProfiler(options =>
            {
                // Path to use for profiler URLs, default is /mini-profiler-resources
                options.RouteBasePath = "/profiler";

                // Control storage - the default is 30 minutes
                //(options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);
                //options.Storage = new SqlServerStorage("Data Source=.;Initial Catalog=MiniProfiler;Integrated Security=True;");

                // Control which SQL formatter to use, InlineFormatter is the default
                options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.SqlServerFormatter();
            }).AddEntityFramework();
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
                app.UseMiniProfiler();
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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Acid.Api.Swagger.index.html");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
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
