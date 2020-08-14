using System;

namespace Neo.LocationSearch.Models
{
    public class Suburb : IEquatable<Suburb>
    {
        public string Name { get; set; }
        public string Postcode { get; set; }
        public string State { get; set; }
        public string Region { get; set; }

        public bool Equals(Suburb other)
        {
            return string.Equals(ToString(), other?.ToString());
        }

        public override string ToString()
        {
            return string.Join(Common.Separator, State, Region, Name, Postcode);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}