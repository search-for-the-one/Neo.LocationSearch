using System;
using System.Runtime.CompilerServices;

namespace Neo.LocationSearch.Models
{
    public struct GeoPoint : IEquatable<GeoPoint>
    {
        public GeoPoint(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public override string ToString()
        {
            return $"{Latitude}{Common.Separator}{Longitude}";
        }

        public bool Equals(GeoPoint other)
        {
            return Math.Abs(Latitude - other.Latitude) < GeoConstants.Epsilon && Math.Abs(Longitude - other.Longitude) < GeoConstants.Epsilon;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Distance GetDistance(GeoPoint other)
        {
            return HaversineApproximationDistance(other);
        }

        private Distance HaversineApproximationDistance(GeoPoint other)
        {
            var latInRads = Latitude * GeoConstants.ToRadiansCoefficient;
            var otherLatInRads = other.Latitude * GeoConstants.ToRadiansCoefficient;
            var sinHalfLatDiff = SineApproximation((latInRads - otherLatInRads) / 2);
            var sinHalfLonDiff = SineApproximation((Longitude - other.Longitude) * GeoConstants.ToRadiansCoefficient / 2);
            var d3 = sinHalfLatDiff * sinHalfLatDiff + CosineApproximation(latInRads) * CosineApproximation(otherLatInRads) * sinHalfLonDiff * sinHalfLonDiff;

            return GeoConstants.EarthDiameter * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double SineApproximation(double x)
        {
            const double linearCoefficient = 4 / Math.PI;
            const double quadraticCoefficient = -4 / (Math.PI * Math.PI);
            const double trigPeriod = Math.PI * 2;
            const double negativeTrigPeriod = -Math.PI * 2;

            if (x > trigPeriod)
                x += negativeTrigPeriod;
            else if (x < negativeTrigPeriod)
                x += trigPeriod;

            double sin;
            //compute sine
            if (x < 0)
            {
                sin = linearCoefficient * x - quadraticCoefficient * x * x;
                sin = -.225 * (sin * sin + sin) + sin;
            }
            else
            {
                sin = linearCoefficient * x + quadraticCoefficient * x * x;
                sin = .225 * (sin * sin - sin) + sin;
            }

            return sin;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double CosineApproximation(double x)
        {
            const double shiftToSine = Math.PI / 2;
            return SineApproximation(x + shiftToSine);
        }
    }
}