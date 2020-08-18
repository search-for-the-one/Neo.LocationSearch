using System.Threading.Tasks;
using Neo.LocationSearch;

namespace GeoMapGenerator.Indexes
{
    internal interface IDataPopulator
    {
        ValueTask Populate(IGeoMapIndex map);
    }
}