using Neo.LocationSearch;

namespace GeoMapUsageExample
{
    public interface IGeoMapConstructor
    {
        IGeoMap FromJsonFile(string dataFile);
        IGeoMap FromBinaryFile(string dataFile);
    }
}