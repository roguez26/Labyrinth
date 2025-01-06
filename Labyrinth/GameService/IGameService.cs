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

        int Start(string lobbyCode, TransferPlayer lobbyCreator);

        [OperationContract(IsOneWay = true)]
        void JoinToGame(string lobbyCode, TransferPlayer user);

        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        int RemoveUserFromGame(string lobbyCode, TransferPlayer user);

        [OperationContract(IsOneWay = true)]
        void SendGameBoardToLobby(string lobbyCode, TransferGameBoard gameBoard);

        [OperationContract(IsOneWay = true)]
        void SelectCharacter(string lobbyCode, string username, string character);

        [OperationContract(IsOneWay = true)]
        void AssignTurn(string lobbyCode, TransferPlayer currentUser);

        [OperationContract(IsOneWay = true)]
        void MovePlayer(string lobbyCode, string username, string direction);

        [OperationContract(IsOneWay = true)]
        void MoveRow(string lobbyCode, string direction, int indexRow, bool toRight);

        [OperationContract(IsOneWay = true)]
        void MoveColumn(string lobbyCode, string direction, int indexRow, bool toRight);
    }
}
