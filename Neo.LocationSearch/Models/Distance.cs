using System;
using System.Diagnostics;
using System.Globalization;

namespace Neo.LocationSearch.Models
{
    [DebuggerDisplay("{" + nameof(metres) + "} metres")]
    public struct Distance : IEquatable<Distance>, IComparable<Distance>
    {
        private readonly double metres;

        private Distance(double metres)
        {
            this.metres = metres;
        }

        public double AsMetres => metres;

        public double AsKilometres => metres / 1000;

        public static Distance FromMetres(double metres)
        {
            return new Distance(metres);
        }

        public static Distance FromKilometres(double kilometres)
        {
            return new Distance(kilometres * 1000);
        }

        public static bool operator >(Distance lhs, Distance rhs)
        {
            return lhs.metres > rhs.metres;
        }

        public static bool operator >=(Distance lhs, Distance rhs)
        {
            return lhs.metres >= rhs.metres;
        }

        public static bool operator <(Distance lhs, Distance rhs)
        {
            return lhs.metres < rhs.metres;
        }

        public static bool operator <=(Distance lhs, Distance rhs)
        {
            return lhs.metres <= rhs.metres;
        }

        public static Distance operator +(Distance lhs, Distance rhs)
        {
            return new Distance(lhs.metres + rhs.metres);
        }

        public static Distance operator -(Distance lhs, Distance rhs)
        {
            return new Distance(lhs.metres - rhs.metres);
        }

        public static Distance operator *(Distance lhs, double multipler)
        {
            return new Distance(lhs.metres * multipler);
        }

        public static double operator /(Distance lhs, Distance rhs)
        {
            return lhs.metres / rhs.metres;
        }

        public static Distance operator /(Distance lhs, double divisor)
        {
            return new Distance(lhs.metres / divisor);
        }

        public static bool operator ==(Distance lhs, Distance rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Distance lhs, Distance rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object rhs)
        {
            // can't use "as" operator on a struct
            return rhs is Distance distance && Equals(distance);
        }

        public override int GetHashCode()
        {
            return metres.GetHashCode();
        }

        public bool Equals(Distance rhs)
        {
            return metres.Equals(rhs.metres);
        }

        public int CompareTo(Distance rhs)
        {
            if (this < rhs)
                return -1;

            if (this > rhs)
                return 1;

            return 0;
        }

        public override string ToString()
        {
            return AsKilometres.ToString(CultureInfo.InvariantCulture);
        }

        public string ToString(string format)
        {
            if (string.IsNullOrWhiteSpace(format) || format.Equals("K"))
                return AsKilometres.ToString(CultureInfo.InvariantCulture);

            return AsMetres.ToString(CultureInfo.InvariantCulture); // format == "M"
        }
    }
}