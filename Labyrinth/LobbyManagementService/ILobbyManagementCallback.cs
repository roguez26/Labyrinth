using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using TransferUser = LabyrinthCommon.TransferUser;
using LabyrinthCommon;

namespace LobbyManagementService
{
    [ServiceContract]
    public interface ILobbyManagementCallback
    {
        [OperationContract]
        void NotifyUserHasJoined(TransferUser user);

        [OperationContract]
        void NotifyUserHasLeft(TransferUser user);

        [OperationContract]
        void GestMembersList(TransferUser[] members);

        [OperationContract]
        void KickOutPlayer(TransferUser user);
    }
}
