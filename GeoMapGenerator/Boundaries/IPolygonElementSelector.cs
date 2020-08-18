using System.Collections.Generic;
using System.Drawing;
using GeoMapGenerator.Indexes.Models;

namespace GeoMapGenerator.Boundaries
{
    internal interface IPolygonElementSelector
    {
        IEnumerable<Point> Elements(ICollection<PointPolygon> polygons, ICollection<PointPolygon> excludePolygons);
    }
}