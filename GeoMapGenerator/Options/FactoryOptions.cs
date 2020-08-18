using GeoMapGenerator.Boundaries;
using GeoMapGenerator.Locations;
using Neo.Extensions.DependencyInjection;

namespace GeoMapGenerator.Options
{
    internal class FactoryOptions : IConfig
    {
        public string OpenDataStore { get; set; } = nameof(OpenDataStore);
        public string BoundaryDataProvider { get; set; } = nameof(EmptyBoundaryDataProvider);
        public string LocationDataProvider { get; set; } = nameof(EmptyLocationDataProvider);
    }
}