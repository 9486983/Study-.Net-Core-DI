using Study.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.Services
{
    /// <summary>
    /// author: zy
    /// datetime: 
    /// des: 
    /// </summary>
    [DI(InjectionType.Singleton)]
    public class VehicleService : IHelloService, IVehicle, IService
    {
        public string ID { get; set; }
        public VehicleService()
        {
            this.ID = Guid.NewGuid().ToString();
        }

        public string Hello(string name)
        {
            return name;
        }

        public string Hello(string firstName, string lastName)
        {
            return $"{firstName}{lastName}";
        }

        public void Run()
        {
            Console.WriteLine($"{ID}: How vehicle is running...");
        }
    }
    /// <summary>
    /// author: zy
    /// datetime: 
    /// des: 
    /// </summary>
    /// 
    [DI(InjectionType.Singleton)]
    public class Car : IVehicle, IService
    {
        public readonly VehicleService helloService;
        public Car(VehicleService helloService)
        {
            this.helloService = helloService;
            this.ID = Guid.NewGuid().ToString();
        }
        public string ID { get; set; }
        public virtual void Run()
        {
            Console.WriteLine($"{helloService.Hello(ID)}: Car is running...");
        }
    }

    /// <summary>
    /// author: zy
    /// datetime: 
    /// des: 
    /// </summary>
    [DI(InjectionType.Singleton)]
    public class Tank : Car
    {
        public readonly string vehicleNo;
        public Tank(VehicleService helloService) : base(helloService)
        {
        }
        public Tank(VehicleService helloService, string vehicleNo) : base(helloService)
        {
            this.vehicleNo = vehicleNo;
        }
        public override void Run()
        {
            Console.WriteLine($"{helloService.Hello(ID)}: {vehicleNo}Tank is running...");
        }
    }



}
