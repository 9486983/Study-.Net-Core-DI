using AutoDI.Attributes;
using AutoDI.Enums;
using System;

namespace AutoDI.Interfaces
{
    public interface IVehicle
    {
        string ID { get; set; }
        void Run();
    }

    public interface IAerocraft
    {
        void Fly();
    }



    /// <summary>
    /// author: zy
    /// datetime: 
    /// des: 
    /// </summary>
    [DI(typeof(IVehicle), InjectionType.Singleton)]
    public class Car : IVehicle
    {
        public Car()
        {
            this.ID = Guid.NewGuid().ToString();
        }
        public string ID { get; set; }

        public void Run()
        {
            Console.WriteLine($"{ID}{Environment.NewLine}Car is running...");
        }
    }


    [DI(typeof(IAerocraft), InjectionType.Transient)]
    public class Fighter : IAerocraft
    {
        [DI]
        public Fighter()
        {
            this.ID = Guid.NewGuid().ToString();
        }
        public string ID { get; set; }

        public void Fly()
        {
            Console.WriteLine($"{ID}{Environment.NewLine}Fighter is flying");
        }
    }

    public interface ICarrier
    {
        void Cruising();
    }

    /// <summary>
    /// author: zy
    /// datetime: 
    /// des: 
    /// </summary>
    [DI(typeof(ICarrier), InjectionType.Singleton)]
    public class Carrier : ICarrier
    {
        readonly IAerocraft aerocraft;
        public Carrier(IAerocraft aerocraft)
        {
            this.ID = Guid.NewGuid().ToString();
            this.aerocraft = aerocraft;

        }

        public string ID { get; set; }
        public void Cruising()
        {
            Console.WriteLine($"{ID}{Environment.NewLine}Carrier is Cruising...");
            this.aerocraft.Fly();
        }
    }


}
