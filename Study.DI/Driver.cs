using Study.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.DI
{
    public class Driver
    {
        public readonly IVehicle vehicle;
        public Driver(IVehicle _vehicle)
        {
            this.vehicle = _vehicle;
        }
    }
}
