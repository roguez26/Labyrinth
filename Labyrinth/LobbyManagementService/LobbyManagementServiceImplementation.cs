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
        private Dictionary<string, List<ILobbyManagementCallback>> _lobbies = new Dictionary<string, List<ILobbyManagementCallback>>();
        public void CreateLobby()
        {
            ILobbyManagementCallback callback = OperationContext.Current.GetCallbackChannel<ILobbyManagementCallback>();
            string lobbyCode = GenerateLobbyCode();

            while (_lobbies.ContainsKey(lobbyCode))
            {
                lobbyCode = GenerateLobbyCode();
            }

            _lobbies[lobbyCode] = new List<ILobbyManagementCallback>();
            _lobbies[lobbyCode].Add(callback);
            Console.WriteLine(lobbyCode);
            callback.BroadcastCreated(lobbyCode);
        }

        public void JoinToGame(string lobbyCode, string userName)
        {
            ILobbyManagementCallback callback = OperationContext.Current.GetCallbackChannel<ILobbyManagementCallback>();

            if (_lobbies.ContainsKey(lobbyCode))
            {
                _lobbies[lobbyCode].Add (callback);
                List<ILobbyManagementCallback> lobbyMembers = _lobbies[lobbyCode];

                foreach (var member in lobbyMembers)
                {
                    member.BroadcastJoined(userName);
                }
            }
        }

        public static string GenerateLobbyCode()
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
