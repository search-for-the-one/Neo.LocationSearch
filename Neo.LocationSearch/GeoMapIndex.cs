using System;
using System.Collections.Generic;
using System.Linq;
using Neo.LocationSearch.Models;

namespace Neo.LocationSearch
{
    public class GeoMapIndex : IGeoMapIndex
    {
        private readonly GeoIndexes indexes;
        private readonly Distance resolution;
        private readonly List<Suburb>[][] suburbs;
        private readonly NearestEnumerator enumerator;

        public GeoMapIndex(GeoMapData data)
        {
            indexes = new GeoIndexes(data.GeoIndexes);
            resolution = Distance.FromMetres(data.ResolutionInMetres);
            enumerator = new NearestEnumerator(indexes, resolution);

            suburbs = new List<Suburb>[indexes.LatitudeIntervalCount][];
            for (var i = 0; i < indexes.LatitudeIntervalCount; i++)
            {
                suburbs[i] = new List<Suburb>[indexes.LongitudeIntervalCount];
            }

            PopulateSuburbs(data);
        }
        
        public GeoMapIndex(GeoPoint southWest, GeoPoint northEast, Distance resolution)
        {
            this.resolution = resolution;
            indexes = new GeoIndexes(southWest, northEast, resolution);
            enumerator = new NearestEnumerator(indexes, resolution);

            suburbs = new List<Suburb>[indexes.LatitudeIntervalCount][];
            for (var i = 0; i < indexes.LatitudeIntervalCount; i++)
            {
                suburbs[i] = new List<Suburb>[indexes.LongitudeIntervalCount];
            }
        }
        
        public IEnumerable<Suburb> GetNearestSuburbs(GeoPoint point, Distance distance)
        {
            var results = enumerator.Enumerate(point, distance);
            return results.SelectMany(x => suburbs[x.X][x.Y] ?? Enumerable.Empty<Suburb>()).Distinct();
        }

        public List<Suburb> this[GeoPoint point]
        {
            get
            {
                var index = indexes.GetIndex(point);
                if (!indexes.IsIndexValid(index))
                    throw new IndexOutOfRangeException(point.ToString());
                return suburbs[index.X][index.Y];
            }
            set
            {
                var index = indexes.GetIndex(point);
                if (!indexes.IsIndexValid(index))
                    throw new IndexOutOfRangeException(point.ToString());
                suburbs[index.X][index.Y] = value?.ToList();
            }
        }

        public GeoMapData Dump() => new GeoMapDataDumper(indexes, resolution, suburbs).Dump();
        public GeoIndex GeoIndex(GeoPoint point) => indexes.GetIndex(point);

        public List<Suburb> this[GeoIndex index]
        {
            get => suburbs[index.X][index.Y];
            set => suburbs[index.X][index.Y] = value;
        }

        private void PopulateSuburbs(GeoMapData data)
        {
            foreach (var suburbGeoIndexes in data.Suburbs)
            {
                foreach (var range in suburbGeoIndexes.IndexRanges)
                {
                    var x = range[0];
                    var l = range[1];
                    var r = range[2];
                    for (var y = l; y <= r; y++)
                    {
                        ref var suburbList = ref suburbs[x][y];
                        suburbList ??= new List<Suburb>(1);
                        suburbList.Add(suburbGeoIndexes.Suburb);
                    }
                }
            }
        }
    }
}