using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParralellRequests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var tasks = new List<Task<string>>();
                var num = 50;
                Console.WriteLine($"Starting {num} Tasks");
                var opts = new ParallelOptions() {MaxDegreeOfParallelism = num};
                Parallel.For(1, num, opts, (i) =>
                {
                    OcIncrementTask();
                });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                throw;
            }
        }

        private static async void OcIncrementTask()
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri("http://slimapi.ocpi.lan:8080/api/counters/onecounter/value");
                var method = new HttpMethod("PATCH");
                var request = new HttpRequestMessage(method, uri);
                
                request.Content = new StringContent("{\"value\": \"+1\"}", Encoding.UTF8, "application/json");
                var responseTask = client.SendAsync(request);
                responseTask.Wait();
                var response =  await responseTask.Result.Content.ReadAsStringAsync();
                Console.WriteLine(response);
            }
        }

        private static async void CsIncrementTask()
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri("http://192.168.0.164:5008/Counter/Increment/44e42adf-805f-46dc-8cd0-5eea9b20bab9");
                var method = new HttpMethod("POST");
                var request = new HttpRequestMessage(method, uri);

                var responseTask = client.SendAsync(request);
                responseTask.Wait();
                var response = await responseTask.Result.Content.ReadAsStringAsync();
                Console.WriteLine(response);
            }
        }
    }
}
