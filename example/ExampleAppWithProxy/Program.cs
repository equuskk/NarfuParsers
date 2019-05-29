using System;
using System.Threading.Tasks;
using Flurl.Http;
using NarfuParsers;

namespace ExampleAppWithProxy
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            ConfigureProxy("https://*.*.*.*:3128");
            var timeout = TimeSpan.FromSeconds(5);

            var scheduleService = new ScheduleService(timeout);
            var lessons = await scheduleService.Students.GetLessons(9092);

            Console.WriteLine("Hello World!");
        }

        private static void ConfigureProxy(string url)
        {
            FlurlHttp.Configure(settings => {
                settings.HttpClientFactory = new ProxyHttpClientFactory(url);
            });
        }
    }
}