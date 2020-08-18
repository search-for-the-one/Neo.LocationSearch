using System.Collections.Generic;
using System.Threading.Tasks;
using Neo.LocationSearch.Boundaries.AustralianOpenData.Models;

namespace Neo.LocationSearch.Boundaries.AustralianOpenData
{
    internal interface IOpenDataStore
    {
        ValueTask<IEnumerable<OpenDataSuburb>> Get();
    }
}