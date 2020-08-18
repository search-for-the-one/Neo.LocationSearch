using System;
using System.Collections.Generic;

namespace Neo.LocationSearch.Models
{
    internal struct GeoArea
    {
        public GeoArea(GeoPoint southWest, GeoPoint northEast)
        {
            SouthWest = southWest;
            NorthEast = northEast;
        }

        public GeoPoint SouthWest { get; set; }
        public GeoPoint NorthEast { get; set; }

        public static GeoArea FromGeoPoints(IEnumerable<GeoPoint> points)
        {
            var minLatitude = double.MaxValue;
            var minLongitude = double.MaxValue;
            var maxLatitude = double.MinValue;
            var maxLongitude = double.MinValue;
            foreach (var point in points)
            {
                minLatitude = Math.Min(minLatitude, point.Latitude);
                minLongitude = Math.Min(minLongitude, point.Longitude);
                maxLatitude = Math.Max(maxLatitude, point.Latitude);
                maxLongitude = Math.Max(maxLongitude, point.Longitude);
            }

            return new GeoArea(new GeoPoint(minLatitude, minLongitude), new GeoPoint(maxLatitude, maxLongitude));
        }

        public bool OverlapWith(GeoArea other)
        {
            return Math.Max(SouthWest.Latitude, other.SouthWest.Latitude) <= Math.Min(NorthEast.Latitude, other.NorthEast.Latitude) &&
                   Math.Max(SouthWest.Longitude, other.SouthWest.Longitude) <= Math.Min(NorthEast.Longitude, other.NorthEast.Longitude);
        }
    }
}