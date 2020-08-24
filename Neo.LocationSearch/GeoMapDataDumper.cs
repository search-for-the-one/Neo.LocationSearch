using System.Collections.Generic;
using System.Linq;
using Neo.LocationSearch.Models;
#if DEBUG
using System.IO;
#endif

namespace Neo.LocationSearch
{
    public class GeoMapDataDumper
    {
        private readonly GeoIndexes indexes;
        private readonly Distance resolution;
        private readonly List<Suburb>[][] suburbs;

        public GeoMapDataDumper(GeoIndexes indexes, Distance resolution, List<Suburb>[][] suburbs)
        {
            this.indexes = indexes;
            this.resolution = resolution;
            this.suburbs = suburbs;
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
    }
}