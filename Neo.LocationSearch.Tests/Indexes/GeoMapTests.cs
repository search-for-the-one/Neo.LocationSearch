using System.Collections.Generic;
using System.Linq;
using GeoMapGenerator.Indexes.Options;
using Neo.LocationSearch.Models;
using NUnit.Framework;

namespace Neo.LocationSearch.Tests.Indexes
{
    public class GeoMapTests
    {
        [Test]
        public void TestGeoMap()
        {
            var map = new GeoMapIndex(GeoMapBuilderOptions.AustraliaSouthWest, GeoMapBuilderOptions.AustraliaNorthEast, Distance.FromKilometres(1));

            var suburb = new Suburb {Name = "Cremorne", Postcode = "3121"};
            var point = new GeoPoint(-37.828280, 144.988010);
            map[point] = new List<Suburb>(new[] {suburb});

            Assert.AreEqual(suburb, map.NearbySuburbs(point, Distance.FromKilometres(1)).Single());
        }
    }
}