using LabyrinthCommon;
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
    public interface ILobbyManagementService
    {
        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        string CreateLobby(TransferUser lobbyCreator);

        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        List<TransferUser> JoinToGame(string lobbyCode, TransferUser user);

    }
}