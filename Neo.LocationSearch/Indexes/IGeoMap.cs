using System.Collections.Generic;
using Neo.LocationSearch.Models;

namespace Neo.LocationSearch.Indexes
{
    public interface IGeoMap
    {
        List<Suburb> this[GeoPoint point] { get; set; }
        IEnumerable<Suburb> NearbySuburbs(GeoPoint point, Distance distance);
    }
}