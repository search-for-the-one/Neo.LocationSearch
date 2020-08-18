using Neo.LocationSearch.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Neo.LocationSearch.Tests.Indexes
{
    public class GeoIndexesTests
    {
        [Test]
        public void TestCreateFromGeoIndexesData()
        {
            var data = new GeoIndexesData
            {
                LatitudeInterval = 1.0,
                LongitudeInterval = 2.0,
                LatitudeIntervalCount = 1,
                LongitudeIntervalCount = 2,
                SouthWest = new GeoPoint(3, 4)
            };

            var indexes = new GeoIndexes(data);

            Assert.AreEqual(JsonConvert.SerializeObject(data), JsonConvert.SerializeObject(indexes.Dump()));
        }
    }
}