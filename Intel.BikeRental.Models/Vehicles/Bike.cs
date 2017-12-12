using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.BikeRental.Models.Vehicles
{
    public class Bike : Vehicle
    {
        public BikeType Type { get; set; }
    }
}
