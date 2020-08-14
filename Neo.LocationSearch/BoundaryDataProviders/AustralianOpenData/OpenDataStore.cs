using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Neo.LocationSearch.BoundaryDataProviders.AustralianOpenData.Models;
using Neo.LocationSearch.BoundaryDataProviders.AustralianOpenData.Options;
using Newtonsoft.Json;

namespace Neo.LocationSearch.BoundaryDataProviders.AustralianOpenData
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
                results.AddRange(collection.Features.Select(feature => new OpenDataSuburb
                {
                    Id = feature.Properties.GetValueOrDefault(options.IdPropertyName),
                    State = dataSource.State,
                    Name = feature.Properties.GetValueOrDefault(dataSource.SuburbPropertyName),
                    UpdateDate = DateTime.Parse(feature.Properties.GetValueOrDefault(dataSource.UpdateTimePropertyName)),
                    Boundary = feature.Geometry
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