using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using log4net;
using TransferUser = LabyrinthCommon.TransferUser;

namespace ChatService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class ChatServiceImplementation : IChatService
    {
        private readonly Dictionary<string, Dictionary<IChatServiceCallback, TransferUser>> _lobbies = new Dictionary<string, Dictionary<IChatServiceCallback, TransferUser>>();

        public int Start(string lobbyCode, TransferUser lobbyCreator)
        {
            int result = 0;
            IChatServiceCallback callback = OperationContext.Current.GetCallbackChannel<IChatServiceCallback>();
            if (string.IsNullOrEmpty(lobbyCode) || lobbyCreator == null || callback == null)
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailChatError"));
            }
            lock (_lobbies)
            {
                if (!_lobbies.ContainsKey(lobbyCode))
                {
                    _lobbies[lobbyCode] = new Dictionary<IChatServiceCallback, TransferUser>
                    {
                        { callback, lobbyCreator }
                    };
                    result = 1;
                }
            }
            return result;
        }

        public int JoinToChat(string lobbyCode, TransferUser user)
        {
            int result = 0;

            IChatServiceCallback callback = OperationContext.Current.GetCallbackChannel<IChatServiceCallback>();
            if (callback == null || user == null || string.IsNullOrEmpty(lobbyCode))
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailChatError"));
            }

            lock (_lobbies)
            {
                if (_lobbies.TryGetValue(lobbyCode, out var lobbyMembers))
                {
                    lobbyMembers[callback] = user;
                    result = 1;
                }
            }
            return result;
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
                        var callbackDictionary = member;
                        ICommunicationObject communicationObject = callbackDictionary.Key as ICommunicationObject;

                        if (communicationObject != null && communicationObject.State == CommunicationState.Opened)
                        {
                            member.Key.BroadcastMessage(message);
                        }
                    }
                }
            }
        }

        public int RemoveUserFromChat(string lobbyCode, TransferUser user)
        {
            int result = 0;

            if (string.IsNullOrEmpty(lobbyCode) || user == null)
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailChatError"));
            }
            if (_lobbies.TryGetValue(lobbyCode, out var lobby))
            {
                var callbackToRemove = lobby.FirstOrDefault(callback => callback.Value.Username == user.Username).Key;
                if (callbackToRemove != null)
                {
                    lobby.Remove(callbackToRemove);
                    if (lobby.Count == 0)
                    {
                        _lobbies.Remove(lobbyCode);
                    }
                    result = 1;
                }
            }
            return result;
        }
    }

}
