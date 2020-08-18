using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo.LocationSearch.Boundaries.AustralianOpenData.Models;
using Neo.LocationSearch.Boundaries.Models;
using Neo.LocationSearch.Models;

namespace Neo.LocationSearch.Boundaries.AustralianOpenData
{
    internal class OpenDataBoundaryDataProvider : IBoundaryDataProvider
    {
        private readonly IOpenDataStore dataStore;

        public OpenDataBoundaryDataProvider(IOpenDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public async ValueTask<IEnumerable<SuburbBoundary>> GetBoundaries()
        {
            var suburbs = await dataStore.Get();

            return suburbs.GroupBy(x => x.State).SelectMany(x => x.GroupBy(y => y.Name).SelectMany(GetBoundaries));
        }

        private static IEnumerable<SuburbBoundary> GetBoundaries(IEnumerable<OpenDataSuburb> sameNameSuburbs)
        {
            var suburbList = sameNameSuburbs.ToList();
            if (suburbList.Count <= 1)
                return new[] {CreateSuburbBoundary(suburbList)};

            var idSuburbs = suburbList.GroupBy(x => x.Id).Select(x => x.ToList()).ToList();
            if (idSuburbs.Count <= 1)
                return new[] {CreateSuburbBoundary(suburbList)};

            var boundaries = idSuburbs.Select(suburbs =>
            {
                var boundary = CreateSuburbBoundary(suburbs);
                return (LastUpdateTime: suburbs.First().UpdateDate, Boundary: boundary,
                    GeoArea: GeoArea.FromGeoPoints(boundary.Boundary.Polygons.SelectMany(x => x)));
            }).OrderByDescending(x => x.LastUpdateTime);

            return GetNoneOverlappedBoundaries(boundaries);
        }

        private static IEnumerable<SuburbBoundary> GetNoneOverlappedBoundaries(
            IEnumerable<(DateTime LastUpdateTime, SuburbBoundary Boundary, GeoArea GeoArea)> boundaries)
        {
            var boundaryList = boundaries.ToList();
            for (var i = 0; i < boundaryList.Count; i++)
            {
                if (!boundaryList.Take(i).Any(x => boundaryList[i].GeoArea.OverlapWith(x.GeoArea)))
                    yield return boundaryList[i].Boundary;
            }
        }

        private static SuburbBoundary CreateSuburbBoundary(IReadOnlyCollection<OpenDataSuburb> suburbs)
        {
            return new SuburbBoundary
            {
                Suburb = new Suburb {Name = suburbs.First().Name, State = suburbs.First().State},
                Boundary = GetBoundary(suburbs.Select(x => x.Boundary))
            };
        }

        private static Boundary GetBoundary(IEnumerable<Geometry> geometries)
        {
            var polygons = new List<Polygon>();
            var excludePolygons = new List<Polygon>();

            foreach (var geometry in geometries)
            {
                foreach (var area in geometry.Coordinates)
                {
                    foreach (var polygon in area)
                    {
                        var geoPolygon = new Polygon(polygon.Select(x => new GeoPoint(x[1], x[0])));
                        if (IsClockWise(polygon))
                            polygons.Add(geoPolygon);
                        else
                            excludePolygons.Add(geoPolygon);
                    }
                }
            }

            return new Boundary
            {
                Polygons = polygons,
                ExcludePolygons = excludePolygons
            };
        }

        private static bool IsClockWise(double[][] polygon)
        {
            var minX = polygon.Min(x => x[0]) - 1;
            var minY = polygon.Min(x => x[1]) - 1;

            var n = polygon.Length;
            var area = 0d;
            for (var i = 0; i < n; i++)
            {
                area += (polygon[i][0] - minX) * (polygon[(i + 1) % n][1] - minY) - (polygon[i][1] - minY) * (polygon[(i + 1) % n][0] - minX);
            }

            return area < 0;
        }
    }
}