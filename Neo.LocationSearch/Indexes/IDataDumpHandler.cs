namespace Neo.LocationSearch.Indexes
{
    internal interface IDataDumpHandler
    {
        void DumpToFile(IGeoMapIndex geoMap);
    }
}