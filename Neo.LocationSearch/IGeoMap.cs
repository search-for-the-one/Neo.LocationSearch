using System.Collections.Generic;
using Neo.LocationSearch.Models;

namespace Neo.LocationSearch
{
    public interface IGeoMap
    {
        List<Suburb> this[GeoPoint point] { get; set; }
        IEnumerable<Suburb> GetNearestSuburbs(GeoPoint point, Distance distance);
    }
}