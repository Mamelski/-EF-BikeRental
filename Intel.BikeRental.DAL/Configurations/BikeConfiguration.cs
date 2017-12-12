namespace Intel.BikeRental.DAL.Configurations
{
    using Intel.BikeRental.Models;
    using Models.Vehicles;
    using System.Data.Entity.ModelConfiguration;

    public class BikeConfiguration : EntityTypeConfiguration<Vehicle>
    {
        public BikeConfiguration()
        {
            Property(p => p.Color)
                .HasMaxLength(20)
                .IsUnicode(false);

            Property(p => p.Number)
                .IsRequired()
                .HasMaxLength(60)
                .IsUnicode(false);
        }
    }
}
