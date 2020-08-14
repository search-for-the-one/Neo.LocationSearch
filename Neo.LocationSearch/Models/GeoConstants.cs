using System;

namespace Neo.LocationSearch.Models
{
    internal static class GeoConstants
    {
        private const double EarthRadiusInMetres = 6376500.0;
        public const double ToRadiansCoefficient = Math.PI / 180;
        public const double Epsilon = 1E-25;
        private static readonly Distance EarthRadius = Distance.FromMetres(EarthRadiusInMetres);
        public static readonly Distance EarthDiameter = EarthRadius * 2;
    }
}