using Neo.Extensions.DependencyInjection;

namespace Neo.LocationSearch.Indexes.Options
{
    internal class DefaultSuburbConflatorOptions : IConfig
    {
        public bool SkipUncoveredArea { get; set; } = true;
        public bool SkipUnmatchedArea { get; set; } = true;
    }
}