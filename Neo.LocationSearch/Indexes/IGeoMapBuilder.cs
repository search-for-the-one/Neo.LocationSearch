using System.Threading.Tasks;

namespace Neo.LocationSearch.Indexes
{
    internal interface IGeoMapBuilder
    {
        ValueTask<IGeoMapIndex> Build();
    }
}