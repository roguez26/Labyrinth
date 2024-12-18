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

    public interface IMenuManagementService
    {
        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        int Start(TransferUser user);

        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        int ChangeAvailability(TransferUser user, bool availability);

        [OperationContract(IsOneWay = true)]
        void InviteFriend(TransferUser inviter, TransferUser invitee, string lobbyCode);

        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        int UpdateCallback(TransferUser user);

        [OperationContract]
        void DeleteUsers();
    }
}
