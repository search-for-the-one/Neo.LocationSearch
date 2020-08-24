using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Neo.LocationSearch.Models;

namespace Neo.LocationSearch
{
    public class NearestEnumerator
    {
        private readonly GeoIndexes indexes;
        private readonly Distance resolution;

        public NearestEnumerator(GeoIndexes indexes, Distance resolution)
        {
            this.indexes = indexes ?? throw new ArgumentNullException(nameof(indexes));

            if (resolution <= Distance.FromMetres(0))
                throw new ArgumentOutOfRangeException(nameof(resolution));
            
            this.resolution = resolution;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<GeoIndex> Enumerate(GeoPoint point, Distance distance)
        {
            if (distance < Distance.FromMetres(0))
                return Enumerable.Empty<GeoIndex>();
            
            var center = indexes.GetIndex(point);
            if (!indexes.IsIndexValid(center))
                throw new ArgumentOutOfRangeException(nameof(point), $"Geo map data does not cover location {point}");

            var length = Math.Max((int) Math.Floor(distance / resolution) - 1, 0);
            var right = center.X + length;
            while (indexes.GetGeoPoint(new GeoIndex(right + 1, center.Y)).GetDistance(point) <= distance)
            {
                right++;
            }

            var left = center.X - length;
            while (indexes.GetGeoPoint(new GeoIndex(left - 1, center.Y)).GetDistance(point) <= distance)
            {
                left--;
            }

            left = indexes.ClampX(left);
            right = indexes.ClampX(right);

            return Enumerate(Enumerable.Range(center.X, right - center.X + 1).Reverse(), center.Y, point, distance)
                .Concat(Enumerate(Enumerable.Range(left, center.X - left), center.Y, point, distance));
        }

        private IEnumerable<GeoIndex> Enumerate(IEnumerable<int> xs, int y, GeoPoint point, Distance distance)
        {
            var top = y;
            var bottom = y;
            foreach (var x in xs)
            {
                while (indexes.GetGeoPoint(new GeoIndex(x, top - 1)).GetDistance(point) <= distance)
                {
                    top--;
                }

                while (indexes.GetGeoPoint(new GeoIndex(x, bottom + 1)).GetDistance(point) <= distance)
                {
                    bottom++;
                }

                top = indexes.ClampY(top);
                bottom = indexes.ClampY(bottom);

                for (var i = top; i <= bottom; i++)
                    yield return new GeoIndex(x, i);
            }
        }
    }
}