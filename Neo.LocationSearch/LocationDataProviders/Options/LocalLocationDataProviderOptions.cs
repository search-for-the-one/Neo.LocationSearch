using Neo.Extensions.DependencyInjection;

namespace Neo.LocationSearch.LocationDataProviders.Options
{
    internal class LocalLocationDataProviderOptions : IConfig
    {
        public string File { get; set; }
    }
}