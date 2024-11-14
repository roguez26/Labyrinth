using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;
using LabyrinthCommon;

namespace FriendsManagementService
{
    [ServiceContract]
    public interface IFriendsManagementService
    {
        [OperationContract]
        int SendFriendRequest(int userId, int friendId);

        
    }
}