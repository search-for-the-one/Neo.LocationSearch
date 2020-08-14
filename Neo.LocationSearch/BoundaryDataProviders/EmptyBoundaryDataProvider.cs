using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo.LocationSearch.BoundaryDataProviders.Models;

namespace Neo.LocationSearch.BoundaryDataProviders
{
    internal class EmptyBoundaryDataProvider : IBoundaryDataProvider
    {
        public ValueTask<IEnumerable<SuburbBoundary>> GetBoundaries()
        {
            return new ValueTask<IEnumerable<SuburbBoundary>>(Enumerable.Empty<SuburbBoundary>());
        }
    }
}