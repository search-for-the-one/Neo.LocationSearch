using Neo.LocationSearch;

namespace GeoMapGenerator.Indexes
{
    internal interface IDataDumpHandler
    {
        void DumpToFile(IGeoMapIndex geoMap);
    }
}