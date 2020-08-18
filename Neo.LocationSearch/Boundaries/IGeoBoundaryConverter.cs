using System.Collections.Generic;
using Neo.LocationSearch.Boundaries.Models;
using Neo.LocationSearch.Models;

namespace Neo.LocationSearch.Boundaries
{
    internal interface IGeoBoundaryConverter
    {
        IEnumerable<GeoPoint> Convert(Boundary boundary, Distance resolution);
    }
}