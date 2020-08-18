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
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Postcode == other.Postcode && State == other.State && Region == other.Region;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Suburb) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Postcode, State, Region);
        }
        
        public override string ToString()
        {
            return string.Join('\t', State, Region, Name, Postcode);
        }
    }
}