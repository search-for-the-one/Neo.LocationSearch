using System.Collections.Generic;
using System.Threading.Tasks;
using Neo.LocationSearch.Boundaries.Models;

namespace Neo.LocationSearch.Boundaries
{
    internal interface IBoundaryDataProvider
    {
        ValueTask<IEnumerable<SuburbBoundary>> GetBoundaries();
    }
}