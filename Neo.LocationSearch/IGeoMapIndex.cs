using System.Collections.Generic;
using Neo.LocationSearch.Models;

namespace Neo.LocationSearch
{
    internal interface IGeoMapIndex : IGeoMap
    {
        List<Suburb> this[GeoIndex index] { get; set; }
        GeoMapData Dump();
        GeoIndex GeoIndex(GeoPoint point);
    }
}