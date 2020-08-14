using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo.LocationSearch.LocationDataProviders.Models;

namespace Neo.LocationSearch.LocationDataProviders
{
    internal class EmptyLocationDataProvider : ILocationDataProvider
    {
        public ValueTask<IEnumerable<SuburbLocation>> Get()
        {
            return new ValueTask<IEnumerable<SuburbLocation>>(Enumerable.Empty<SuburbLocation>());
        }
    }
}