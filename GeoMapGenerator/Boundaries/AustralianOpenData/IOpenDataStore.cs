using System.Collections.Generic;
using System.Threading.Tasks;
using GeoMapGenerator.Boundaries.AustralianOpenData.Models;

namespace GeoMapGenerator.Boundaries.AustralianOpenData
{
    internal interface IOpenDataStore
    {
        ValueTask<IEnumerable<OpenDataSuburb>> Get();
    }
}