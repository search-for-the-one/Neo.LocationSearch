﻿using System;
using System.Collections.Generic;
using System.Linq;
using Neo.LocationSearch.Models;
#if DEBUG
using System.IO;
#endif

namespace Neo.LocationSearch
{
    public class GeoMapIndex : IGeoMapIndex
    {
        private readonly GeoIndexes indexes;
        private readonly Distance resolution;
        private readonly List<Suburb>[][] suburbs;

        public GeoMapIndex(GeoMapData data)
        {
            indexes = new GeoIndexes(data.GeoIndexes);
            resolution = Distance.FromMetres(data.ResolutionInMetres);

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

            suburbs = new List<Suburb>[indexes.LatitudeIntervalCount][];
            for (var i = 0; i < indexes.LatitudeIntervalCount; i++)
            {
                suburbs[i] = new List<Suburb>[indexes.LongitudeIntervalCount];
            }
        }

        public IEnumerable<Suburb> NearbySuburbs(GeoPoint point, Distance distance)
        {
            var nearByIndexes = NearbyIndexes(point, distance);
            return nearByIndexes.SelectMany(x => suburbs[x.X][x.Y] ?? Enumerable.Empty<Suburb>()).Distinct();
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

        public GeoMapData Dump()
        {
#if DEBUG
            var uncoveredSuburbs = new Dictionary<Suburb, GeoPoint>();
#endif
            var dumpSuburbs = new Dictionary<Suburb, List<GeoIndex>>();
            for (var i = 0; i < indexes.LatitudeIntervalCount; i++)
            {
                for (var j = 0; j < indexes.LongitudeIntervalCount; j++)
                {
                    if (suburbs[i][j] == null)
                        continue;

                    foreach (var suburb in suburbs[i][j])
                    {
                        if (string.IsNullOrEmpty(suburb.Postcode))
                        {
#if DEBUG
                            uncoveredSuburbs[suburb] = indexes.GetGeoPoint(new GeoIndex(i, j));
#endif
                            continue;
                        }

                        if (!dumpSuburbs.ContainsKey(suburb))
                            dumpSuburbs[suburb] = new List<GeoIndex>();
                        dumpSuburbs[suburb].Add(new GeoIndex(i, j));
                    }
                }
            }
            
#if DEBUG
            using var sw = new StreamWriter("uncovered-suburbs.txt");
            foreach (var (key, value) in uncoveredSuburbs)
            {
                sw.WriteLine($"{key}\t{value}");
            }
#endif

            return new GeoMapData
            {
                GeoIndexes = indexes.Dump(),
                ResolutionInMetres = resolution.AsMetres,
                Suburbs = dumpSuburbs.Select(kv => new SuburbGeoIndexes
                {
                    Suburb = kv.Key,
                    IndexRanges = ToRanges(kv.Value).Select(x => x.ToArray()).ToList()
                }).ToList()
            };
        }

        public GeoIndex GeoIndex(GeoPoint point)
        {
            return indexes.GetIndex(point);
        }

        public List<Suburb> this[GeoIndex index]
        {
            get => suburbs[index.X][index.Y];
            set => suburbs[index.X][index.Y] = value;
        }

        private static IEnumerable<GeoIndexRange> ToRanges(IEnumerable<GeoIndex> geoIndexes)
        {
            return geoIndexes.GroupBy(x => x.X).SelectMany(x => GetRanges(x.Select(y => y.Y))
                .Select(r => new GeoIndexRange(x.Key, r.Left, r.Right)));
        }

        private static IEnumerable<(int Left, int Right)> GetRanges(IEnumerable<int> indexList)
        {
            var sortedList = indexList.OrderBy(x => x).ToList();
            var i = 0;
            while (i < sortedList.Count)
            {
                var j = i + 1;
                while (j < sortedList.Count && sortedList[j] == sortedList[j - 1] + 1) j++;

                yield return (sortedList[i], sortedList[j - 1]);

                i = j;
            }
        }

        protected IEnumerable<GeoIndex> NearbyIndexes(GeoPoint point, Distance distance)
        {
            var center = indexes.GetIndex(point);
            var length = (int) Math.Floor(distance / resolution) - 1;
            var upper = center.X + length;
            while (indexes.GetGeoPoint(new GeoIndex(upper + 1, center.Y)).GetDistance(point) <= distance)
            {
                upper++;
            }

            var lower = center.X - length;
            while (indexes.GetGeoPoint(new GeoIndex(lower - 1, center.Y)).GetDistance(point) <= distance)
            {
                lower--;
            }

            return NearbyIndexes(Enumerable.Range(center.X, upper - center.X + 1).Reverse(), center.Y, point, distance)
                .Concat(NearbyIndexes(Enumerable.Range(lower, center.X - lower), center.Y, point, distance))
                .Where(indexes.IsIndexValid);
        }

        private IEnumerable<GeoIndex> NearbyIndexes(IEnumerable<int> xs, int y, GeoPoint point, Distance distance)
        {
            var left = y;
            var right = y;
            foreach (var x in xs)
            {
                while (indexes.GetGeoPoint(new GeoIndex(x, left - 1)).GetDistance(point) <= distance)
                {
                    left--;
                }

                while (indexes.GetGeoPoint(new GeoIndex(x, right + 1)).GetDistance(point) <= distance)
                {
                    right++;
                }

                for (var i = left; i <= right; i++)
                    yield return new GeoIndex(x, i);
            }
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