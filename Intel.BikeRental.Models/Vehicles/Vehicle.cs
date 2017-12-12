using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.BikeRental.Models.Vehicles
{
    public abstract class Vehicle : Base
    {
        public int VehicleId { get; set; }

        // [MaxLenght(50)]
        public string Color { get; set; }

        // [Required]
        public string Number { get; set; }

        public bool IsActive { get; set; }

        public override string ToString()
        {
            return $"[{this.VehicleId}] [{this.GetType().Name}] : {this.Number} | {this.Color}";
        }
    }
}
