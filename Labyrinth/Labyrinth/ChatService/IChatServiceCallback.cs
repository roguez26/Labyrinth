using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ChatService
{
    [ServiceContract]
    public interface IChatServiceCallback
    {
        [OperationContract]
        void BroadcastMessage(string message);
    }
}