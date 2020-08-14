using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Neo.LocationSearch.Indexes.Models;

namespace Neo.LocationSearch.Indexes
{
    internal class PolygonElementSelector : IPolygonElementSelector
    {
        public IEnumerable<Point> Elements(ICollection<PointPolygon> polygons, ICollection<PointPolygon> excludePolygons)
        {
            var width = polygons.SelectMany(x => x.Points).Concat(excludePolygons.SelectMany(x => x.Points)).Max(x => x.X) + 1;
            var height = polygons.SelectMany(x => x.Points).Concat(excludePolygons.SelectMany(x => x.Points)).Max(x => x.Y) + 1;

            using var bmp = new Bitmap(width, height);
            using var g = Graphics.FromImage(bmp);

            using var brush = new SolidBrush(Color.Black);
            foreach (var polygon in polygons)
            {
                g.FillPolygon(brush, polygon.Points, FillMode.Winding);
            }

            using var excludeBrush = new SolidBrush(Color.White);
            foreach (var excludePolygon in excludePolygons)
            {
                g.FillPolygon(excludeBrush, excludePolygon.Points, FillMode.Winding);
            }

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    if (bmp.GetPixel(i, j).ToArgb() == Color.Black.ToArgb())
                        yield return new Point(i, j);
                }
            }
        }
    }
}