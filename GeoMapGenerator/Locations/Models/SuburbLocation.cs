using Neo.LocationSearch.Models;

namespace GeoMapGenerator.Locations.Models
{
    internal class SuburbLocation
    {
        public GeoPoint GeoLocation { get; set; }
        public Suburb Suburb { get; set; }
    }
}