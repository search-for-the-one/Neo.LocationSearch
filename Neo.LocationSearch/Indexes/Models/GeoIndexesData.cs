using Neo.LocationSearch.Models;

namespace Neo.LocationSearch.Indexes.Models
{
    internal class GeoIndexesData
    {
        public double LongitudeInterval;
        public GeoPoint SouthWest;
        public int LatitudeIntervalCount { get; set; }

        public int LongitudeIntervalCount { get; set; }
        public double LatitudeInterval { get; set; }
    }
}