using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Neo.ConsoleApp.DependencyInjection;
using Neo.LocationSearch;
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
            var repeat = true;

            while (repeat)
            {
                try
                {
                    NearestSuburbs(map);
                    repeat = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return Task.FromResult(0);
        }

        private static void NearestSuburbs(IGeoMap map)
        {
            while (true)
            {
                Console.WriteLine("Input latitude, longitude and distance in kilometers:");
                var line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                    break;

                var arr = line.Split(new[] {' ', ','}, StringSplitOptions.RemoveEmptyEntries);
                var latitude = double.Parse(arr[0]);
                var longitude = double.Parse(arr[1]);
                var km = double.Parse(arr[2]);

                var sw = Stopwatch.StartNew();
                var suburbs = map.GetNearestSuburbs(new GeoPoint(latitude, longitude), Distance.FromKilometres(km)).ToList();
                sw.Stop();
                
                foreach (var suburb in suburbs.OrderBy(x => x.Name))
                {
                    Console.WriteLine(suburb);
                }
                Console.WriteLine($"Time taken: {sw.Elapsed.TotalMilliseconds:F3}ms");
                PrintMemoryUsage();
            }
        }

        private IGeoMap CreateFromDataFile()
        {
            Console.WriteLine("Input '1' to load from json file, '2' to load from binary file");
            var option = Console.ReadLine();
            Console.WriteLine("Input file name");
            var file = Console.ReadLine();

            PrintMemoryUsage();

            var sw = Stopwatch.StartNew();
            var result = option == "1" ? geoMapConstructor.FromJsonFile(file) : geoMapConstructor.FromBinaryFile(file);
            sw.Stop();
            Console.WriteLine($"Loaded geo-map data in {sw.Elapsed.TotalMilliseconds:F3}ms");
            
            ForceGc();
            PrintMemoryUsage();

            return result;
        }

        private static void ForceGc()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
        }

        private static void PrintMemoryUsage()
        {
            Console.WriteLine($"GC memory size: {GC.GetTotalMemory(false) / (1024.0 * 1024.0):F1}MB");
        }
    }
}