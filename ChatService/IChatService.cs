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

        [OperationContract (IsOneWay = true)]
        void Start(string lobbyCode, TransferUser lobbyCreator);
        [OperationContract (IsOneWay = true)]
        void JoinToChat(string lobbyCode, TransferUser user);

        [OperationContract (IsOneWay = true)]
        void RemoveUserFromChat(string lobbyCode, TransferUser user);
    }
}
