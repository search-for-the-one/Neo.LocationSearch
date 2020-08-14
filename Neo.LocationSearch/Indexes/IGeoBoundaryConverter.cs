using System.Collections.Generic;
using Neo.LocationSearch.BoundaryDataProviders.Models;
using Neo.LocationSearch.Models;

namespace Neo.LocationSearch.Indexes
{
    internal interface IGeoBoundaryConverter
    {
        IEnumerable<GeoPoint> Convert(Boundary boundary, Distance resolution);
    }
}