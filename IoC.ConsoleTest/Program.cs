using Autofac;
using DryIoc;
using IoC.Interfaces;
using IoC.Services;
using System;
using System.Diagnostics;

namespace IoC.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TestDryIoc();

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
            foreach (var h in hotelSvc.GetHotelAll())
                Console.WriteLine($"{h.HotelId} - {h.HotelName} Status : {h.IsActive}");

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
                foreach (var h in hotelSvc.GetHotelAll())
                    Console.WriteLine($"{h.HotelId} - {h.HotelName} Status : {h.IsActive}");
            }
            Console.WriteLine("####################################################");
            overallProcess.Stop();
            Console.WriteLine($"Elapsed Time {overallProcess.Elapsed} ");
        }

        public static class ContainerAutofacRegister
        {
            public static Autofac.IContainer Configure()
            {
                // Create your builder.
                var builder = new ContainerBuilder();

                // Usually you're only interested in exposing the type
                // via its interface:
                builder.RegisterType<HotelServices>().As<IHotelServices>();
                builder.RegisterType<DataServices>().As<IDataServices>();

                // However, if you want BOTH services (not as common)
                // you can say so:
                // builder.RegisterType<DataServices>().AsSelf().As<IDataServices>();

                return builder.Build();
            }
        }
    }
}
