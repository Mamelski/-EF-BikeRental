    namespace Intel.BikeRental.DAL.Configurations
{
    using Intel.BikeRental.Models;
    using System.Data.Entity.ModelConfiguration;

    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            // ToTable("Użytkownicy");

            Property(p => p.FirstName)
                .HasMaxLength(50);

            Property(p => p.LastName)
                .HasMaxLength(50);

            Ignore(p => p.LoggedIn);

            Ignore(p => p.Parameters);

            Property(p => p.SerializedParameters)
                .HasColumnName("Parameters");

            Property(p => p.PhoneNumber)
                .IsConcurrencyToken();

            Property(p => p.RowVersion)
                .IsRowVersion();
        }
    }
}
