namespace Neo.LocationSearch.Models
{
    public class GeoIndexesData
    {
        public double LongitudeInterval;
        public GeoPoint SouthWest;
        public int LatitudeIntervalCount { get; set; }

        public int LongitudeIntervalCount { get; set; }
        public double LatitudeInterval { get; set; }
    }
}