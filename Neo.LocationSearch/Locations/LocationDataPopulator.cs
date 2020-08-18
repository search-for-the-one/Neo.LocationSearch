using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Neo.LocationSearch.Extensions;
using Neo.LocationSearch.Indexes;
using Neo.LocationSearch.Locations.Models;
using Neo.LocationSearch.Locations.Options;
using Neo.LocationSearch.Models;

namespace Neo.LocationSearch.Locations
{
    internal class LocationDataPopulator : IDataPopulator
    {
        private readonly ILocationDataProvider locationDataProvider;
        private readonly LocationDataPopulatorOptions options;
        private readonly List<SuburbLocation> locationApiOnlySuburbs = new List<SuburbLocation>();
        private readonly List<SuburbLocation> incorrectStateSuburbs = new List<SuburbLocation>();
        private readonly List<(SuburbLocation location, List<Suburb> suburbs)> unmatchedSuburbs = new List<(SuburbLocation location, List<Suburb> suburbs)>();

        public LocationDataPopulator(ILocationDataProvider locationDataProvider, LocationDataPopulatorOptions options)
        {
            this.locationDataProvider = locationDataProvider;
            this.options = options;
        }

        public async ValueTask Populate(IGeoMapIndex map)
        {
            var suburbLocations = await locationDataProvider.Get();
            foreach (var suburbLocation in suburbLocations)
            {
                Conflate(map, suburbLocation);
            }

            OutputInLocationApiOnlySuburbs();
            OutputIncorrectStateSuburbs();
            OutputUnmatchedSuburbs();
        }

        private void OutputInLocationApiOnlySuburbs()
        {
            using var sw = new StreamWriter("in-location-api-only.txt");
            foreach (var location in locationApiOnlySuburbs)
            {
                sw.WriteLine(location);
            }
        }

        private void OutputIncorrectStateSuburbs()
        {
            using var sw = new StreamWriter("incorrect-state.txt");
            foreach (var location in incorrectStateSuburbs)
            {
                sw.WriteLine(location);
            }
        }

        private void OutputUnmatchedSuburbs()
        {
            using var sw = new StreamWriter("unmatched.txt");
            foreach (var (location, suburbs) in unmatchedSuburbs)
            {
                sw.WriteLine($"{location}{Common.Separator}{string.Join($"{Common.Separator}", suburbs)}");
            }
        }

        private void Conflate(IGeoMap map, SuburbLocation location)
        {
            var suburbs = map[location.GeoLocation] ??= new List<Suburb>();
            
            if (suburbs.Count == 0)
            {
                locationApiOnlySuburbs.Add(location);
                if (!options.SkipUncoveredArea) 
                    suburbs.Add(location.Suburb);
            }
            else
            {
                var sameSuburb = GetSameSuburb(suburbs, location.Suburb);
                if (sameSuburb != null)
                {
                    sameSuburb.Postcode ??= location.Suburb.Postcode;
                    sameSuburb.Name = location.Suburb.Name;
                    if (sameSuburb.State == location.Suburb.State)
                    {
                        sameSuburb.Region = location.Suburb.Region;
                    }
                    else
                    {
                        incorrectStateSuburbs.Add(location);
                    }
                }
                else
                {
                    unmatchedSuburbs.Add((location, suburbs));
                    if (!options.SkipUnmatchedArea)
                        suburbs.Add(location.Suburb);
                }
            }
        }

        private static Suburb GetSameSuburb(IReadOnlyCollection<Suburb> existingSuburbs, Suburb suburb)
        {
            return existingSuburbs.FirstOrDefault(x => string.Equals(x.Name, suburb.Name, StringComparison.OrdinalIgnoreCase)) ??
                   existingSuburbs.FirstOrDefault(x => x.Name.EditDistance(suburb.Name, true) * 1.0 / Math.Max(x.Name.Length, suburb.Name.Length) <= 0.2);
        }
    }
}