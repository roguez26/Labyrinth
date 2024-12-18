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
            string lobbyCode;
            ILobbyManagementCallback callback = OperationContext.Current.GetCallbackChannel<ILobbyManagementCallback>();

            if (lobbyCreator == null || callback == null)
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailJoinToLobbyError"));
            } 
            lobbyCode = GenerateLobbyCode();
            
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
            ILobbyManagementCallback callback = OperationContext.Current.GetCallbackChannel<ILobbyManagementCallback>();

            if (!string.IsNullOrEmpty(lobbyCode) && user != null && callback != null) 
            {
                Dictionary<ILobbyManagementCallback, TransferUser> lobbyMembers = new Dictionary<ILobbyManagementCallback, TransferUser>();

                lock (_lobbies)
                {
                    if (_lobbies.TryGetValue(lobbyCode, out lobbyMembers))
                    {
                        lobbyMembers[callback] = user;
                        TransferUser[] members = lobbyMembers.Values.ToArray();

                        foreach (var member in lobbyMembers)
                        {
                            member.Key.NotifyUserHasJoined(user);
                            member.Key.GestMembersList(members);
                        }
                    }
                }
            }
        }

        public void RemoveUserFromLobby(string lobbyCode, TransferUser user)
        {
            if (!string.IsNullOrEmpty(lobbyCode) && user != null)
            {
                if (_lobbies.ContainsKey(lobbyCode))
                {
                    var lobby = _lobbies[lobbyCode];
                    var memberForRemove = lobby.FirstOrDefault(member => member.Value.Username == user.Username).Key;

                    if (memberForRemove != null)
                    {
                        lobby.Remove(memberForRemove);
                        if (lobby.Count > 0)
                        {
                            var members = lobby.Values.ToArray();

                            foreach (var callback in lobby.Keys)
                            {
                                callback.NotifyUserHasLeft(user);
                                callback.GestMembersList(members);
                            }
                        }
                        else
                        {
                            _lobbies.Remove(lobbyCode);
                        }
                        memberForRemove.KickOutPlayer(user);
                    }
                }
            }
        }

        private string GenerateLobbyCode()
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
