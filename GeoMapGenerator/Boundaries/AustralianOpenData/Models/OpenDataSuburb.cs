using System;

namespace GeoMapGenerator.Boundaries.AustralianOpenData.Models
{
    internal class OpenDataSuburb
    {
        public string State { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public DateTime UpdateDate { get; set; }
        public Geometry Boundary { get; set; }

        public override string ToString()
        {
            return $"{State}\t{Name}";
        }
    }
}