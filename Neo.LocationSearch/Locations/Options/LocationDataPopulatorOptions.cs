using Neo.Extensions.DependencyInjection;

namespace Neo.LocationSearch.Locations.Options
{
    internal class LocationDataPopulatorOptions : IConfig
    {
        public bool SkipUncoveredArea { get; set; } = true;
        public bool SkipUnmatchedArea { get; set; } = true;
    }
}