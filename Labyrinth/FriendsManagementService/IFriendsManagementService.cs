using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;
using LabyrinthCommon;
using System.Security.Cryptography;

namespace FriendsManagementService
{
    [ServiceContract]
    public interface IFriendsManagementService
    {
        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        int SendFriendRequest(int userId, int friendId);
        [OperationContract]
        TransferUser[] GetMyFriendsList(int idUser);

        [OperationContract]
        TransferFriendRequest[] GetFriendRequestsList(int idUser);

        [OperationContract]
        bool IsFriend(int userId, int friendId);

        [OperationContract]
        int AttendFriendRequest(int friendRequestId, LabyrinthCommon.FriendRequestStatus status);
    }

}