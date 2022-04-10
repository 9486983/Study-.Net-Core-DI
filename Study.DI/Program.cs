using System;
using Microsoft.Extensions.DependencyInjection;
using Study.Core;
using Study.Services;

namespace Study.DI
{
    class Program
    {
        static void Main(string[] args)
        {
            #region AutoDI
            AutoDI.Register<HelloWorldService>(InjectionType.Transient);
            AutoDI.RegisterWithDll("Study.Services");

            DemoRun();

            DriverRun<Car>();
            DriverRun<Tank>();

            DriverRun();
            #endregion

            #region Publish Subscribe
            string subject = "Sys_Theme_Changed";

            MessageManager.Subscribe(subject, (value) =>
            {
                MyClass my = value as MyClass;
                for (int i = 0; i < my.Count; i++)
                {
                    Console.WriteLine($"Sys_Theme_{i + 1} {my.Color}");
                }
            });


            while (true)
            {
                Console.WriteLine("input count...");
                int.TryParse(Console.ReadLine(), out int count);
                MessageManager.Publish(subject, new MyClass() { Count = count, Color = $"color: {count}" });
            }
            #endregion

            Console.ReadKey();


        }


        /// <summary>
        /// author: zy
        /// datetime: 
        /// des: 
        /// </summary>
        public class MyClass
        {
            public int Count { get; set; }
            public string Color { get; set; }
        }

        #region AutoDI
        private static void DemoRun()
        {
            for (int i = 0; i < 10; i++)
            {
                var demo = AutoDI.Default.GetService<HelloService>();
                Console.WriteLine("==============================================");
                Console.WriteLine(demo.HelloWorld());
                Console.WriteLine(demo.HelloWorld2());
                var demo2 = AutoDI.Default.GetService<HelloService>($"name{i + 1}");
                Console.WriteLine("==============================================");
                Console.WriteLine(demo2.HelloWorld());
                Console.WriteLine(demo2.HelloWorld2());
            }
        }

        private static void DriverRun()
        {
            string no = "Heavy";
            for (int i = 0; i < 10; i++)
            {
                IVehicle tank = AutoDI.Default.GetService<Tank>(no);

                Driver driver = new Driver(tank);
                driver.vehicle.Run();
            }
        }

        private static void DriverRun<T>() where T : IVehicle
        {
            //IVehicle vehicle = AutoDI.Default.GetService<T>();
            for (int i = 0; i < 10; i++)
            {
                IVehicle vehicle = AutoDI.Default.GetService<T>();

                Driver driver = new Driver(vehicle);
                driver.vehicle.Run();
            }
        }
        #endregion
    }
}
