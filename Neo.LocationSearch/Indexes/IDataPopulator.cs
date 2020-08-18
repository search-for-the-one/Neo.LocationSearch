using System.Threading.Tasks;

namespace Neo.LocationSearch.Indexes
{
    internal interface IDataPopulator
    {
        ValueTask Populate(IGeoMapIndex map);
    }
}