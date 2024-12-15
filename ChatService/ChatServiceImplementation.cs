using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using log4net;
using TransferUser = LabyrinthCommon.TransferUser;

namespace ChatService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class ChatServiceImplementation : IChatService
    {
        private readonly Dictionary<string, Dictionary<IChatServiceCallback, TransferUser>> _lobbies = new Dictionary<string, Dictionary<IChatServiceCallback, TransferUser>>();
        private static readonly ILog _log = LogManager.GetLogger(typeof(ChatServiceImplementation));

        public void Start(string lobbyCode, TransferUser lobbyCreator)
        {
            IChatServiceCallback callback = OperationContext.Current.GetCallbackChannel<IChatServiceCallback>();
            if (callback == null || lobbyCreator == null || string.IsNullOrEmpty(lobbyCode))
            {
                return;
            }
           
            lock (_lobbies)
            {
                if (!_lobbies.ContainsKey(lobbyCode))
                {
                    _lobbies[lobbyCode] = new Dictionary<IChatServiceCallback, TransferUser>
                    {
                        { callback, lobbyCreator }
                    };
                }
            }
        }

        public void JoinToChat(string lobbyCode, TransferUser user)
        {
            IChatServiceCallback callback = OperationContext.Current.GetCallbackChannel<IChatServiceCallback>();
            if (callback == null || user == null || string.IsNullOrEmpty(lobbyCode))
            {
                return;
            }
         
            lock (_lobbies)
            {
                if (_lobbies.TryGetValue(lobbyCode, out var lobbyMembers) && !lobbyMembers.ContainsKey(callback))
                {
                    lobbyMembers[callback] = user;
                }
            }
        }

        public void SendMessage(string message, string lobbyCode)
        {
            if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(lobbyCode))
            {
                return;
            }

            lock (_lobbies)
            {
                if (_lobbies.TryGetValue(lobbyCode, out var lobbyMembers))
                {
                    foreach (var member in lobbyMembers)
                    {
                        member.Key.BroadcastMessage(message);
                    }
                }
            }
        }

        public void RemoveUserFromChat(string lobbyCode, TransferUser user)
        {
            if (string.IsNullOrEmpty(lobbyCode) || user == null)
            {
                return;
            }

            lock (_lobbies)
            {
                if (_lobbies.TryGetValue(lobbyCode, out var lobby))
                {
                    var callbackToRemove = lobby.FirstOrDefault(callback => callback.Value.Username == user.Username).Key;
                    if (callbackToRemove != null)
                    {
                        lobby.Remove(callbackToRemove);
                    }
                    if (lobby.Count == 0)
                    {
                        _lobbies.Remove(lobbyCode);
                    }
                }
            }
        }
    }

}
