using LabyrinthCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GameService
{
    [ServiceContract(CallbackContract = typeof(IGameServiceCallback))]
    public interface IGameService
    {
        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]

        int Start(string lobbyCode, TransferUser lobbyCreator);

        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]

        int JoinToGame(string lobbyCode, TransferUser user);

        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        int RemoveUserFromGame(string lobbyCode, TransferUser user);

        [OperationContract]
        void ChangeGameStatus(string lobbyCode, bool isStarted);

        [OperationContract(IsOneWay = true)]
        void SendGameBoardToLobby(string lobbyCode, TransferGameBoard gameBoard);

        [OperationContract]
        void SelectCharacter(string skinPath);

        [OperationContract(IsOneWay = true)]
        void AsignTurn(string lobbyCode, TransferUser currentUser);

    }
}
