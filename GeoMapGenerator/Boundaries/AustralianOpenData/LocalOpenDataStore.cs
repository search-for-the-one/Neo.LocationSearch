using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GeoMapGenerator.Boundaries.AustralianOpenData.Models;
using GeoMapGenerator.Boundaries.AustralianOpenData.Options;
using Newtonsoft.Json;

namespace GeoMapGenerator.Boundaries.AustralianOpenData
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
                    var date = feature.Properties.GetValueOrDefault(dataSource.UpdateTimePropertyName);
                    yield return new OpenDataSuburb
                    {
                        Id = feature.Properties.GetValueOrDefault(options.IdPropertyName),
                        State = dataSource.State,
                        Name = feature.Properties.GetValueOrDefault(dataSource.SuburbPropertyName),
                        UpdateDate = date != null ? DateTime.Parse(date) : DateTime.UnixEpoch,
                        Boundary = feature.Geometry
                    };
                }
            }
        }
    }
}