using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoMapGenerator.Locations.Models;

namespace GeoMapGenerator.Locations
{
    internal class EmptyLocationDataProvider : ILocationDataProvider
    {
        public ValueTask<IEnumerable<SuburbLocation>> Get()
        {
            return new ValueTask<IEnumerable<SuburbLocation>>(Enumerable.Empty<SuburbLocation>());
        }
    }
}