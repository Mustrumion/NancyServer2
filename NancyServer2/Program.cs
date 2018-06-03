using Nancy;
using Nancy.Hosting.Self;
using NancyServer2.DAOs;
using NancyServer2.Modules;
using NancyServer2.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NancyServer2
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionData.Server = @"DESKTOP-VN1ED9P\SQLEXPRESS";
            //ConnectionData.Server = @"DESKTOP-MQ6K8RV\SQLEXPRESS";
            //ConnectionData.Server = @"ZIUTEK-KOMPUTER\SQLEXPRESS";
            //ConnectionData.Database = "HobbyHunter";
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
                string command = null;
                while (command == null || command.ToLower() != "close")
                {
                    Console.WriteLine("Input 'deploy' to redeploy all tables (this will wipe all data and populate tables with example data).");
                    Console.WriteLine("Input 'close' to close the host.");
                    command = Console.ReadLine().ToLower();
                    switch(command)
                    {
                        case "deploy":
                            Deploy();
                            break;
                    }
                }

                host.Stop();
            }

        }

        private static void Deploy()
        {
            DeploymentDAO dao = new DeploymentDAO();
            dao.Redeploy();
            UserDAO registrator = new UserDAO();
            User user1 = new User()
            {
                Email = "user@user.user",
                Password = "useruser"
            };
            registrator.Register(user1);
            User user2 = new User()
            {
                Email = "user2@user.user",
                Password = "useruser"
            };
            registrator.Register(user2);
            UserProfileDAO profiler = new UserProfileDAO();
            user1 = profiler.GetUserByEmail(user1.Email);

            UserProfile profile = new UserProfile
            {
                UserID = user1.ID,
                Nick = "JhonnyJohnTheJonniest",
                Name = "John",
                Surname = "Johner",
                Born = new DateTime(2000, 1, 1),
                AgeVisible = true,
                NameVisible = true,
            };
            profiler.SaveUserProfile(profile);

            UserPhoto photo = new UserPhoto
            {
                UserID = user1.ID,
                PhotoBase64 = Convert.ToBase64String(File.ReadAllBytes(@"Resources\emoji.png")),
            };
            profiler.SaveUserProfilePhoto(photo);
            Console.WriteLine("Redeployment done");
        }
    }
}
