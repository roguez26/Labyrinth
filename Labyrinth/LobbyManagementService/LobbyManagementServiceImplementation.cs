using TransferUser = LabyrinthCommon.TransferUser;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using LabyrinthCommon;
using log4net;
using System.Linq;

namespace LobbyManagementService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class LobbyManagementServiceImplementation : ILobbyManagementService
    {
        private Dictionary<string, Dictionary<ILobbyManagementCallback, TransferUser>> _lobbies = new Dictionary<string, Dictionary<ILobbyManagementCallback, TransferUser>>();
        private static readonly ILog _log = LogManager.GetLogger(typeof(LobbyManagementServiceImplementation));

        public string CreateLobby(TransferUser lobbyCreator)
        {
            ILobbyManagementCallback callback = OperationContext.Current.GetCallbackChannel<ILobbyManagementCallback>();
            string lobbyCode = GenerateLobbyCode();

            while (_lobbies.ContainsKey(lobbyCode))
            {
                lobbyCode = GenerateLobbyCode();
            }

            _lobbies[lobbyCode] = new Dictionary<ILobbyManagementCallback, TransferUser>();
            _lobbies[lobbyCode].Add(callback, lobbyCreator);

            return lobbyCode;
        }


        public void JoinToGame(string lobbyCode, TransferUser user)
        {
            try
            {
                ILobbyManagementCallback callback = OperationContext.Current.GetCallbackChannel<ILobbyManagementCallback>();

                Dictionary<ILobbyManagementCallback, TransferUser> lobbyMembers = null;

                if (callback != null && user != null)
                {
                    lock (_lobbies)
                    {
                        if (!_lobbies.TryGetValue(lobbyCode, out lobbyMembers) || lobbyMembers.ContainsKey(callback))
                        {
                            callback.GestMembersList(null);
                            return;
                        }
                        lobbyMembers[callback] = user;
                    }

                    TransferUser[] members = lobbyMembers.Values.ToArray();

                    foreach (var member in lobbyMembers)
                    {
                        member.Key.NotifyUserHasJoined(user);
                        member.Key.GestMembersList(members);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error($"Error in JoinToGame: {ex.Message}");
            }
        }

        public void RemoveUserFromLobby(string lobbyCode, TransferUser user)
        {
            if (_lobbies.ContainsKey(lobbyCode))
            {
                var lobby = _lobbies[lobbyCode];

                var callbackToRemove = lobby.FirstOrDefault(kv => kv.Value.Username == user.Username).Key;

                if (callbackToRemove != null)
                {
                    lobby.Remove(callbackToRemove);

                    var members = lobby.Values.ToArray();

                    foreach (var callback in lobby.Keys)
                    {
                        
                        callback.NotifyUserHasLeft(user);
                        callback.GestMembersList(members);
                    }
                    callbackToRemove.KickOutPlayer(user);
                }
            }
        }

        public string GenerateLobbyCode()
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
