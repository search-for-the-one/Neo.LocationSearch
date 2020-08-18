using Neo.Extensions.DependencyInjection;

namespace Neo.LocationSearch.Locations.Options
{
    internal class LocalLocationDataProviderOptions : IConfig
    {
        public string File { get; set; }
    }
}