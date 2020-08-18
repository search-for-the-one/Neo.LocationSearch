using Neo.Extensions.DependencyInjection;
using Neo.LocationSearch.Boundaries;
using Neo.LocationSearch.Locations;

namespace Neo.LocationSearch.Options
{
    internal class FactoryOptions : IConfig
    {
        public string OpenDataStore { get; set; } = nameof(OpenDataStore);
        public string BoundaryDataProvider { get; set; } = nameof(EmptyBoundaryDataProvider);
        public string LocationDataProvider { get; set; } = nameof(EmptyLocationDataProvider);
    }
}