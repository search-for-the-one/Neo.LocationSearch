using System.Collections.Generic;
using Neo.LocationSearch.Models;

namespace GeoMapGenerator.Boundaries.Models
{
    internal class Polygon : List<GeoPoint>
    {
        public Polygon()
        {
        }

        public Polygon(IEnumerable<GeoPoint> collection) : base(collection)
        {
        }

        public Polygon(int capacity) : base(capacity)
        {
        }
    }
}