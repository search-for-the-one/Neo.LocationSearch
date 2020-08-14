using System.Collections.Generic;
using System.Threading.Tasks;
using Neo.LocationSearch.BoundaryDataProviders.Models;

namespace Neo.LocationSearch.BoundaryDataProviders
{
    internal interface IBoundaryDataProvider
    {
        ValueTask<IEnumerable<SuburbBoundary>> GetBoundaries();
    }
}