using System.IO;
using Binaron.Serializer;
using Neo.LocationSearch.Indexes.Models;
using Neo.LocationSearch.Indexes.Options;
using Newtonsoft.Json;

namespace Neo.LocationSearch.Indexes
{
    internal class DataDumpHandler : IDataDumpHandler
    {
        private readonly GeoMapDumpHandlerOptions options;

        public DataDumpHandler(GeoMapDumpHandlerOptions options)
        {
            this.options = options;
        }

        public void DumpToFile(IGeoMapIndex geoMap)
        {
            var dump = geoMap.Dump();
            DumpToJsonFile(dump);
            DumpToBinaryFile(dump);
        }

        private void DumpToJsonFile(GeoMapData data)
        {
            using var sw = new StreamWriter(options.JsonFile);
            sw.WriteLine(JsonConvert.SerializeObject(data));
        }

        private void DumpToBinaryFile(GeoMapData data)
        {
            if (File.Exists(options.BinaryFile))
                File.Delete(options.BinaryFile);
            
            using var stream = File.Open(options.BinaryFile, FileMode.CreateNew);
            BinaronConvert.Serialize(data, stream);
        }
    }
}