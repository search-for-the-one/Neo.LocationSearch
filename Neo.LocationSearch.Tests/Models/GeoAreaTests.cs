using Neo.LocationSearch.Models;
using NUnit.Framework;

namespace Neo.LocationSearch.Tests.Models
{
    public class GeoAreaTests
    {
        [Test]
        public void TestFromGeoPoints()
        {
            var area = GeoArea.FromGeoPoints(new[] {new GeoPoint(1, 2), new GeoPoint(2, 1), new GeoPoint(2, 3)});
            Assert.AreEqual(new GeoPoint(1, 1), area.SouthWest);
            Assert.AreEqual(new GeoPoint(2, 3), area.NorthEast);
        }

        [Test]
        public void TestOverlapWith()
        {
            Assert.IsFalse(new GeoArea(new GeoPoint(0, 0), new GeoPoint(1, 1)).OverlapWith(new GeoArea(new GeoPoint(2, 2), new GeoPoint(3, 3))));
            Assert.IsTrue(new GeoArea(new GeoPoint(0, 0), new GeoPoint(3, 3)).OverlapWith(new GeoArea(new GeoPoint(2, 2), new GeoPoint(4, 4))));
        }
    }
}