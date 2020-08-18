using System.Threading.Tasks;
using Neo.LocationSearch;

namespace GeoMapGenerator.Indexes
{
    internal interface IGeoMapBuilder
    {
        ValueTask<IGeoMapIndex> Build();
    }
}