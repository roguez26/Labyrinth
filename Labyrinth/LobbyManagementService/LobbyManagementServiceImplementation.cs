using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace LobbyManagementService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class LobbyManagementServiceImplementation : ILobbyManagementService
    {
        private Dictionary<string, List<ILobbyManagementCallback>> lobbies = new Dictionary<string, List<ILobbyManagementCallback>>();
        public void createLobby()
        {
            ILobbyManagementCallback callback = OperationContext.Current.GetCallbackChannel<ILobbyManagementCallback>();
            string lobbyCode = generateLobbyCode();

            while (lobbies.ContainsKey(lobbyCode))
            {
                lobbyCode = generateLobbyCode();
            }

            lobbies[lobbyCode] = new List<ILobbyManagementCallback>();
            lobbies[lobbyCode].Add(callback);
            Console.WriteLine(lobbyCode);
            callback.BroadcastCreated(lobbyCode);
        }

        public void joinToGame(string lobbyCode, string userName)
        {
            ILobbyManagementCallback callback = OperationContext.Current.GetCallbackChannel<ILobbyManagementCallback>();

            if (lobbies.ContainsKey(lobbyCode))
            {
                lobbies[lobbyCode].Add (callback);
                List<ILobbyManagementCallback> lobbyMembers = lobbies[lobbyCode];

                foreach (var member in lobbyMembers)
                {
                    member.BroadcastJoined(userName);
                }
            }
        }

        public static string generateLobbyCode()
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < 3; i++)
            {
                if (i > 0)
                {
                    stringBuilder.Append('-');
                }

                for (int j = 0; j < 3; j++)
                {
                    stringBuilder.Append(chars[random.Next(chars.Length)]);
                }
            }

            return stringBuilder.ToString();
        }

    }
}
