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
<<<<<<< HEAD
        [OperationContract(IsOneWay = true)]
        void CreateLobby();

        [OperationContract(IsOneWay = true)]
        void JoinToGame(string lobbyCode, string userName);
=======
        [OperationContract]
        string CreateLobby(TransferUser lobbyCreator);

        [OperationContract]
        List<TransferUser> JoinToGame(string lobbyCode, TransferUser user);
>>>>>>> 786b1c85b3e846d2eb8fa5adc8a7f8dcbcf25ea2

    }
}
