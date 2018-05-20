using Nancy;
using Nancy.Hosting.Self;
using NancyServer2.DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NancyServer2
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionData.Server = @"DESKTOP-MQ6K8RV\SQLEXPRESS";
            ConnectionData.Database = "HobbyDatabase";
            ConnectionData.AuthenticationString = "Integrated Security = SSPI";

            var uri = new Uri("http://localhost:6666");

            HostConfiguration hostConfigs = new HostConfiguration()
            {
                UrlReservations = new UrlReservations() { CreateAutomatically = true }
            };

            using (NancyHost host = new NancyHost(uri, new DefaultNancyBootstrapper(), hostConfigs))
            {
                host.Start();

                Console.WriteLine("Your application is running on " + uri);
                Console.WriteLine("Press any [Enter] to close the host.");
                Console.ReadLine();

                host.Stop();
            }

        }
    }
}
