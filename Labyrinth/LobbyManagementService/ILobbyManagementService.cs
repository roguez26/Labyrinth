using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LobbyManagementService
{
    [ServiceContract(CallbackContract = typeof(ILobbyManagementCallback))]
    internal interface ILobbyManagementService
    {
        [OperationContract(IsOneWay = true)]
        void CreateLobby();

        [OperationContract(IsOneWay = true)]
        void JoinToGame(string lobbyCode, string userName);

    }
}
