using Autofac;
using DryIoc;
using Ioc.Repository.Repositories.Models;
using IoC.Interfaces;
using IoC.Services;
using System;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace IoC.ConsoleTest
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }
        static void Main(string[] args)
        {
            // TestDryIoc();

            TestAutoFac();
            Console.ReadLine();
        }

        static void TestDryIoc()
        {
            var overallProcess = new Stopwatch();
            overallProcess.Restart();
            Console.WriteLine("################## DryIoc Ioc ###################");

            var container = new Container();
            container.Register<IHotelServices, HotelServices>();
            container.Register<IDataServices, DataServices>();
            //Singleton
            //container.Register<IHotelServices, HotelServices>(Reuse.Singleton);
            //container.Register<IDataServices, DataServices>(Reuse.Singleton);

            var hotelSvc = container.Resolve<IHotelServices>();
            foreach (var h in hotelSvc.GetHotelAll().Result)
                Console.WriteLine($"{h.HotelId} - {h.Name} Status : {h.IsActive}");

            Console.WriteLine("####################################################");
            overallProcess.Stop();
            Console.WriteLine($"Elapsed Time {overallProcess.Elapsed} ");
        }

        static void TestAutoFac()
        {
            var overallProcess = new Stopwatch();
            overallProcess.Restart();

            var container = ContainerAutofacRegister.Configure();
            Console.WriteLine("################## Auto face Ioc ###################");

            using (var scope = container.BeginLifetimeScope())
            {
                var hotelSvc = scope.Resolve<IHotelServices>();
                var hh = hotelSvc.GetHotelAll().Result;
                foreach (var h in hotelSvc.GetHotelAll().Result)
                    Console.WriteLine($"{h.HotelId} - {h.Name} Status : {h.IsActive}");
            }
            Console.WriteLine("####################################################");
            overallProcess.Stop();
            Console.WriteLine($"Elapsed Time {overallProcess.Elapsed} ");
        }

        static void InitialConfig()
        {
            // Get environment
            var environment = Environment.GetEnvironmentVariable("Environment");
            // Build config to get information in appsettings.json
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                                .AddJsonFile(path: $"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public static class ContainerAutofacRegister
        {
            public static Autofac.IContainer Configure()
            {
                InitialConfig();

                // Create your builder.
                var builder = new ContainerBuilder();

                var serviceCollection = new ServiceCollection()
                    .AddDbContext<EF_DEMOContext>(b => b.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

                builder.Populate(serviceCollection);
                // Usually you're only interested in exposing the type
                // via its interface:
                builder.RegisterType<HotelServices>().As<IHotelServices>();
                builder.RegisterType<DataServices>().As<IDataServices>();

                builder.RegisterType<EF_DEMOContext>().AsSelf().As<DbContext>().InstancePerLifetimeScope();

                // However, if you want BOTH services (not as common)
                // you can say so:
                // builder.RegisterType<DataServices>().AsSelf().As<IDataServices>();
                return builder.Build();
            }
        }
    }
}
