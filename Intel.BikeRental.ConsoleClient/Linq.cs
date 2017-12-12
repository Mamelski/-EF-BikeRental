using Intel.BikeRental.DAL;
using Intel.BikeRental.Models.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.BikeRental.ConsoleClient
{
    public class Linq
    {
        public void Test()
        {
            // LinqToSQL        - not supported
            // LinqToEntity
            // LinqToObjects
            // LinqToXML

            // .Where - no query to database. Only on first usage (i.e.: ToList()).
            // .Where - returns expression object.

            // Except
            // Intersect

            using (var dbContext = new BikeRentalContext())
            {
                // Multiple sorting.
                var vehicles = dbContext.Vehicles.OrderBy(v => v.Color).ThenBy(v => v.Number);

                // Select only colors and ID's
                var colorIds = dbContext.Vehicles.Select(v => new { v.VehicleId, v.Color });

                // Create copy of vehicle.
                var newVehicle = dbContext.Vehicles.Select(v => new Bike { VehicleId = v.VehicleId });

                // Named properties.
                var namedVehicle = dbContext.Vehicles
                    .Select(v => new { Ajdi = v.VehicleId, Kolor = v.Color })
                    .Where(v => v.Ajdi == 1);

                // GroupBy - groupped objects with count.
                var grouppedByColor = dbContext.Vehicles
                    .GroupBy(v => v.Color)
                    .Select(g => new { Color = g.Key, Quantity = g.Count() } );

                // GroupBy two variables.
                var twoGroups = dbContext.Vehicles
                    .GroupBy(v => new { v.Color, v.IsActive })
                    .Select(g => new { Active = g.Key.IsActive, Quantity = g.Count(), Color = g.Key.Color })
                    .ToList();

                // Tables join
                // var rentals = dbContext.Rentals.Include(r => r.User).Include(r => r.Vehicle);

                // Tables diff.
                var vehRed = dbContext.Vehicles.Where(v => v.Color == "Red");
                var vehNotActive = dbContext.Vehicles.Where(v => !v.IsActive);
                var diff = vehRed.Except(vehNotActive);

                // Table union.
                var union = vehRed.Union(vehNotActive);

                // All active vehicles.
                var allActive = dbContext.Vehicles.All(v => v.IsActive);

                // Other syntax.
                var query = (from vehicle in dbContext.Vehicles
                            where vehicle.Color == "Red"
                            orderby vehicle.Number
                            select vehicle)
                            .Where(v => v.VehicleId == 1);

                dbContext.SaveChanges();
            }
        }
    }
}
