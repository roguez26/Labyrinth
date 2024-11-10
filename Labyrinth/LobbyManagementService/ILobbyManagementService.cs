using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TransferUser = LabyrinthCommon.TransferUser;

namespace LobbyManagementService
{
    [ServiceContract(CallbackContract = typeof(ILobbyManagementCallback))]
    internal interface ILobbyManagementService
    {
        [OperationContract]
        string CreateLobby(TransferUser lobbyCreator);

        [OperationContract]
        List<TransferUser> JoinToGame(string lobbyCode, TransferUser user);

    }
}
