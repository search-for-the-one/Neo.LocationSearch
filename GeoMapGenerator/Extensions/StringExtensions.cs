using System;

namespace GeoMapGenerator.Extensions
{
    public static class StringExtensions
    {
        public static int EditDistance(this string first, string second, bool ignoreCase = false)
        {
            if (string.IsNullOrEmpty(first) && string.IsNullOrEmpty(second))
                return 0;

            if (string.IsNullOrEmpty(first))
                return second.Length;

            if (string.IsNullOrEmpty(second))
                return first.Length;

            if (ignoreCase)
            {
                first = first.ToLowerInvariant();
                second = second.ToLowerInvariant();
            }

            var cur = new int[second.Length + 1];
            for (var i = 0; i <= second.Length; i++)
            {
                cur[i] = i;
            }

            var next = new int[second.Length + 1];

            foreach (var ch in first)
            {
                next[0] = cur[0] + 1;
                for (var j = 1; j <= second.Length; j++)
                {
                    next[j] = Math.Min(Math.Min(cur[j] + 1, next[j - 1] + 1), cur[j - 1] + (ch == second[j - 1] ? 0 : 1));
                }

                var temp = cur;
                cur = next;
                next = temp;
            }

            return cur[second.Length];
        }
    }
}