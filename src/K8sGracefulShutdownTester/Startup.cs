using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace K8sGracefulShutdownTester
{
    public enum State
    {
        Running,
        AfterSigterm
    }

    public class Startup
    {
        private State state = State.Running;

        private void Log(string msg) => Console.WriteLine($"{DateTime.UtcNow}: {msg}");

        public void ConfigureServices(IServiceCollection services)
        {
        }

        private async Task SleepAndPrintForSeconds(int seconds)
        {
            do
            {
                Log($"Sleeping ({seconds} seconds left)");
                await Task.Delay(1000);
            } while (--seconds > 0);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            Log($"Application starting. Process ID: {Process.GetCurrentProcess().Id}");
            appLifetime.ApplicationStopping.Register(ApplicationStopping);
            appLifetime.ApplicationStopped.Register(ApplicationStopped);


            app.Run(async (context) =>
            {
                var message = $"Host: {Environment.MachineName}, State: {state}";

                Log($"Incoming request at {context.Request.Path}, {message}");

                if (context.Request.Path.Value.Contains("slow"))
                {
                    await SleepAndPrintForSeconds(10);
                }
                else
                {
                    await Task.Delay(100);
                }

                await context.Response.WriteAsync(message);
            });
        }

        private void ApplicationStopping()
        {
            state = State.AfterSigterm;
            Log("ApplicationStopping called");
            // Log("ApplicationStopping called, sleeping for 10s");
            // Thread.Sleep(10000);
            // Log("ApplicationStopping 10s sleep done");
        }

        private void ApplicationStopped()
        {
            Log("ApplicationStopped called");
        }
    }
}
