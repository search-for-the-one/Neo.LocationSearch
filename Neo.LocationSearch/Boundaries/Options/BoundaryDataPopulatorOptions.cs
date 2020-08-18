using Neo.Extensions.DependencyInjection;

namespace Neo.LocationSearch.Boundaries.Options
{
    public class BoundaryDataPopulatorOptions : IConfig
    {
        public double BoundaryResolutionInMetres { get; set; }
    }
}