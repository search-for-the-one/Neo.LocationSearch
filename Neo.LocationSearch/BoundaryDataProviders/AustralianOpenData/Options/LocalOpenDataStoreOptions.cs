using System.Collections.Generic;
using Neo.Extensions.DependencyInjection;

namespace Neo.LocationSearch.BoundaryDataProviders.AustralianOpenData.Options
{
    public class LocalOpenDataStoreOptions : IConfig
    {
        public string IdPropertyName { get; set; } = "loc_pid";
        public List<LocalOpenDataSource> DataSources { get; set; } = new List<LocalOpenDataSource>();
    }
}