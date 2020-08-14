using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo.LocationSearch.BoundaryDataProviders;
using Neo.LocationSearch.Indexes.Options;
using Neo.LocationSearch.LocationDataProviders;
using Neo.LocationSearch.Models;

namespace Neo.LocationSearch.Indexes
{
    internal class GeoMapBuilder : IGeoMapBuilder
    {
        private readonly IBoundaryDataProvider boundaryDataProvider;
        private readonly IGeoBoundaryConverter geoBoundaryConverter;
        private readonly GeoMapBuilderOptions geoMapBuilderOptions;
        private readonly ILocationDataProvider locationDataProvider;
        private readonly ISuburbConflator suburbConflator;

        public GeoMapBuilder(IBoundaryDataProvider boundaryDataProvider, ILocationDataProvider locationDataProvider,
            IGeoBoundaryConverter geoBoundaryConverter, ISuburbConflator suburbConflator, GeoMapBuilderOptions geoMapBuilderOptions)
        {
            this.geoMapBuilderOptions = geoMapBuilderOptions;
            this.boundaryDataProvider = boundaryDataProvider;
            this.locationDataProvider = locationDataProvider;
            this.geoBoundaryConverter = geoBoundaryConverter;
            this.suburbConflator = suburbConflator;
        }

        public async ValueTask<IGeoMapIndex> Build()
        {
            var map = new GeoMapIndex(geoMapBuilderOptions.SouthWest, geoMapBuilderOptions.NorthEast,
                Distance.FromMetres(geoMapBuilderOptions.MapResolutionInMetres));

            var boundaryResolution = Distance.FromMetres(geoMapBuilderOptions.BoundaryResolutionInMetres);
            await PopulateBoundaries(map, boundaryResolution);

            await PopulateLocations(map);

            return map;
        }

        private async ValueTask PopulateBoundaries(IGeoMapIndex map, Distance resolution)
        {
            var suburbBoundaries = await boundaryDataProvider.GetBoundaries();
            foreach (var suburbBoundary in suburbBoundaries)
            {
                foreach (var geoIndex in geoBoundaryConverter.Convert(suburbBoundary.Boundary, resolution).Select(map.GeoIndex).Distinct())
                {
                    map[geoIndex] ??= new List<Suburb>();
                    map[geoIndex].Add(suburbBoundary.Suburb);
                }
            }
        }

        private async ValueTask PopulateLocations(IGeoMapIndex map)
        {
            var suburbLocations = await locationDataProvider.Get();
            foreach (var suburbLocation in suburbLocations)
            {
                AddSuburb(map, suburbLocation.GeoLocation, suburbLocation.Suburb);
            }
        }

        private void AddSuburb(IGeoMap map, GeoPoint point, Suburb suburb)
        {
            map[point] = suburbConflator.Conflate(map[point], suburb);
        }
    }
}