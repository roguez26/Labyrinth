using ChatService;
using CatalogManagementService;
using UserManagementService;
using System;
using System.ServiceModel;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost chatHost = new ServiceHost(typeof(ChatService.ChatServiceImplementation)))
            {
                using (ServiceHost userManagementHost = new ServiceHost(typeof(UserManagementService.UserManagementServiceImplementation)))
                {
                    using (ServiceHost catalogManagementHost = new ServiceHost(typeof(CatalogManagementService.CatalogManagementServiceImplementation)))
                    {
                        chatHost.Open();
                        Console.WriteLine("chat service is running");

                        userManagementHost.Open();
                        Console.WriteLine("user management service is running");

                        catalogManagementHost.Open();
                        Console.WriteLine("catalog management service is running");

                        Console.ReadLine();
                    }
                }
            }
        }

    }
}
