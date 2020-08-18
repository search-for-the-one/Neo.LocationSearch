using System.Collections.Generic;

namespace Neo.LocationSearch.Models
{
    public class SuburbGeoIndexes
    {
        public Suburb Suburb { get; set; }
        public List<int[]> IndexRanges { get; set; }
    }
}