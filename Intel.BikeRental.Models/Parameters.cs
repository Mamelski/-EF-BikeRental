namespace Intel.BikeRental.Models
{
    public class Parameters
    {
        public int P1 { get; set; }

        public int P2 { get; set; }

        public override string ToString()
        {
            return $"{this.P1};{this.P2}";
        }
    }
}
