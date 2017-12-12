using Intel.BikeRental.DAL;
using System;
using System.Linq;
using System.Data.Entity;

namespace Intel.BikeRental.ConsoleClient
{
    public class Lazy
    {
        public void EagerLoadingTest()
        {
            using(var context = new BikeRentalContext())
            {
                var rentals = context.Rentals
                    .Include(r => r.User)            // single
                    // .Include("User.Parameters")      // deeper, not recommended
                    // .Include(r => r.User.Select)     // deeper when list
                    // .Include(r => r.User.Parameters) // deeper only for objects
                    .ToList();

                rentals.ForEach(r => Console.WriteLine($"{r.DateFrom} -> {r.User.FirstName} {r.User.LastName}")
                );
            }
        }

        public void LazyLoadingTest()
        {
            //
            // Model changes:
            //      public virtual User User { get; set; }
            // 
            // Context changes:
            //      ctor:
            //          this.Configuration.LazyLoadingEnabled = true;
            //          this.Configuration.ProxyCreationEnabled = true;
            //
            // WARNING: This method creates proxies - it is not always working with serializers.
            //
            using (var context = new BikeRentalContext())
            {
                var rentals = context.Rentals.ToList();
                rentals.ForEach(r => Console.WriteLine($"{r.DateFrom} -> {r.User.FirstName} {r.User.LastName}"));
            }
        }
    }
}
