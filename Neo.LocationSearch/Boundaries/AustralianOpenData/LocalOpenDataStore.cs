using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Neo.LocationSearch.Boundaries.AustralianOpenData.Models;
using Neo.LocationSearch.Boundaries.AustralianOpenData.Options;
using Newtonsoft.Json;

namespace Neo.LocationSearch.Boundaries.AustralianOpenData
{
    internal class LocalOpenDataStore : IOpenDataStore
    {
        private readonly LocalOpenDataStoreOptions options;

        public LocalOpenDataStore(LocalOpenDataStoreOptions options)
        {
            this.options = options;
        }

        public ValueTask<IEnumerable<OpenDataSuburb>> Get()
        {
            return new ValueTask<IEnumerable<OpenDataSuburb>>(GetSuburbs());
        }

        private IEnumerable<OpenDataSuburb> GetSuburbs()
        {
            foreach (var dataSource in options.DataSources)
            {
                var collection = JsonConvert.DeserializeObject<FeatureCollection>(File.ReadAllText(dataSource.File));
                foreach (var feature in collection.Features)
                {
                    yield return new OpenDataSuburb
                    {
                        Id = feature.Properties.GetValueOrDefault(options.IdPropertyName),
                        State = dataSource.State,
                        Name = feature.Properties.GetValueOrDefault(dataSource.SuburbPropertyName),
                        UpdateDate = DateTime.Parse(feature.Properties.GetValueOrDefault(dataSource.UpdateTimePropertyName)),
                        Boundary = feature.Geometry
                    };
                }
            }
        }
    }
}