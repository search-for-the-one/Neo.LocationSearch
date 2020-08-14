using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Neo.LocationSearch.LocationDataProviders.Models;
using Neo.LocationSearch.LocationDataProviders.Options;
using Newtonsoft.Json;

namespace Neo.LocationSearch.LocationDataProviders
{
    internal class LocalLocationDataProvider : ILocationDataProvider
    {
        private readonly LocalLocationDataProviderOptions options;

        public LocalLocationDataProvider(LocalLocationDataProviderOptions options)
        {
            this.options = options;
        }

        public ValueTask<IEnumerable<SuburbLocation>> Get()
        {
            return new ValueTask<IEnumerable<SuburbLocation>>(JsonConvert.DeserializeObject<IEnumerable<SuburbLocation>>(File.ReadAllText(options.File)));
        }
    }
}