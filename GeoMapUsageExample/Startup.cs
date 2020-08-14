using Microsoft.Extensions.DependencyInjection;
using Neo.ConsoleApp.DependencyInjection;
using Neo.LocationSearch.Indexes;

namespace GeoMapUsageExample
{
    public class Startup : ConsoleAppStartup<int>
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGeoMapConstructor, GeoMapConstructor>();
        }
    }
}