using System.Collections.Generic;

namespace Neo.LocationSearch.Models
{
    public class GeoMapData
    {
        public GeoIndexesData GeoIndexes { get; set; }
        public double ResolutionInMetres { get; set; }
        public List<SuburbGeoIndexes> Suburbs { get; set; }
    }
}