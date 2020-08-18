using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoMapGenerator.Boundaries.Models;

namespace GeoMapGenerator.Boundaries
{
    internal class EmptyBoundaryDataProvider : IBoundaryDataProvider
    {
        public ValueTask<IEnumerable<SuburbBoundary>> GetBoundaries()
        {
            return new ValueTask<IEnumerable<SuburbBoundary>>(Enumerable.Empty<SuburbBoundary>());
        }
    }
}