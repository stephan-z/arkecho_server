using ArkEcho.Server;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;

namespace ArkEcho.BlazorPage
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            using (ArkEchoServer.Instance)
            {
                // TODO: Server vorher instanzieren, host raus -> konfigurierbarer Port
                // Start the WebHost, Server and Controllers
                // Set WebRoot for Static Files (css etc.), use xcopy for output
                using IWebHost host = WebHost.CreateDefaultBuilder(args)
                                .UseUrls("https://*:5001")
                                .UseKestrel()
                                .UseStartup<Startup>()
                                //.UseWebRoot($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\wwwroot")
                                .Build();

                if (host == null)
                    return;

                if (!ArkEchoServer.Instance.Init(host))
                {
                    Console.WriteLine("Problem on Initializing the ArkEcho.Server!");
                    return;
                }
                else
                {
                    host.Run(); // Starts Event Cycle and API

                    if (ArkEchoServer.Instance.RestartRequested)
                        System.Diagnostics.Process.Start("ArkEcho.Server.exe");
                }
            }

            Environment.Exit(0);
        }
    }
}
