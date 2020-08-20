namespace Neo.LocationSearch.Models
{
    public class Suburb
    {
        public string Name { get; set; }
        public string Postcode { get; set; }
        public string State { get; set; }
        public string Region { get; set; }
        
        public override string ToString()
        {
            return string.Join('\t', State, Region, Name, Postcode);
        }
    }
}