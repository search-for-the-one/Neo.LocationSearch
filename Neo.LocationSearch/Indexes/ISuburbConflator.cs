using System.Collections.Generic;
using Neo.LocationSearch.Models;

namespace Neo.LocationSearch.Indexes
{
    internal interface ISuburbConflator
    {
        List<Suburb> Conflate(List<Suburb> existingSuburbs, Suburb suburb);
    }
}