using System;
using Neo.LocationSearch.Models;

namespace Neo.LocationSearch.Indexes.Models
{
    internal class GeoIndexes
    {
        private readonly double latitudeInterval;
        private readonly double longitudeInterval;
        private readonly GeoPoint southWest;

        public GeoIndexes(GeoIndexesData data)
        {
            latitudeInterval = data.LatitudeInterval;
            longitudeInterval = data.LongitudeInterval;
            southWest = data.SouthWest;
            LatitudeIntervalCount = data.LatitudeIntervalCount;
            LongitudeIntervalCount = data.LongitudeIntervalCount;
        }

        public GeoIndexes(GeoPoint southWest, GeoPoint northEast, Distance resolution)
        {
            this.southWest = southWest;

            var latitudeDistance = Distance.FromMetres(Math.Max(southWest.GetDistance(new GeoPoint(northEast.Latitude, southWest.Longitude)).AsMetres,
                new GeoPoint(southWest.Latitude, northEast.Longitude).GetDistance(northEast).AsMetres));

            LatitudeIntervalCount = (int) Math.Ceiling(latitudeDistance / resolution) + 1;
            latitudeInterval = (northEast.Latitude - southWest.Latitude) / LatitudeIntervalCount;

            var longitudeDistance = Distance.FromMetres(Math.Max(southWest.GetDistance(new GeoPoint(southWest.Latitude, northEast.Longitude)).AsMetres,
                new GeoPoint(northEast.Latitude, southWest.Longitude).GetDistance(northEast).AsMetres));
            LongitudeIntervalCount = (int) Math.Ceiling(longitudeDistance / resolution) + 1;
            longitudeInterval = (northEast.Longitude - southWest.Longitude) / LongitudeIntervalCount;
        }

        public int LatitudeIntervalCount { get; }

        public int LongitudeIntervalCount { get; }

        public GeoPoint GetGeoPoint(GeoIndex geoIndex)
        {
            return new GeoPoint(southWest.Latitude + geoIndex.X * latitudeInterval, southWest.Longitude + geoIndex.Y * longitudeInterval);
        }

        public GeoIndex GetIndex(GeoPoint point)
        {
            return new GeoIndex((int) Math.Round((point.Latitude - southWest.Latitude) / latitudeInterval),
                (int) Math.Round((point.Longitude - southWest.Longitude) / longitudeInterval));
        }

        public bool IsIndexValid(GeoIndex geoIndex)
        {
            return geoIndex.X >= 0 && geoIndex.X < LatitudeIntervalCount && geoIndex.Y >= 0 && geoIndex.Y < LongitudeIntervalCount;
        }

        public GeoIndexesData Dump()
        {
            return new GeoIndexesData
            {
                LatitudeInterval = latitudeInterval,
                LongitudeInterval = longitudeInterval,
                LatitudeIntervalCount = LatitudeIntervalCount,
                LongitudeIntervalCount = LongitudeIntervalCount,
                SouthWest = southWest
            };
        }
    }
}