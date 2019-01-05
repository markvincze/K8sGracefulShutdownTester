using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace K8sGracefulShutdownTester.Tester
{
    class Program
    {
        // const string serviceUrl = "http://35.241.131.163/";
        const string serviceUrl = "http://localhost:5000/";

        static void Main(string[] args)
        {
            Test().Wait();
            Console.WriteLine("Hello World!");
        }

        static async Task Test()
        {
            using (var httpClient = new HttpClient())
            {
                while (true)
                {
                    try
                    {
                        var resp = await httpClient.GetAsync(serviceUrl);

                        if (!resp.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Error! StatusCode: {0}, Response: {1}", resp.StatusCode, await resp.Content.ReadAsStringAsync());
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error! Exception: {0}", ex.Message);
                    }
                }
            }
        }
    }
}
