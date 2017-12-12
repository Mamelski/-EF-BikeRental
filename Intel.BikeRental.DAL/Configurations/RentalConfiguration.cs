namespace Intel.BikeRental.DAL.Configurations
{
    using Intel.BikeRental.Models;
    using System.Data.Entity.ModelConfiguration;

    public class RentalConfiguration : EntityTypeConfiguration<Rental>
    {
        private const string DateTime2 = "datetime2";

        public RentalConfiguration()
        {
            // Property(p => p.DateFrom).HasColumnType(DateTime2);
            // Property(p => p.DateTo).HasColumnType(DateTime2);
        }
    }
}
