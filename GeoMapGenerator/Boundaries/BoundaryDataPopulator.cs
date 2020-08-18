using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoMapGenerator.Boundaries.Options;
using GeoMapGenerator.Indexes;
using Neo.LocationSearch;
using Neo.LocationSearch.Models;

namespace GeoMapGenerator.Boundaries
{
    internal class BoundaryDataPopulator : IDataPopulator
    {
        private readonly IBoundaryDataProvider dataProvider;
        private readonly IGeoBoundaryConverter boundaryConverter;
        private readonly BoundaryDataPopulatorOptions options;

        public BoundaryDataPopulator(IBoundaryDataProvider dataProvider, IGeoBoundaryConverter boundaryConverter, BoundaryDataPopulatorOptions options)
        {
            this.dataProvider = dataProvider;
            this.boundaryConverter = boundaryConverter;
            this.options = options;
        }

        public async ValueTask Populate(IGeoMapIndex map)
        {
            var resolution = Distance.FromMetres(options.BoundaryResolutionInMetres);
            var suburbBoundaries = await dataProvider.GetBoundaries();
            foreach (var suburbBoundary in suburbBoundaries)
            {
                foreach (var geoIndex in boundaryConverter.Convert(suburbBoundary.Boundary, resolution).Select(map.GeoIndex).Distinct())
                {
                    map[geoIndex] ??= new List<Suburb>();
                    map[geoIndex].Add(suburbBoundary.Suburb);
                }
            }
        }
    }
}