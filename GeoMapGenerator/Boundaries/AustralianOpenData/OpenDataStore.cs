using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GeoMapGenerator.Boundaries.AustralianOpenData.Models;
using GeoMapGenerator.Boundaries.AustralianOpenData.Options;
using Newtonsoft.Json;

namespace GeoMapGenerator.Boundaries.AustralianOpenData
{
    internal class OpenDataStore : IOpenDataStore
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly OpenDataStoreOptions options;

        public OpenDataStore(OpenDataStoreOptions options, IHttpClientFactory httpClientFactory)
        {
            this.options = options;
            this.httpClientFactory = httpClientFactory;
        }

        public async ValueTask<IEnumerable<OpenDataSuburb>> Get()
        {
            var results = new List<OpenDataSuburb>();
            foreach (var dataSource in options.DataSources)
            {
                var collection = JsonConvert.DeserializeObject<FeatureCollection>(await Get(dataSource.Url));
                results.AddRange(collection.Features.Select(feature =>
                {
                    var date = feature.Properties.GetValueOrDefault(dataSource.UpdateTimePropertyName);
                    return new OpenDataSuburb
                    {
                        Id = feature.Properties.GetValueOrDefault(options.IdPropertyName),
                        State = dataSource.State,
                        Name = feature.Properties.GetValueOrDefault(dataSource.SuburbPropertyName),
                        UpdateDate = date != null ? DateTime.Parse(date) : DateTime.UnixEpoch,
                        Boundary = feature.Geometry
                    };
                }));
            }

            return results;
        }

        private async ValueTask<string> Get(string url)
        {
            var client = httpClientFactory.CreateClient();
            return await client.GetAsync(url).Result.Content.ReadAsStringAsync();
        }
    }
}