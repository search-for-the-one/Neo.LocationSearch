using System;
using System.Collections.Generic;
using System.Linq;
using GeoMapGenerator.Indexes.Options;
using Neo.LocationSearch.Models;
using NUnit.Framework;

namespace Neo.LocationSearch.Tests
{
    public class GeoMapIndexTests : GeoMapIndex
    {
        private readonly GeoIndexes indexes;
        private readonly Distance resolution;
        
        public GeoMapIndexTests() : base(GeoMapBuilderOptions.AustraliaSouthWest, GeoMapBuilderOptions.AustraliaNorthEast, Distance.FromKilometres(1))
        {
            indexes = new GeoIndexes(GeoMapBuilderOptions.AustraliaSouthWest, GeoMapBuilderOptions.AustraliaNorthEast, Distance.FromKilometres(1));
            resolution = Distance.FromKilometres(1);
        }

        [TestCase(-43.7, 112.8, 25)]
        [TestCase(-9, 159.2, 25)]
        [TestCase(-43.7, 159.2, 25)]
        [TestCase(-9, 112.8, 25)]
        [TestCase(-37.841711, 145.058282, 1)]
        [TestCase(-35.576834, 146.445648, 1)]
        [TestCase(-37.841711, 145.058282, 5)]
        [TestCase(-35.576834, 146.445648, 5)]
        [TestCase(-37.841711, 145.058282, 10)]
        [TestCase(-35.576834, 146.445648, 10)]
        [TestCase(-37.841711, 145.058282, 25)]
        [TestCase(-35.576834, 146.445648, 25)]
        public void TestNearbyIndexes(double latitude, double longitude, double distanceInKm)
        {
            var point = new GeoPoint(latitude, longitude);
            var distance = Distance.FromKilometres(distanceInKm);

            var expected = NaiveNearbyIndexes(point, distance).OrderBy(x => x.X).ThenBy(x => x.Y);
            var actual = NearbyIndexes(point, distance).OrderBy(x => x.X).ThenBy(x => x.Y);
            CollectionAssert.AreEqual(expected, actual);
        }

        private IEnumerable<GeoIndex> NaiveNearbyIndexes(GeoPoint point, Distance distance)
        {
            var center = indexes.GetIndex(point);
            var length = (int) Math.Ceiling(distance / resolution) * 2;

            for (var x = center.X - length; x <= center.X + length; x++)
            {
                for (var y = center.Y - length; y <= center.Y + length; y++)
                {
                    var index = new GeoIndex(x, y);
                    if(indexes.IsIndexValid(index) && indexes.GetGeoPoint(index).GetDistance(point) <= distance)
                        yield return index;
                }
            }
        }
    }
}