using LabyrinthCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GameService
{
    [ServiceContract]
    public interface IGameServiceCallback
    {
       
        [OperationContract]
        void ReceiveGameBoard(TransferGameBoard gameBoard);


        [OperationContract]
        void NotifyTurn(TransferPlayer currentPlayer);

        [OperationContract]
        void NotifyPlayerHasJoined(TransferPlayer player);

        [OperationContract]
        void MovePlayerOnTile(string username, string direction);

        [OperationContract]
        void MoveRowOnBoard(string direction, int indexRow, bool toRight);

        [OperationContract]
        void MoveColumnOnBoard(string direction, int indexRow, bool toRight);
        [OperationContract]
        void UpdatePlayerCharacter(string username, string character);
    }
}
