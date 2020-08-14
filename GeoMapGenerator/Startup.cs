using Microsoft.Extensions.DependencyInjection;
using Neo.ConsoleApp.DependencyInjection;
using Neo.Extensions.DependencyInjection;
using Neo.LocationSearch.BoundaryDataProviders;
using Neo.LocationSearch.BoundaryDataProviders.AustralianOpenData;
using Neo.LocationSearch.BoundaryDataProviders.AustralianOpenData.Options;
using Neo.LocationSearch.Indexes;
using Neo.LocationSearch.Indexes.Options;
using Neo.LocationSearch.LocationDataProviders;
using Neo.LocationSearch.LocationDataProviders.Options;
using Neo.LocationSearch.Options;

namespace GeoMapGenerator
{
    public class Startup : ConsoleAppStartup<int>
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddConfig<FactoryOptions>(Configuration);

            services.AddSingleton<IGeoMapBuilder, GeoMapBuilder>();
            services.AddConfig<GeoMapBuilderOptions>(Configuration);

            services.AddSingletonFromFactory<IBoundaryDataProvider>(s =>
                s.AddService<EmptyBoundaryDataProvider>()
                    .AddService<OpenDataBoundaryDataProvider>()
                    .WithOption<FactoryOptions>(f => f.BoundaryDataProvider));

            services.AddSingleton<IGeoBoundaryConverter, GeoBoundaryConverter>();
            services.AddSingleton<IPolygonElementSelector, PolygonElementSelector>();

            services.AddConfig<DefaultSuburbConflatorOptions>(Configuration);
            services.AddSingleton<ISuburbConflator, DefaultSuburbConflator>();

            services.AddConfig<LocalLocationDataProviderOptions>(Configuration);
            services.AddSingletonFromFactory<ILocationDataProvider>(s =>
                s.AddService<EmptyLocationDataProvider>()
                    .AddService<LocalLocationDataProvider>()
                    .WithOption<FactoryOptions>(f => f.LocationDataProvider));

            services.AddConfig<LocalOpenDataStoreOptions>(Configuration);
            services.AddConfig<OpenDataStoreOptions>(Configuration);
            services.AddSingletonFromFactory<IOpenDataStore>(s =>
                s.AddService<LocalOpenDataStore>()
                    .AddService<OpenDataStore>()
                    .WithOption<FactoryOptions>(f => f.OpenDataStore));

            services.AddConfig<GeoMapDumpHandlerOptions>(Configuration);
            services.AddSingleton<IDataDumpHandler, DataDumpHandler>();

            services.AddSingleton<IGeoMapConstructor, GeoMapConstructor>();
        }
    }
}