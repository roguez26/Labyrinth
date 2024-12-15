using ChatService;
using CatalogManagementService;
using UserManagementService;
using LobbyManagementService;
using FriendsManagementService;
using System;
using System.ServiceModel;
using log4net;
using log4net.Config;

namespace Host
{
    public class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        private static ServiceHost _chatHost;
        private static ServiceHost _userManagementHost;
        private static ServiceHost _catalogManagementHost;
        private static ServiceHost _lobbyManagementHost;
        private static ServiceHost _friendsManagementHost;
        private static ServiceHost _menuManagementHost;
        private static ServiceHost _userProfilePictureManagementHost;

        public static void StartHost()
        {
            XmlConfigurator.Configure();
            string logDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            if (!System.IO.Directory.Exists(logDirectory))
            {
                System.IO.Directory.CreateDirectory(logDirectory);
            }

            try
            {
                _chatHost = new ServiceHost(typeof(ChatService.ChatServiceImplementation));
                _userManagementHost = new ServiceHost(typeof(UserManagementService.UserManagementServiceImplementation));
                _catalogManagementHost = new ServiceHost(typeof(CatalogManagementService.CatalogManagementServiceImplementation));
                _lobbyManagementHost = new ServiceHost(typeof(LobbyManagementService.LobbyManagementServiceImplementation));
                _friendsManagementHost = new ServiceHost(typeof(FriendsManagementService.FriendsManagementServiceImplementation));
                _menuManagementHost = new ServiceHost(typeof(MenuManagementService.MenuManagementServiceImplementation));
                _userProfilePictureManagementHost = new ServiceHost(typeof(UserManagementService.UserProfilePictureManagementServiceImplementation));

                _chatHost.Open();
                log.Info("Chat service is running");

                _userManagementHost.Open();
                log.Info("User management service is running");

                _catalogManagementHost.Open();
                log.Info("Catalog management service is running");

                _lobbyManagementHost.Open();
                log.Info("Lobby management service is running");

                _friendsManagementHost.Open();
                log.Info("Friends management service is running");

                _menuManagementHost.Open();
                log.Info("Menu management service is running");

                _userProfilePictureManagementHost.Open();
                log.Info("");
            }
            catch (Exception exception)
            {
                log.Error("Error al iniciar los servicios", exception);
                StopHost();
            }
        }

        public static void StopHost()
        {
            try
            {
                if (_chatHost != null)
                {
                    _chatHost.Close();
                    log.Info("Chat service stopped");
                }

                if (_userManagementHost != null)
                {
                    _userManagementHost.Close();
                    log.Info("User management service stopped");
                }

                if (_userProfilePictureManagementHost != null)
                {
                    _userProfilePictureManagementHost.Close();
                    log.Info("");
                }

                if (_catalogManagementHost != null)
                {
                    _catalogManagementHost.Close();
                    log.Info("Catalog management service stopped");
                }

                if (_lobbyManagementHost != null)
                {
                    _lobbyManagementHost.Close();
                    log.Info("Lobby management service stopped");
                }

                if (_friendsManagementHost != null)
                {
                    _friendsManagementHost.Close();
                    log.Info("Friends management service stopped");
                }
                
                if ( _menuManagementHost != null)
                {
                    _menuManagementHost.Close();
                    log.Info("Menu management service stopped");
                }
            }
            catch (Exception ex)
            {
                log.Error("Error al detener los servicios", ex);
            }
        }

        static void Main(string[] args)
        {
            StartHost();
            Console.WriteLine("Press Enter to stop services...");
            Console.ReadLine();
            StopHost();
        }

    }
}
