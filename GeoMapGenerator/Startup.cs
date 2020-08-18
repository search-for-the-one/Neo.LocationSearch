using GeoMapGenerator.Boundaries;
using GeoMapGenerator.Boundaries.AustralianOpenData;
using GeoMapGenerator.Boundaries.AustralianOpenData.Options;
using GeoMapGenerator.Boundaries.Options;
using GeoMapGenerator.Indexes;
using GeoMapGenerator.Indexes.Options;
using GeoMapGenerator.Locations;
using GeoMapGenerator.Locations.Options;
using GeoMapGenerator.Options;
using Microsoft.Extensions.DependencyInjection;
using Neo.ConsoleApp.DependencyInjection;
using Neo.Extensions.DependencyInjection;

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