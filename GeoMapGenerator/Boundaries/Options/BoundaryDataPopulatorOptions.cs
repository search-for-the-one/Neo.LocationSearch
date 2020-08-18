using Neo.Extensions.DependencyInjection;

namespace GeoMapGenerator.Boundaries.Options
{
    public class BoundaryDataPopulatorOptions : IConfig
    {
        public double BoundaryResolutionInMetres { get; set; }
    }
}