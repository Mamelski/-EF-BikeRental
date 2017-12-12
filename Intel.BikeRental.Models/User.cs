namespace Intel.BikeRental.Models
{
    public class User : Base
    {
        public int UserId { get; set; }

        public int UserKey { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public int Discount { get; set; }

        public bool LoggedIn { get; set; }

        //// Getter - serialize to string property.
        ////
        //// Set in configuration:
        //// Ignore(p => p.Parameters);                 // ignore original property
        ////
        ////    Property(p => p.SerializedParameters)
        ////        .HasColumnName("Parameters");       // rename string property
        ////
        ////
        ////    We can override SaveChanges() method in context class to
        ////    serialize objects.
        ////
        ////
        ////
        ////
        ////
        ////
        ////
        ////
        ////
        ////
        ////
        public Parameters Parameters { get; set; }

        public string SerializedParameters { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
