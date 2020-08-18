using System.Collections.Generic;
using System.Threading.Tasks;
using GeoMapGenerator.Indexes.Options;
using Neo.LocationSearch;
using Neo.LocationSearch.Models;

namespace GeoMapGenerator.Indexes
{
    internal class GeoMapBuilder : IGeoMapBuilder
    {
        private readonly IEnumerable<IDataPopulator> dataPopulators;
        private readonly GeoMapBuilderOptions geoMapBuilderOptions;

        public GeoMapBuilder(IEnumerable<IDataPopulator> dataPopulators, GeoMapBuilderOptions geoMapBuilderOptions)
        {
            this.dataPopulators = dataPopulators;
            this.geoMapBuilderOptions = geoMapBuilderOptions;
        }

        public async ValueTask<IGeoMapIndex> Build()
        {
            var map = new GeoMapIndex(geoMapBuilderOptions.SouthWest, geoMapBuilderOptions.NorthEast,
                Distance.FromMetres(geoMapBuilderOptions.MapResolutionInMetres));

            foreach (var dataPopulator in dataPopulators)
            {
                await dataPopulator.Populate(map);
            }

            return map;
        }
    }
}