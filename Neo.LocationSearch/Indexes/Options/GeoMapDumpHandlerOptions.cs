using Neo.Extensions.DependencyInjection;

namespace Neo.LocationSearch.Indexes.Options
{
    internal class GeoMapDumpHandlerOptions : IConfig
    {
        public string JsonFile { get; set; }
        public string BinaryFile { get; set; }
    }
}