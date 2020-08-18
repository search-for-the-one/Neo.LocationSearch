using System.IO;
using Binaron.Serializer;
using Neo.LocationSearch;
using Neo.LocationSearch.Models;
using Newtonsoft.Json;

namespace GeoMapUsageExample
{
    public class GeoMapConstructor : IGeoMapConstructor
    {
        public IGeoMap FromJsonFile(string dataFile)
        {
            return new GeoMapIndex(JsonConvert.DeserializeObject<GeoMapData>(File.ReadAllText(dataFile)));
        }

        public IGeoMap FromBinaryFile(string dataFile)
        {
            return new GeoMapIndex(BinaronConvert.Deserialize<GeoMapData>(File.OpenRead(dataFile)));
        }
    }
}