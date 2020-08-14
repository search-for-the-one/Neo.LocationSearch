using System;
using System.Linq;
using System.Threading.Tasks;
using Neo.ConsoleApp.DependencyInjection;
using Neo.LocationSearch.Indexes;
using Neo.LocationSearch.Models;

namespace GeoMapUsageExample
{
    internal class GeoMapUsageExampleApp : IConsoleApp<int>
    {
        private readonly IGeoMapConstructor geoMapConstructor;

        public GeoMapUsageExampleApp(IGeoMapConstructor geoMapConstructor)
        {
            this.geoMapConstructor = geoMapConstructor;
        }

        public Task<int> Run()
        {
            var map = CreateFromDataFile();

            NearBySuburbs(map);

            return new Task<int>(() => 0);
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

        private IGeoMap CreateFromDataFile()
        {
            Console.WriteLine("Input 1 from json file, 2 from binary file");
            var option = Console.ReadLine();
            Console.WriteLine("Input file name");
            var file = Console.ReadLine();

            return option == "1" ? geoMapConstructor.FromJsonFile(file) : geoMapConstructor.FromBinaryFile(file);
        }
    }
}