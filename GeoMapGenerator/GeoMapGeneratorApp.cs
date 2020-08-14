using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Neo.ConsoleApp.DependencyInjection;
using Neo.LocationSearch.Indexes;
using Neo.LocationSearch.Models;

namespace GeoMapGenerator
{
    internal class GeoMapGeneratorApp : IConsoleApp<int>
    {
        private readonly IDataDumpHandler dataDumpHandler;
        private readonly IGeoMapBuilder geoMapBuilder;

        public GeoMapGeneratorApp(IGeoMapBuilder geoMapBuilder, IDataDumpHandler dataDumpHandler)
        {
            this.geoMapBuilder = geoMapBuilder;
            this.dataDumpHandler = dataDumpHandler;
        }

        public async Task<int> Run()
        {
            if (!Directory.Exists("open-data"))
                ZipFile.ExtractToDirectory("open-data.zip", "open-data");
            
            var map = await geoMapBuilder.Build();
            dataDumpHandler.DumpToFile(map);

            NearBySuburbs(map);

            return 0;
        }

        private static void NearBySuburbs(IGeoMap map)
        {
            Console.WriteLine("Input latitude, longitude and distance in kilometers:");
            while (true)
            {
                var line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                    break;

                var arr = line.Split(new[] {' ', ','}, StringSplitOptions.RemoveEmptyEntries);
                var latitude = double.Parse(arr[0]);
                var longitude = double.Parse(arr[1]);
                var km = double.Parse(arr[2]);

                var suburbs = map.NearbySuburbs(new GeoPoint(latitude, longitude), Distance.FromKilometres(km)).OrderBy(x => x.Name);
                foreach (var suburb in suburbs)
                {
                    Console.WriteLine(suburb);
                }
            }
        }
    }
}