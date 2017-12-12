namespace Intel.BikeRental.DAL
{
    using Intel.BikeRental.DAL.Configurations;
    using Intel.BikeRental.DAL.Conventions;
    using Intel.BikeRental.Models;
    using Models.Vehicles;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    public class BikeRentalContext : DbContext
    {
        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<Station> Stations { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Rental> Rentals { get; set; }

        public BikeRentalContext() : base("BikeRentalConnection")
        {
            this.ObjectContext.ObjectMaterialized += ObjectContextObjectMaterialized;
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = true;
        }

        private void ObjectContextObjectMaterialized(object sender, ObjectMaterializedEventArgs eventArgs)
        {
            if (eventArgs.Entity.GetType() == typeof(User))
            {
                User user = eventArgs.Entity as User;
                user.Parameters = new Parameters { P1 = int.Parse(user.SerializedParameters.Split(';')[0]), P2 = int.Parse(user.SerializedParameters.Split(';')[1]) };
            }
        }

        public override int SaveChanges()
        {
            var users = this.ChangeTracker.Entries<User>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(e => e.Entity);

            foreach (var user in users)
            {
                // !ToString - mocking serialize.
                user.SerializedParameters = user.Parameters.ToString();   // MOCK serialization.
            }

            return base.SaveChanges();
        }

        // Workaround to access old context for old methods support.
        public ObjectContext ObjectContext
        {
            get
            {
                return ((IObjectContextAdapter)this).ObjectContext;
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Set joined key.
            // modelBuilder.Entity<User>()
            //     .HasKey(k => new { k.FirstName, k.LastName });

            // Set table key.
            // modelBuilder.Entity<User>()
            //     .HasKey(k => k.UserId);

            // Table name change.
            // modelBuilder.Entity<User>()
            //     .ToTable("Użytkownicy");

            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new BikeConfiguration());
            modelBuilder.Configurations.Add(new RentalConfiguration());
            modelBuilder.Configurations.Add(new StationConfiguration());

            modelBuilder.Conventions.Add(new DateTime2Convention());
            modelBuilder.Conventions.Add(new KeyConvention());

            // When class have ID but should be treated as complex type.
            // modelBuilder.ComplexType<Location>();

            // Ignore from using in database.
            // modelBuilder.Ignore<User>();


            ////
            //// Abstraction usage in database.
            ////

            // TPH - [default] - Table Per Hierarchy.
            // Default - do nothing

            // TPT - Table Per Type - One common table, in addition to table per type.
            // modelBuilder.Entity<Bike>().ToTable("Bikes");
            // modelBuilder.Entity<Scooter>().ToTable("Scooters");

            // TPC - Table Per Concrete type - each type has separate table.
            // modelBuilder.Entity<Bike>().Map(m => 
            //     {
            //         m.MapInheritedProperties();
            //         m.ToTable("Bikes");
            //     }
            // );
            // 
            // modelBuilder.Entity<Scooter>().Map(m =>
            //     {
            //         m.MapInheritedProperties();
            //         m.ToTable("Scooters");
            //     }
            // );

            // modelBuilder.Conventions.Remove();

            modelBuilder.HasDefaultSchema("rentals");

            base.OnModelCreating(modelBuilder);
        }
    }
}
