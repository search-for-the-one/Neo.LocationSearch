using System.Collections.Generic;

namespace Neo.LocationSearch.Indexes.Models
{
    internal class GeoIndexRange
    {
        public GeoIndexRange(int[] values) : this(values[0], values[1], values[2])
        {
        }

        public GeoIndexRange(int x, int l, int r)
        {
            X = x;
            L = l;
            R = r;
        }

        public int X { get; set; }
        public int L { get; set; }
        public int R { get; set; }

        public int[] ToArray()
        {
            return new[] {X, L, R};
        }
    }
}