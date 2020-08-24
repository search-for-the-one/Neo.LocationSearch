using System;
using System.Collections.Generic;
using GeoMapGenerator.Indexes.Options;
using Neo.LocationSearch.Models;
using NUnit.Framework;

namespace Neo.LocationSearch.Tests
{
    public class GeoMapIndexTests
    {
        private readonly GeoIndexes indexes;
        private readonly Distance resolution;
        
        public GeoMapIndexTests()
        {
            indexes = new GeoIndexes(GeoMapBuilderOptions.AustraliaSouthWest, GeoMapBuilderOptions.AustraliaNorthEast, Distance.FromKilometres(1));
            resolution = Distance.FromKilometres(1);
        }

        [Test]
        public void OutOfRangeTest()
        {
            var point = new GeoPoint(36.081764,-115.1459853); // near Las Vegas airport!
            var distance = Distance.FromKilometres(1);
            var enumerator = new NearestEnumerator(indexes, resolution);
            Assert.Throws<ArgumentOutOfRangeException>(() => enumerator.Enumerate(point, distance));
        }

        [TestCase(-43.7, 112.8, 25)]
        [TestCase(-9.1, 159.1, 25)]
        [TestCase(-43.7, 159.1, 25)]
        [TestCase(-9.1, 112.8, 25)]
        [TestCase(-37.841711, 145.058282, 0)]
        [TestCase(-37.841711, 145.058282, 1)]
        [TestCase(-35.576834, 146.445648, 1)]
        [TestCase(-37.841711, 145.058282, 5)]
        [TestCase(-35.576834, 146.445648, 5)]
        [TestCase(-37.841711, 145.058282, 10)]
        [TestCase(-35.576834, 146.445648, 10)]
        [TestCase(-37.841711, 145.058282, 25)]
        [TestCase(-35.576834, 146.445648, 25)]
        public void TestNearestIndexes(double latitude, double longitude, double distanceInKm)
        {
            var point = new GeoPoint(latitude, longitude);
            var distance = Distance.FromKilometres(distanceInKm);
            var enumerator = new NearestEnumerator(indexes, resolution);

            var expected = NaiveNearestIndexes(point, distance);
            var actual = enumerator.Enumerate(point, distance);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        private IEnumerable<GeoIndex> NaiveNearestIndexes(GeoPoint point, Distance distance)
        {
            var center = indexes.GetIndex(point);
            var length = (int) Math.Ceiling(distance / resolution) * 2;

            if (length == 0)
            {
                var index = new GeoIndex(center.X, center.Y);
                if (indexes.IsIndexValid(index))
                    yield return index;
                yield break;
            }

            for (var x = center.X - length; x <= center.X + length; x++)
            for (var y = center.Y - length; y <= center.Y + length; y++)
            {
                var index = new GeoIndex(x, y);
                if (indexes.IsIndexValid(index) && indexes.GetGeoPoint(index).GetDistance(point) <= distance)
                    yield return index;
            }
        }
    }
}