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
        private readonly Dictionary<string, Dictionary<IGameServiceCallback, TransferUser>> _lobbies
            = new Dictionary<string, Dictionary<IGameServiceCallback, TransferUser>>();

        private readonly Dictionary<string, bool> _lobbyStatus = new Dictionary<string, bool>();

        private static readonly ILog _log = LogManager.GetLogger(typeof(GameServiceImplementation));

        public int Start(string lobbyCode, TransferUser lobbyCreator)
        {
            int result = 0;
            IGameServiceCallback callback = OperationContext.Current.GetCallbackChannel<IGameServiceCallback>();

            if (string.IsNullOrEmpty(lobbyCode) || lobbyCreator == null || callback == null)
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(
                    new LabyrinthCommon.LabyrinthException("FailGameError"));
            }

            lock (_lobbies)
            {
                if (!_lobbies.ContainsKey(lobbyCode))
                {
                    _lobbies[lobbyCode] = new Dictionary<IGameServiceCallback, TransferUser>
                {
                    { callback, lobbyCreator }
                };

                    _lobbyStatus[lobbyCode] = false;
                    Console.WriteLine("join" + lobbyCreator.Username);
                    result = 1;
                }
            }
            return result;
        }

        public int JoinToGame(string lobbyCode, TransferUser user)
        {
            int result = 0;

            IGameServiceCallback callback = OperationContext.Current.GetCallbackChannel<IGameServiceCallback>();

            if (callback == null || user == null || string.IsNullOrEmpty(lobbyCode))
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(
                    new LabyrinthCommon.LabyrinthException("FailGameError"));
            }

            lock (_lobbies)
            {
                if (_lobbyStatus.TryGetValue(lobbyCode, out bool gameStarted) && gameStarted)
                {
                    throw new FaultException<LabyrinthCommon.LabyrinthException>(
                        new LabyrinthCommon.LabyrinthException("GameAlreadyStartedError"));
                }

                if (_lobbies.TryGetValue(lobbyCode, out var lobbyMembers))
                {
                    lobbyMembers[callback] = user;
                    Console.WriteLine("join" + user.Username);

                    result = 1;
                }
            }
            return result;
        }

        public int RemoveUserFromGame(string lobbyCode, TransferUser user)
        {
            int result = 0;

            if (string.IsNullOrEmpty(lobbyCode) || user == null)
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(
                    new LabyrinthCommon.LabyrinthException("FailChatError"));
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

        public void StartGame(string lobbyCode)
        {
            lock (_lobbyStatus)
            {
                if (_lobbyStatus.ContainsKey(lobbyCode))
                {
                    _lobbyStatus[lobbyCode] = true; 
                }
                else
                {
                    throw new FaultException<LabyrinthCommon.LabyrinthException>(
                        new LabyrinthCommon.LabyrinthException("LobbyNotFoundError"));
                }
            }
        }

        public bool IsGameStarted(string lobbyCode)
        {
            lock (_lobbyStatus)
            {
                return _lobbyStatus.TryGetValue(lobbyCode, out bool gameStarted) && gameStarted;
            }
        }

        public void ChangeGameStatus(string lobbyCode, bool isStarted)
        {
            lock (_lobbyStatus)
            {
                if (_lobbyStatus.ContainsKey(lobbyCode))
                {
                    _lobbyStatus[lobbyCode] = isStarted; 
                }
                else
                {
                    throw new FaultException<LabyrinthCommon.LabyrinthException>(
                        new LabyrinthCommon.LabyrinthException("LobbyNotFoundError"));
                }
            }
        }

        public void SendGameBoardToLobby(string lobbyCode, TransferGameBoard gameBoard)
        {
            Console.WriteLine("try");
            if (!string.IsNullOrEmpty(lobbyCode) && gameBoard != null)
            {
                if (_lobbies.TryGetValue(lobbyCode, out var lobbyMembers))
                {
                    Console.WriteLine("find");

                    // Obtener el primer jugador
                    var firstPlayer = lobbyMembers.Keys.FirstOrDefault();

                    // Enviar el tablero solo a los demás jugadores, no al primero
                    foreach (var player in lobbyMembers.Keys)
                    {
                        if (player != firstPlayer) // Evitar que el primer jugador reciba el tablero
                        {
                            Console.WriteLine("notified");

                            try
                            {
                                player.ReceiveGameBoard(gameBoard);
                            }
                            catch (Exception ex)
                            {
                                _log.Error($"Error al enviar el tablero al jugador: {ex.Message}");
                                Console.WriteLine("error");

                            }
                        }
                    }
                }
            }
        }
    }

}
