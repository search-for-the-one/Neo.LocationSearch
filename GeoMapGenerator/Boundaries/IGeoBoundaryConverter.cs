using System.Collections.Generic;
using GeoMapGenerator.Boundaries.Models;
using Neo.LocationSearch.Models;

namespace GeoMapGenerator.Boundaries
{
    internal interface IGeoBoundaryConverter
    {
        IEnumerable<GeoPoint> Convert(Boundary boundary, Distance resolution);
    }
}