using Neo.Extensions.DependencyInjection;

namespace GeoMapGenerator.Locations.Options
{
    internal class LocationDataPopulatorOptions : IConfig
    {
        public bool SkipUncoveredArea { get; set; } = true;
        public bool SkipUnmatchedArea { get; set; } = true;
    }
}