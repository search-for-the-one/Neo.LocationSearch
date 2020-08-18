using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Neo.LocationSearch.Boundaries.Models;
using Neo.LocationSearch.Indexes.Models;
using Neo.LocationSearch.Models;

namespace Neo.LocationSearch.Boundaries
{
    internal class GeoBoundaryConverter : IGeoBoundaryConverter
    {
        private readonly IPolygonElementSelector polygonElementSelector;

        public GeoBoundaryConverter(IPolygonElementSelector polygonElementSelector)
        {
            this.polygonElementSelector = polygonElementSelector;
        }

        public IEnumerable<GeoPoint> Convert(Boundary boundary, Distance resolution)
        {
            var indexes = GetGeoIndexes(boundary, resolution);

            var elements = polygonElementSelector.Elements(boundary.Polygons.Select(x => ToPointPolygon(x, indexes)).ToList(),
                boundary.ExcludePolygons.Select(x => ToPointPolygon(x, indexes)).ToList());
            foreach (var element in elements)
            {
                yield return indexes.GetGeoPoint(new GeoIndex(element.X, element.Y));
            }
        }

        private static PointPolygon ToPointPolygon(Polygon polygon, GeoIndexes indexes)
        {
            return new PointPolygon
            {
                Points = ToPoints(polygon, indexes).ToArray()
            };
        }

        private static IEnumerable<Point> ToPoints(Polygon polygon, GeoIndexes indexes)
        {
            var previous = new Point(-1, -1);
            foreach (var current in polygon.Select(indexes.GetIndex).Select(index => new Point(index.X, index.Y)))
            {
                if (current != previous)
                    yield return current;
                previous = current;
            }
        }

        private static GeoIndexes GetGeoIndexes(Boundary boundary, Distance resolution)
        {
            var area = GeoArea.FromGeoPoints(boundary.Polygons.Concat(boundary.ExcludePolygons).SelectMany(x => x));
            return new GeoIndexes(area.SouthWest, area.NorthEast, resolution);
        }
    }
}