using LabyrinthCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ChatService
{
    [ServiceContract(CallbackContract = typeof(IChatServiceCallback))]
    public interface IChatService
    {
        [OperationContract(IsOneWay = true)]
        void SendMessage(string message, string lobbyCode);

        [OperationContract ]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]

        int Start(string lobbyCode, TransferUser lobbyCreator);

        [OperationContract ]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]

        int JoinToChat(string lobbyCode, TransferUser user);

        [OperationContract ]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        int RemoveUserFromChat(string lobbyCode, TransferUser user);
    }
}
