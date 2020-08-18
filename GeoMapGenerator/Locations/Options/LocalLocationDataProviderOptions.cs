using Neo.Extensions.DependencyInjection;

namespace GeoMapGenerator.Locations.Options
{
    internal class LocalLocationDataProviderOptions : IConfig
    {
        public string File { get; set; }
    }
}