using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Exercise1
{
    class Program
    {
        private static readonly IEnumerable<string> Urls = new List<string>()
            {"https://www.plexure.com", "https://www.plexure.com", "https://www.plexure.com"};

        static void Main(string[] args)
        {
            Console.WriteLine("Press 'C' to terminate the application...\n");

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            var mainThread = new Thread(() => { MainAsync(cancellationToken).GetAwaiter().GetResult(); });
            var cancelThread = new Thread(() =>
            {
                if (Console.ReadKey(true).KeyChar.ToString().ToUpperInvariant() == "C")
                    cts.Cancel();
            });

            mainThread.Start();
            cancelThread.Start();
            cancelThread.Join();
        }

        static async Task MainAsync(CancellationToken cancellationToken)
        {
            try
            {
                var httpClient = new MyHttpClient();
                var aggregatedContentLength = await httpClient.GetAggregatedContentLength(Urls, cancellationToken);
                Console.WriteLine("Aggregated Content Length: " + aggregatedContentLength);
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("cancelled");
            }
        }
    }

    class MyHttpClient : HttpClient
    {
        public async Task<long> GetAggregatedContentLength(IEnumerable<string> urls,
            CancellationToken cancellationToken)
        {
            var tasks = urls.Select(url => GetContentLength(url, cancellationToken)).ToList();
            await Task.WhenAll(tasks);
            return tasks.Sum(t => t.Result);
        }

        private async Task<long> GetContentLength(string url, CancellationToken cancellationToken)
        {
            var response = await GetAsync(url, cancellationToken);
            return response.Content.Headers.ContentLength ?? 0;
        }
    }
}