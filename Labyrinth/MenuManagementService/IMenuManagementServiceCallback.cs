using LabyrinthCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MenuManagementService
{
    [ServiceContract]

    internal interface IMenuManagementServiceCallback
    {
        [OperationContract]
        void AttendInvitation(string lobbyCode);
    }
}
