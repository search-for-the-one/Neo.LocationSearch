using Microsoft.Extensions.DependencyInjection;
using Neo.ConsoleApp.DependencyInjection;
using Neo.Extensions.DependencyInjection;
using Neo.LocationSearch.Boundaries;
using Neo.LocationSearch.Boundaries.AustralianOpenData;
using Neo.LocationSearch.Boundaries.AustralianOpenData.Options;
using Neo.LocationSearch.Boundaries.Options;
using Neo.LocationSearch.Indexes;
using Neo.LocationSearch.Indexes.Options;
using Neo.LocationSearch.Locations;
using Neo.LocationSearch.Locations.Options;
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

            AddBoundary(services);
            AddLocation(services);
            
            services.AddConfig<GeoMapDumpHandlerOptions>(Configuration);
            services.AddSingleton<IDataDumpHandler, DataDumpHandler>();
            services.AddSingleton<IGeoMapConstructor, GeoMapConstructor>();
        }

        private void AddBoundary(IServiceCollection services)
        {
            services.AddSingletonFromFactory<IBoundaryDataProvider>(s =>
                s.AddService<EmptyBoundaryDataProvider>()
                    .AddService<OpenDataBoundaryDataProvider>()
                    .WithOption<FactoryOptions>(f => f.BoundaryDataProvider));

            services.AddConfig<BoundaryDataPopulatorOptions>(Configuration);
            services.AddSingleton<IGeoBoundaryConverter, GeoBoundaryConverter>();
            services.AddSingleton<IPolygonElementSelector, PolygonElementSelector>();
            services.AddSingleton<IDataPopulator, BoundaryDataPopulator>();
            
            services.AddConfig<LocalOpenDataStoreOptions>(Configuration);
            services.AddConfig<OpenDataStoreOptions>(Configuration);
            services.AddSingletonFromFactory<IOpenDataStore>(s =>
                s.AddService<LocalOpenDataStore>()
                    .AddService<OpenDataStore>()
                    .WithOption<FactoryOptions>(f => f.OpenDataStore));
        }

        private void AddLocation(IServiceCollection services)
        {
            services.AddConfig<LocationDataPopulatorOptions>(Configuration);
            services.AddSingleton<IDataPopulator, LocationDataPopulator>();

            services.AddConfig<LocalLocationDataProviderOptions>(Configuration);
            services.AddSingletonFromFactory<ILocationDataProvider>(s =>
                s.AddService<EmptyLocationDataProvider>()
                    .AddService<LocalLocationDataProvider>()
                    .WithOption<FactoryOptions>(f => f.LocationDataProvider));
        }
    }
}