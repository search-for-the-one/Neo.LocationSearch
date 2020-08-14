using System.Collections.Generic;

namespace Neo.LocationSearch.Indexes.Models
{
    internal class GeoMapData
    {
        public GeoIndexesData GeoIndexes { get; set; }
        public double ResolutionInMetres { get; set; }
        public List<SuburbGeoIndexes> Suburbs { get; set; }
    }
}