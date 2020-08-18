using Neo.LocationSearch.Models;

namespace Neo.LocationSearch.Locations.Models
{
    internal class SuburbLocation
    {
        public GeoPoint GeoLocation { get; set; }
        public Suburb Suburb { get; set; }

        public override string ToString()
        {
            return $"{Suburb}{Common.Separator}{GeoLocation}";
        }
    }
}