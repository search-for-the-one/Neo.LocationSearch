using System.Threading.Tasks;

namespace GeoMapUsageExample
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            return await new Startup().RunAsync<GeoMapUsageExampleApp>();
        }
    }
}