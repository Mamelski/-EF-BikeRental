using Intel.BikeRental.DAL;
using Intel.BikeRental.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Intel.BikeRental.ConsoleClient
{
    public class Transaction
    {
        public void SimpleTransaction()
        {
            using (var context = new BikeRentalContext())
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    // Modify 1
                    var user = context.Users.Find(1);
                    user.PhoneNumber = DateTime.Now.ToString();

                    // Save 1
                    context.SaveChanges();

                    var bike = context.Vehicles.First(v => v.IsActive);
                    var station = context.Stations.Find(1);

                    var rental = new Rental
                    {
                        Cost = 10,
                        DateFrom = DateTime.Now,
                        StationFrom = station,
                        User = user,
                        Vehicle = bike,
                    };

                    // Modify
                    context.Rentals.Add(rental);

                    // Rollback mock
                    throw new Exception("Mock");

                    // Save 2
                    context.SaveChanges();
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw;
                }

                transaction.Commit();
            }
        }

        public void DistributedTransactionScope()
        {
            // System.Transactions
            using (var transaction = new TransactionScope())
            {
                var user = new User { FirstName = "Janusz", Discount = 40, PhoneNumber = "111-222-333", Parameters = new Parameters { P1 = 123, P2 = 345 } };

                using (var firstContext = new BikeRentalContext())
                {
                    firstContext.Users.Add(user);
                    firstContext.SaveChanges();
                }

                using (var secondContext = new BikeRentalContext())
                {
                    secondContext.Users.Add(user);
                    secondContext.SaveChanges();
                }

                // Set transaction flag ONLY. Changes are not commited.
                transaction.Complete();
            }
        }

        public void ConcurentTest(int userId)
        {
            using (var firstContext = new BikeRentalContext())
            using (var secondContext = new BikeRentalContext())
            {
                // First session.
                var user = firstContext.Users.Find(userId);
                user.PhoneNumber = "555-555-555";

                // Second session.
                var user2 = secondContext.Users.Find(userId);
                user2.PhoneNumber = "777-777-777";

                //---------------------------------------------
                // Setting concurency:
                //---------------------------------------------
                //
                // in UserConfiguration, on property:
                //
                // Property(p => p.PhoneNumber)
                //     .IsConcurrencyToken();
                //
                //---------------------------------------------
                //
                // or in model:
                //
                // [ConcurencyCheck]
                //
                //---------------------------------------------
                //
                // or on whole class (table):
                //
                // 1) Need to add new column in model:
                // public byte[] RowVersion { get; set; }
                //
                // 2) In UserConfiguration:
                // Property(p => p.RowVersion)
                //     .IsRowVersion();
                //
                //---------------------------------------------

                // Mixed context save order.
                secondContext.SaveChanges();
                Thread.Sleep(TimeSpan.FromSeconds(2)); // Coffee break

                try
                {
                    firstContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException concurrencyException)
                {
                    Console.WriteLine("Entry was changed :(");

                    Console.WriteLine("Would you like to reload/override changes? (r/o)");
                    var key = Console.ReadKey();

                    // Reload entries from DB.
                    foreach (var entry in concurrencyException.Entries)
                    {
                        switch (key.Key)
                        {
                            case ConsoleKey.R:
                                entry.Reload();
                                break;

                            case ConsoleKey.O:
                                entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                                firstContext.SaveChanges();
                                break;

                            default:
                                throw;
                        }

                    }
                }
            }
        }

        public void ConcurentRowVersion(int userId)
        {
            using (var firstContext = new BikeRentalContext())
            using (var secondContext = new BikeRentalContext())
            {
                // First session.
                var user = firstContext.Users.Find(userId);
                user.LastName = "Kowalski";

                // Second session.
                var user2 = secondContext.Users.Find(userId);
                user2.LastName = "Kowalssk234i";

                //---------------------------------------------
                // Setting concurency:
                //---------------------------------------------
                //
                // or on whole class (table):
                //
                // 1) Need to add new column in model:
                // public byte[] RowVersion { get; set; }
                //
                // 2) In UserConfiguration:
                // Property(p => p.RowVersion)
                //     .IsRowVersion();
                //
                //---------------------------------------------

                // Mixed context save order.
                secondContext.SaveChanges();
                Thread.Sleep(TimeSpan.FromSeconds(2)); // Coffee break

                try
                {
                    firstContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    Console.WriteLine("Entry was changed :(");
                }
            }
        }
    }
}
