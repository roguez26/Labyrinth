using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TransferUser = LabyrinthCommon.TransferUser;

namespace MenuManagementService
{
    [ServiceContract(CallbackContract = typeof(IMenuManagementServiceCallback))]

    internal interface IMenuManagementService
    {
        [OperationContract]
        void Start(TransferUser user);

        [OperationContract]
        void End(TransferUser user);

        [OperationContract]
        void InviteFriend(string username, string lobbyCode);
    }
}
