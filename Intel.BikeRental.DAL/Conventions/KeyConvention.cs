namespace Intel.BikeRental.DAL.Conventions
{
    using System.Data.Entity.ModelConfiguration.Conventions;

    public class KeyConvention : Convention
    {
        public KeyConvention()
        {
            Properties().Where(c => c.Name.EndsWith("Key")).Configure(c => c.IsKey());
        }
    }
}
