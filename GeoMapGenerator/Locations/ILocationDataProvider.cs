using System.Collections.Generic;
using System.Threading.Tasks;
using GeoMapGenerator.Locations.Models;

namespace GeoMapGenerator.Locations
{
    internal interface ILocationDataProvider
    {
        ValueTask<IEnumerable<SuburbLocation>> Get();
    }
}