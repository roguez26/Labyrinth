using LabyrinthCommon;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GameService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class GameServiceImplementation : IGameService
    {
        private readonly Dictionary<string, Dictionary<IGameServiceCallback, TransferPlayer>> _lobbies
            = new Dictionary<string, Dictionary<IGameServiceCallback, TransferPlayer>>();

        private readonly Dictionary<string, bool> _lobbyStatus = new Dictionary<string, bool>();

        private static readonly ILog _log = LogManager.GetLogger(typeof(GameServiceImplementation));

        public int Start(string lobbyCode, TransferPlayer lobbyCreator)
        {
            int result = 0;
            IGameServiceCallback callback = OperationContext.Current.GetCallbackChannel<IGameServiceCallback>();

            if (string.IsNullOrEmpty(lobbyCode) || lobbyCreator == null || callback == null)
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(
                    new LabyrinthCommon.LabyrinthException("FailGameMessage"));
            }

            lock (_lobbies)
            {
                if (!_lobbies.ContainsKey(lobbyCode))
                {
                    _lobbies[lobbyCode] = new Dictionary<IGameServiceCallback, TransferPlayer>
                    {
                        { callback, lobbyCreator }
                    };
                    _lobbyStatus[lobbyCode] = false;
                    result = 1;
                }
            }
            return result;
        }

        public void JoinToGame(string lobbyCode, TransferPlayer user)
        {
            IGameServiceCallback callback = OperationContext.Current.GetCallbackChannel<IGameServiceCallback>();

            if (callback == null || user == null || string.IsNullOrEmpty(lobbyCode))
            {
                return;
            }                

            if (!_lobbies.TryGetValue(lobbyCode, out var lobbyMembers))
            {
                return;
            }

            lobbyMembers[callback] = user;
            foreach (var player in lobbyMembers.Keys)
            {
                if (player is ICommunicationObject communicationObject && communicationObject.State == CommunicationState.Opened)
                {
                    try
                    {
                        player.NotifyPlayerHasJoined(user);
                    }
                    catch (CommunicationException exception)
                    {
                        _log.Error(exception.Message);
                    }
                }
            }
        }


        public int RemoveUserFromGame(string lobbyCode, TransferPlayer user)
        {
            int result = 0;

            if (string.IsNullOrEmpty(lobbyCode) || user == null)
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(
                    new LabyrinthCommon.LabyrinthException("FailGameMessage"));
            }

            lock (_lobbies)
            {
                if (_lobbies.TryGetValue(lobbyCode, out var lobby))
                {
                    var callbackToRemove = lobby.FirstOrDefault(callback => callback.Value.Username == user.Username).Key;
                    if (callbackToRemove != null)
                    {
                        lobby.Remove(callbackToRemove);
                        if (lobby.Count == 0)
                        {
                            _lobbies.Remove(lobbyCode);
                            _lobbyStatus.Remove(lobbyCode); 
                        }
                        result = 1;
                    }
                }
            }
            return result;
        }

        public void SendGameBoardToLobby(string lobbyCode, TransferGameBoard gameBoard)
        {
            if (!string.IsNullOrEmpty(lobbyCode) && gameBoard != null)
            {
                if (_lobbies.TryGetValue(lobbyCode, out var lobbyMembers))
                {
                    var firstPlayer = lobbyMembers.Keys.FirstOrDefault();

                    foreach (var player in lobbyMembers.Keys)
                    {
                        if (player is ICommunicationObject communicationObject && communicationObject.State == CommunicationState.Opened && player != firstPlayer)
                        {
                            try
                            {
                                player.ReceiveGameBoard(gameBoard);
                            }
                            catch (CommunicationException exception)
                            {
                                _log.Error(exception.Message);
                            }
                        }
                    }
                }
            }
        }

        public void SelectCharacter(string lobbyCode, string username,string character)
        {
            if (!string.IsNullOrEmpty(lobbyCode) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(character)) 
            {

                if(_lobbies.TryGetValue(lobbyCode, out var lobbyMembers))
                {
                    foreach (var player in lobbyMembers.Keys)
                    {
                        if (player is ICommunicationObject communicationObject && communicationObject.State == CommunicationState.Opened)
                        {
                            try
                            {
                                player.UpdatePlayerCharacter(username, character);
                            }
                            catch (CommunicationException exception)
                            {
                                _log.Error(exception.Message);
                            }
                        }
                    }
                }
            }
        }

        public void AssignTurn(string lobbyCode, TransferPlayer currentUser)
        {
            if (_lobbies.ContainsKey(lobbyCode))
            {
                var lobby = _lobbies[lobbyCode];
                var users = lobby.Values.ToList();

                int currentIndex = users.FindIndex(user => user.Username == currentUser.Username);
                if (currentIndex != -1)
                {
                    int usersCount = users.Count;
                    int attempts = 0;
                    do
                    {
                        int nextIndex = (currentIndex + 1) % usersCount;
                        TransferPlayer nextUser = users[nextIndex];

                        var userCallbackPair = lobby.FirstOrDefault(member => member.Value == nextUser);
                        IGameServiceCallback userCallback = userCallbackPair.Key;

                        if (userCallback is ICommunicationObject communicationObject && communicationObject.State == CommunicationState.Opened)
                        {
                            userCallback.NotifyTurn(nextUser);
                            return; 
                        }

                        currentIndex = nextIndex;
                        attempts++;
                    } while (attempts < usersCount);
                }
            }
        }

        public void MovePlayer(string lobbyCode, string username, string direction)
        {
            if (!string.IsNullOrEmpty(lobbyCode) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(direction))
            {
                if (_lobbies.TryGetValue(lobbyCode, out var lobbyMembers))
                {
                    foreach (var playerCallback in lobbyMembers.Keys)
                    {
                        if (playerCallback is ICommunicationObject communicationObject && communicationObject.State == CommunicationState.Opened)
                        {
                            try
                            {
                                playerCallback.MovePlayerOnTile(username, direction);
                            }
                            catch (CommunicationException ex)
                            {
                                _log.Error(ex.Message);
                            }
                        }
                    }
                }
            }
        }


        public void MoveRow(string lobbyCode, string direction, int indexRow, bool toRight) 
        {
            if (!string.IsNullOrEmpty(lobbyCode) && !string.IsNullOrEmpty(direction) && indexRow >= 0)
            {
                if (_lobbies.TryGetValue(lobbyCode, out var lobbyMembers))
                {
                    foreach (var player in lobbyMembers.Keys)
                    {
                        if (player is ICommunicationObject communicationObject &&communicationObject.State == CommunicationState.Opened)
                        {
                            try
                            {
                                player.MoveRowOnBoard(direction, indexRow, toRight);
                            }
                            catch (CommunicationException ex)
                            {
                                _log.Error(ex.Message);
                            }
                        }
                    }
                }
            }
        }

        public void MoveColumn(string lobbyCode, string direction, int indexRow, bool toRight)
        {
            if (!string.IsNullOrEmpty(lobbyCode) && !string.IsNullOrEmpty(direction) && indexRow >= 0)
            {
                if (_lobbies.TryGetValue(lobbyCode, out var lobbyMembers))
                {
                    foreach (var player in lobbyMembers.Keys)
                    {
                        if (player is ICommunicationObject communicationObject && communicationObject.State == CommunicationState.Opened)
                        {
                            try
                            {
                                player.MoveColumnOnBoard(direction, indexRow, toRight);
                            }
                            catch (CommunicationException ex)
                            {
                                _log.Error(ex.Message);
                            }
                        }
                    }
                }
            }

        }
    }

}
