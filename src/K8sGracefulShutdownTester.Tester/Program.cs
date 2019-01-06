using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace K8sGracefulShutdownTester.Tester
{
    class Program
    {
        const string serviceUrl = "http://35.241.131.163/";
        //const string serviceUrl = "http://localhost:5000/";

        static void Main(string[] args)
        {
            Test().Wait();
            Console.WriteLine("Hello World!");
        }

        static async Task Test()
        {
            while (true)
            {
                using (var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(2) })
                {
                    try
                    {
                        var resp = await httpClient.GetAsync(serviceUrl);

                        if (resp.IsSuccessStatusCode)
                        {
                            Console.WriteLine("{0}: Successful request", DateTime.UtcNow);
                        }
                        else
                        {
                            Console.WriteLine("{0}: Error! StatusCode: {1}, Response: {2}", DateTime.UtcNow, resp.StatusCode, await resp.Content.ReadAsStringAsync());
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("{0}: Error! Exception: {1}", DateTime.UtcNow, ex.Message);
                    }
                }
            }
        }
    }
}
