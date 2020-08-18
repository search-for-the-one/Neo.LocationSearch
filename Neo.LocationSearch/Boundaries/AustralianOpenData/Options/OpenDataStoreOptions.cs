using System.Collections.Generic;
using Neo.Extensions.DependencyInjection;

namespace Neo.LocationSearch.Boundaries.AustralianOpenData.Options
{
    public class OpenDataStoreOptions : IConfig
    {
        public string IdPropertyName { get; set; } = "loc_pid";
        public List<OpenDataSource> DataSources { get; set; } = new List<OpenDataSource>();
    }
}