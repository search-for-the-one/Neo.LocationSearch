using System.Collections.Generic;

namespace Neo.LocationSearch.BoundaryDataProviders.Models
{
    internal class Boundary
    {
        public List<Polygon> Polygons { get; set; }
        public List<Polygon> ExcludePolygons { get; set; }
    }
}