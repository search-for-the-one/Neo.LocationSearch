using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo.LocationSearch.Locations.Models;

namespace Neo.LocationSearch.Locations
{
    internal class EmptyLocationDataProvider : ILocationDataProvider
    {
        public ValueTask<IEnumerable<SuburbLocation>> Get()
        {
            return new ValueTask<IEnumerable<SuburbLocation>>(Enumerable.Empty<SuburbLocation>());
        }
    }
}