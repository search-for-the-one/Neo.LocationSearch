using Neo.LocationSearch.Models;

namespace Neo.LocationSearch.LocationDataProviders.Models
{
    internal class SuburbLocation
    {
        public GeoPoint GeoLocation { get; set; }
        public Suburb Suburb { get; set; }
    }
}