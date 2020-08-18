using Neo.Extensions.DependencyInjection;
using Neo.LocationSearch.Models;

namespace GeoMapGenerator.Indexes.Options
{
    public class GeoMapBuilderOptions : IConfig
    {
        public static readonly GeoPoint AustraliaSouthWest = new GeoPoint(-43.7, 112.8);
        public static readonly GeoPoint AustraliaNorthEast = new GeoPoint(-9, 159.2);
        public double MapResolutionInMetres { get; set; }
        public GeoPoint SouthWest { get; set; } = AustraliaSouthWest;
        public GeoPoint NorthEast { get; set; } = AustraliaNorthEast;
    }
}