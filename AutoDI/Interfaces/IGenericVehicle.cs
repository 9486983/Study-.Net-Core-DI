using AutoDI.Attributes;
using AutoDI.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDI.Interfaces
{
    public interface IGenericVehicle<T1, T2>
    {

    }


    /// <summary>
    /// author: zy
    /// datetime: 
    /// des: 
    /// </summary>
    /// 
    [DI(typeof(IGenericVehicle<,>), InjectionType.Singleton)]
    public class GenericVehicle<T1, T2> : IGenericVehicle<T1, T2>
    {
        public string ID { get; set; }
        [DI]
        public GenericVehicle()
        {
            this.ID = Guid.NewGuid().ToString();
            Console.WriteLine($"{ID}{this.GetType().Name}{Environment.NewLine}T1: {typeof(T1).Name}   T2: {typeof(T2).Name}");
        }
    }
}
