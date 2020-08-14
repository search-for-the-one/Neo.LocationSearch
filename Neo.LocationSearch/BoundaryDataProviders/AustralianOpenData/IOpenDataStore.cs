using System.Collections.Generic;
using System.Threading.Tasks;
using Neo.LocationSearch.BoundaryDataProviders.AustralianOpenData.Models;

namespace Neo.LocationSearch.BoundaryDataProviders.AustralianOpenData
{
    internal interface IOpenDataStore
    {
        ValueTask<IEnumerable<OpenDataSuburb>> Get();
    }
}