using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace LobbyManagementService
{
    [ServiceContract]
    public interface ILobbyManagementCallback
    {
        [OperationContract]
        void BroadcastCreated(string message);

        [OperationContract]
        void BroadcastJoined(string userName);
    }
}
