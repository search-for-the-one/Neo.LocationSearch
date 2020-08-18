using System.Collections.Generic;

namespace Neo.LocationSearch.Boundaries.AustralianOpenData.Models
{
    internal class Feature
    {
        public Geometry Geometry { get; set; }
        public Dictionary<string, string> Properties { get; set; }
    }
}