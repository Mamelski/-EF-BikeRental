using Intel.BikeRental.DAL;
using Intel.BikeRental.Models.Vehicles;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.BikeRental.ConsoleClient
{
    public class Sql
    {
        public void CommandTest()
        {
            using (var context = new BikeRentalContext())
            {

                Console.Write("Deactivate vehicles with color: ");
                var color = Console.ReadLine();

                // SQL Injection H4ck1n6
                // var color = "Green' OR 1=1 OR ''='''";
                // var color = "Green'; DROP TABLE Users;'";

                // UPDATE [rentals].[Vehicles] SET IsActive = 1 WHERE Color = 'Red'
                // context.Database.ExecuteSqlCommand($"UPDATE [rentals].[Vehicles] SET IsActive = 0 WHERE Color = '{color}'");

                // Using SQL Injection protection
                var command = "UPDATE [rentals].[Vehicles] SET IsActive = 0 WHERE Color = @Color";
                var colorParameter = new SqlParameter("@Color", color);
                context.Database.ExecuteSqlCommand(command, colorParameter);
            }
        }

        public void GetObjectsTest()
        {
            var sql = "SELECT * FROM [rentals].[Vehicles] WHERE Type=0 AND Color='Red'";
            Console.WriteLine(sql);
            Console.WriteLine();
            Console.WriteLine("Results:");

            using (var context = new BikeRentalContext())
            {
                var bikes = context.Database.SqlQuery<Bike>(sql);

                foreach (var bike in bikes)
                {
                    Console.WriteLine(bike);
                }
            }
        }

        public void StorageProceduresTest()
        {
            /* 

            CREATE PROCEDURE uspDeleteBike(@BikeId int)
            AS BEGIN
                DELETE FROM [rentals].[Vehicles] WHERE [VehicleId] = @BikeId
            END

            */

            Console.Write("Enter Bike ID to delete: ");
            var bikeId = Console.ReadLine();
            var bikeIdParameter = new SqlParameter("@BikeId", bikeId);

            // Output parameter.
            // var outputParameter = new SqlParameter
            // {
            //     Direction = System.Data.ParameterDirection.Output
            // };

            using(var context = new BikeRentalContext())
            {
                context.Database.ExecuteSqlCommand("uspDeleteBike @BikeId", bikeIdParameter);
            }
        }
    }
}
