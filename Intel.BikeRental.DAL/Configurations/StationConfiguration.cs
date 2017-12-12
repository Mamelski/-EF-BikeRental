using Intel.BikeRental.Models;
using System.Data.Entity.ModelConfiguration;

namespace Intel.BikeRental.DAL.Configurations
{
    public class StationConfiguration : EntityTypeConfiguration<Station>
    {
        public StationConfiguration()
        {
            Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(10);

            // Automatically generate and use storage procedures.
            MapToStoredProcedures();

            // Change name of storage procedure.
            // MapToStoredProcedures(s => s.Update(u => u.HasName("sp_modify_station")));
        }
    }
}
