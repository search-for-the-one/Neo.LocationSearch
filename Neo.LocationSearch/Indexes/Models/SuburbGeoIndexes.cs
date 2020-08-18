using System.Collections.Generic;
using Neo.LocationSearch.Models;

namespace Neo.LocationSearch.Indexes.Models
{
    internal class SuburbGeoIndexes
    {
        public Suburb Suburb { get; set; }
        public List<int[]> IndexRanges { get; set; }
    }
}