using System.Collections.Generic;
using System.Threading.Tasks;
using Neo.LocationSearch.LocationDataProviders.Models;

namespace Neo.LocationSearch.LocationDataProviders
{
    internal interface ILocationDataProvider
    {
        ValueTask<IEnumerable<SuburbLocation>> Get();
    }
}