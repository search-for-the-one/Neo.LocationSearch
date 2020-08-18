using System.Collections.Generic;
using System.Threading.Tasks;
using Neo.LocationSearch.Locations.Models;

namespace Neo.LocationSearch.Locations
{
    internal interface ILocationDataProvider
    {
        ValueTask<IEnumerable<SuburbLocation>> Get();
    }
}