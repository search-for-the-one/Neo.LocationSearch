using System.Collections.Generic;
using System.Drawing;
using Neo.LocationSearch.Indexes.Models;

namespace Neo.LocationSearch.Indexes
{
    internal interface IPolygonElementSelector
    {
        IEnumerable<Point> Elements(ICollection<PointPolygon> polygons, ICollection<PointPolygon> excludePolygons);
    }
}