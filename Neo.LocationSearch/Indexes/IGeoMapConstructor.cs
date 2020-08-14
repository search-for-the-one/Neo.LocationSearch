namespace Neo.LocationSearch.Indexes
{
    public interface IGeoMapConstructor
    {
        IGeoMap FromJsonFile(string dataFile);
        IGeoMap FromBinaryFile(string dataFile);
    }
}