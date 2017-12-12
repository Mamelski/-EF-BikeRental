namespace Intel.BikeRental.ConsoleClient
{
    using DAL;
    using DAL.Configurations;
    using DAL.Migrations;
    using Models;
    using Models.Vehicles;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    class Program
    {
        static void Main(string[] args)
        {
            // Database.SetInitializer(new MigrateDatabaseToLatestVersion<BikeRentalContext, Configuration>());

            // AddStations(10);

            // AddUser("Jacek", 20);
            // AddUser("Marek", 20);
            // AddUser("Pablo", 20);

            // AddScooter("Magenta", 50, 100);
            // AddScooter("Violet", 20, 50);
            // AddBike("Blue", BikeType.City);
            // AddScooter("Green", 125, 270);
            // AddBike("Red", BikeType.Mountain);
            // AddScooter("Magenta", 60, 105);
            // AddScooter("Magenta", 80, 110);
            // AddScooter("Black", 225, 470);
            // AddBike("Red", BikeType.City);
            // AddBike("Black", BikeType.Tandem);

            // AddRental();

            // AddUserStateTest();
            // 
            // ChangeBikeColor();
            // 
            // MockWebService();

            // GetVehiclesTest();

            // LINQ operations.
            // Linq linq = new Linq();
            // linq.Test();

            // SQL commands operations.
            // Sql sql = new Sql();
            // sql.CommandTest();
            // sql.GetObjectsTest();
            // sql.StorageProceduresTest();

            // UpdateParametersTest();

            // Transaction transaction = new Transaction();
            // transaction.SimpleTransaction();
            // transaction.DistributedTransactionScope();
            // transaction.ConcurentTest(19);
            // transaction.ConcurentRowVersion(28);

            // StorageProcedures storageProcs = new StorageProcedures();
            // storageProcs.StorageTest();

            // AsyncTests asyncTests = new AsyncTests();
            // asyncTests.Start();
            // asyncTests.CalculateTest();
            // asyncTests.CalculateAsyncTest();
            // asyncTests.CalculateAsyncTestTasks();
            // asyncTests.CalculateAsyncEntityTest();
            // Console.WriteLine("Main thread finished.");

            Lazy lazy = new Lazy();
            // lazy.EagerLoadingTest();
            lazy.LazyLoadingTest();

            // Optimalization:
            //
            //  EntityStateManager:
            //      AsNotTracking
            //
            //  DetectChanges (method DetectChanges - in this class):
            //      context.Configuration.AutoDetectChangesEnabled = false;     // This is global setting.
            //      context.ChangeTracker.DetectChanges();                      
            //
            //  EF PowerTools (modify views - in training script):
            //      "Generate Views" option - this generates views as files in project.
            //      When app starts it loads views.
            //
            //      WARNING: When DB changes - view must be regenerated.
            //
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        private static void DetectChanges()
        {
            using(var context = new BikeRentalContext())
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                var user = context.Users.First();
                user.FirstName = "Bart";

                context.ChangeTracker.DetectChanges();

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();
                }
            }
        }

        private static void UpdateParametersTest()
        {
            using (var context = new BikeRentalContext())
            {
                var user = context.Users.Find(25);
                user.Parameters = new Parameters { P1 = DateTime.Now.Minute, P2 = DateTime.Now.Millisecond };

                context.Entry(user).State = EntityState.Modified;       // Property is ignored - force modification.
                context.SaveChanges();
            }
        }

        private static void GetVehiclesTest()
        {
            using(var context = new BikeRentalContext())
            {
                var vehicles = context.Vehicles;

                foreach (var vehicle in vehicles)
                {
                    Console.WriteLine(vehicle.ToString());
                }
            }
        }

        private static void AddScooter(string color, byte maxSpeed, int capacity)
        {
            var scooter = new Scooter
            {
                Color = color,
                MaxSpeed = maxSpeed,
                EngineCapacity = capacity,
                IsActive = true,
                Number = $"S1001_{DateTime.Now.Millisecond}"
            };

            using (var dbContext = new BikeRentalContext())
            {
                dbContext.Database.Exists();

                var metadataNotFound = dbContext.Database.CompatibleWithModel(false);

                dbContext.Vehicles.Add(scooter);
                dbContext.SaveChanges();
            }
        }

        private static void MockWebService()
        {
            using (var dbContext = new BikeRentalContext())
            {
                try
                {
                    var bike = dbContext.Vehicles.AsNoTracking().First(b => b.VehicleId == 1);

                    // WebService send, response, deserialize.

                    var receivedBike = new Bike
                    {
                        VehicleId = 1,
                        IsActive = bike.IsActive,
                        Color = "Green",
                        Number = bike.Number,
                        Type = (bike as Bike).Type
                    };

                    // This will add new.
                    // dbContext.Bikes.Add(receivedBike);                          // State: Added.

                    dbContext.Entry(receivedBike);                              // State: Detached.
                    dbContext.Vehicles.Attach(receivedBike);                       // Attach to context (only when not tracking).
                    dbContext.Entry(receivedBike);                              // State: Unchanged.

                    // Whole object is updated.
                    // dbContext.Entry(receivedBike).State = EntityState.Modified;

                    // Only property is updated.
                    dbContext.Entry(receivedBike).Property(p => p.Color).IsModified = true;

                    dbContext.Entry(receivedBike);                              // State: Modified.
                    dbContext.SaveChanges();

                    //// Library for nested, duplicated objects when using i.e.: WebServices (attach nested objects).
                    //// GraphDiff
                    //// context.UpdateGraph(company, map => map.OwnedCollection(p => p.Contacts));
                }
                catch (Exception exception)
                {
                    throw;
                }
            }
        }

        private static void AddUserStateTest()
        {
            var user = new User
            {
                FirstName = "Marcin",
                LastName = $"N_{DateTime.Now.Hour}_{DateTime.Now.Minute}",
                Discount = 40,
                PhoneNumber = "123456789"
            };

            using (var dbContext = new BikeRentalContext())
            {
                try
                {
                    // Add or update - own:
                    // dbContext.Users.Any(u => u == user);     // only bool
                    //                                          // performance best: Find().
                    
                    dbContext.Entry(user);          // State: Detached.
                    dbContext.Users.Add(user);      // Add user to context.
                    dbContext.Entry(user);          // State: Added.
                    dbContext.SaveChanges();        // Save context changes.
                    dbContext.Entry(user);          // State: Unchanged.
                    dbContext.Users.Remove(user);   // Remove user.
                    dbContext.Entry(user);          // State: Deleted.
                    dbContext.SaveChanges();        // Save context changes.
                    dbContext.Entry(user);          // State: Detached.
                }
                catch (Exception exception)
                {
                    throw;
                }
            }
        }

        // Change states tracking example.
        private static void ChangeBikeColor()
        {
            using (var dbContext = new BikeRentalContext())
            {
                const string NewColor = "Red";

                var bike = dbContext.Vehicles.First(b => b.Color != NewColor);
                var user = dbContext.Users.Find(1);

                bike.Color = NewColor;
                bike.Number = "B1999";

                // Override tracked object state - can be changed to i.e. create copy (Added).
                dbContext.Entry(bike).State = EntityState.Unchanged;

                // All operating objects (entries).
                dbContext.ChangeTracker.Entries();

                // Stop tracking for performance upgrade - only for non-modify queries.
                dbContext.Vehicles.Where(b => b.Color.Contains("Red")).AsNoTracking();

                dbContext.SaveChanges();
            }
        }

        private static void AddBike(string color, BikeType type)
        {
            var bike = new Bike
            {
                Color = color,
                Type = type,
                IsActive = true,
                Number = $"B1001_{DateTime.Now.Hour}_{DateTime.Now.Minute}"
            };

            using (var dbContext = new BikeRentalContext())
            {
                dbContext.Database.Exists();

                var metadataNotFound = dbContext.Database.CompatibleWithModel(false);

                dbContext.Vehicles.Add(bike);
                dbContext.SaveChanges();
            }
        }

        private static void AddStations(int count)
        {
            var stations = new List<Station>();

            for (int i = 0; i < count; i++)
            {
                var station = new Station
                {
                    Name = $"Stacja_{i}",
                    Address = $"Adres_{i}_{DateTime.Now.Hour}_{DateTime.Now.Minute}",
                    Location = new Location { Latitude = DateTime.Now.Millisecond, Longitude = DateTime.Now.Millisecond },
                    Capacity = 10,
                    IsActive = true
                };

                stations.Add(station);
            }

            using (var dbContext = new BikeRentalContext())
            {
                dbContext.Database.Exists();
                dbContext.Stations.AddRange(stations);

                try
                {
                    dbContext.SaveChanges();
                }
                catch (Exception exception)
                {
                    throw;
                }
            }
        }

        private static void AddUser(string name, int discount)
        {
            var user = new User
            {
                FirstName = name,
                LastName = $"N_{DateTime.Now.Hour}_{DateTime.Now.Minute}",
                Discount = discount,
                PhoneNumber = "123123123",
                Parameters = new Parameters { P1 = DateTime.Now.Minute, P2 = DateTime.Now.Millisecond }
            };

            using (var dbContext = new BikeRentalContext())
            {
                try
                {
                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                }
                catch (Exception exception)
                {
                    throw;
                }
            }
        }

        private static void AddRental()
        {

            var userLukasz = new User
            {
                FirstName = "Lukasz",
                LastName = $"K_{DateTime.Now.Hour}_{DateTime.Now.Minute}",
                Discount = 60,
                PhoneNumber = "123123123"
            };

            using (var dbContext = new BikeRentalContext())
            {
                try
                {
                    dbContext.Users.Add(userLukasz);
                    dbContext.SaveChanges();
                }
                catch (Exception exception)
                {
                    throw;
                }
            }

            using (var dbContext = new BikeRentalContext())
            {
                var user = dbContext.Users.First(u => u.FirstName.Contains("Luk"));
                var bike = dbContext.Vehicles.Find(1);
                var station = dbContext.Stations.Find(1);

                var rental = new Rental
                {
                    Vehicle = bike,
                    User = user,
                    StationFrom = station,
                    DateFrom = DateTime.Now
                };

                try
                {
                    dbContext.Rentals.Add(rental);
                    dbContext.SaveChanges();
                }
                catch (Exception exception)
                {
                    throw;
                }
            }
        }
    }
}
