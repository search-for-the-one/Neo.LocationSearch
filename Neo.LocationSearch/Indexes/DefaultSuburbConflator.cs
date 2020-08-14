using System;
using System.Collections.Generic;
using System.Linq;
using Neo.LocationSearch.Extensions;
using Neo.LocationSearch.Indexes.Options;
using Neo.LocationSearch.Models;

namespace Neo.LocationSearch.Indexes
{
    internal class DefaultSuburbConflator : ISuburbConflator
    {
        private readonly DefaultSuburbConflatorOptions options;

        public DefaultSuburbConflator(DefaultSuburbConflatorOptions options)
        {
            this.options = options;
        }

        public List<Suburb> Conflate(List<Suburb> existingSuburbs, Suburb suburb)
        {
            if (existingSuburbs == null)
            {
                Console.WriteLine("Area is uncovered for {0}", suburb);
                return options.SkipUncoveredArea ? null : new List<Suburb> {suburb};
            }

            var sameSuburb = GetSameSuburb(existingSuburbs, suburb);
            if (sameSuburb != null)
            {
                sameSuburb.Postcode ??= suburb.Postcode;
                sameSuburb.Name = suburb.Name;
                sameSuburb.Region = suburb.Region;
                return existingSuburbs;
            }

            Console.WriteLine("Missing suburb: {0} Existing suburbs: {1}", suburb, string.Join("\t", existingSuburbs));
            if (options.SkipUnmatchedArea)
                return existingSuburbs;

            existingSuburbs.Add(suburb);
            return existingSuburbs;
        }

        private static Suburb GetSameSuburb(IReadOnlyCollection<Suburb> existingSuburbs, Suburb suburb)
        {
            return existingSuburbs.FirstOrDefault(x => string.Equals(x.Name, suburb.Name, StringComparison.OrdinalIgnoreCase)) ??
                   existingSuburbs.FirstOrDefault(x => x.Name.EditDistance(suburb.Name, true) * 1.0 / Math.Max(x.Name.Length, suburb.Name.Length) <= 0.2);
        }
    }
}