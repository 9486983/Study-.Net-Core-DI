using AutoDI.DI;
using AutoDI.Interfaces;
using System;
using System.Collections.Generic;

namespace AutoDI
{
    class Program
    {
        static void Main(string[] args)
        {
            var root = new ServicesContainer().RegisterAssembly();

            for (int i = 0; i < 10; i++)
            {
                var vehicle = root.GetService<ICarrier>();
                vehicle.Cruising();
            }

            var generic = root.GetService<IGenericVehicle<string, int>>();
            var genericList = root.GetService<IGenericVehicle<string, List<string>>>();
            var genericDictionary = root.GetService<IGenericVehicle<Dictionary<string, List<int>>, int>>();


            Console.ReadKey();

        }
    }
}
