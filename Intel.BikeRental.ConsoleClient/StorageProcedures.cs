using Intel.BikeRental.DAL;
using Intel.BikeRental.Models;

namespace Intel.BikeRental.ConsoleClient
{
    public class StorageProcedures
    {
        public void StorageTest()
        {
            //
            // To enable automatic storage procedures there is a need to configure object:
            // MapToStoredProcedures();
            //
            using (var context = new BikeRentalContext())
            {
                var station = new Station()
                {
                    Address = "jakis adres",
                    Name = "stacjaX",
                    Location = new Location { Latitude = 1, Longitude = 1 }
                };

                context.Stations.Add(station);

                try
                {
                    context.SaveChanges();
                }
                catch (System.Exception exception)
                {
                    throw;
                }
            }
        }
    }
}
