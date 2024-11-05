using ChatService;
using CatalogManagementService;
using UserManagementService;
using System;
using System.ServiceModel;

namespace Host
{
    public class Program
    {
        private static ServiceHost _chatHost;
        private static ServiceHost _userManagementHost;
        private static ServiceHost _catalogManagementHost;

        public static void StartHost()
        {
            _chatHost = new ServiceHost(typeof(ChatService.ChatServiceImplementation));
            _userManagementHost = new ServiceHost(typeof(UserManagementService.UserManagementServiceImplementation));
            _catalogManagementHost = new ServiceHost(typeof(CatalogManagementService.CatalogManagementServiceImplementation));

            _chatHost.Open();
            Console.WriteLine("chat service is running");

            _userManagementHost.Open();
            Console.WriteLine("user management service is running");

            _catalogManagementHost.Open();
            Console.WriteLine("catalog management service is running");
        }

        public static void StopHost()
        {
            if (_chatHost != null)
            {
                _chatHost.Close();
            }

            if (_userManagementHost != null)
            {
                _userManagementHost.Close();
            }

            if (_catalogManagementHost != null)
            {
                _catalogManagementHost.Close();
            }
        }

        static void Main(string[] args)
        {
            StartHost();
            Console.ReadLine();
            StopHost();
        }

    }
}
