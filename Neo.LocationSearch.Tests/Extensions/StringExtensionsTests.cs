using Neo.LocationSearch.Extensions;
using NUnit.Framework;

namespace Neo.LocationSearch.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [TestCase("a", "bab", 2)]
        [TestCase("aaaa", "aabaa", 1)]
        [TestCase("aaaa", "aaaa", 0)]
        [TestCase("aa", "aab", 1)]
        [TestCase("aa", "baa", 1)]
        public void TestEditDistance(string first, string second, int distance)
        {
            var actual = first.EditDistance(second);

            Assert.AreEqual(distance, actual);
        }
    }
}