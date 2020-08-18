using Neo.Extensions.DependencyInjection;

namespace GeoMapGenerator.Indexes.Options
{
    internal class GeoMapDumpHandlerOptions : IConfig
    {
        public string JsonFile { get; set; }
        public string BinaryFile { get; set; }
    }
}