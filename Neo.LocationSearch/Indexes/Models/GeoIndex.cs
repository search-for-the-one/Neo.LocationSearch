using System;

#nullable enable
namespace Neo.LocationSearch.Indexes.Models
{
    internal struct GeoIndex : IEquatable<GeoIndex>
    {
        public GeoIndex(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override int GetHashCode()
        {
            return $"{X},{Y}".GetHashCode();
        }

        public bool Equals(GeoIndex other)
        {
            return X == other.X && Y == other.Y;
        }
    }
}