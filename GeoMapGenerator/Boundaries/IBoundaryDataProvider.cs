using System.Collections.Generic;
using System.Threading.Tasks;
using GeoMapGenerator.Boundaries.Models;

namespace GeoMapGenerator.Boundaries
{
    internal interface IBoundaryDataProvider
    {
        ValueTask<IEnumerable<SuburbBoundary>> GetBoundaries();
    }
}