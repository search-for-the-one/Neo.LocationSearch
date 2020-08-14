namespace Neo.LocationSearch.Indexes.Models
{
    internal class GeoIndexRange
    {
        private const char Separator = ',';

        public GeoIndexRange(int x, int l, int r)
        {
            X = x;
            L = l;
            R = r;
        }

        public GeoIndexRange(string value)
        {
            var arr = value.Split(Separator);
            X = int.Parse(arr[0]);
            L = int.Parse(arr[1]);
            R = int.Parse(arr[2]);
        }

        public int X { get; set; }
        public int L { get; set; }
        public int R { get; set; }

        public override string ToString()
        {
            return string.Join(Separator, X, L, R);
        }
    }
}