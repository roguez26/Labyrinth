using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ChatService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class ChatServiceImplementation : IChatService
    {
        private static List<IChatServiceCallback> _clients = new List<IChatServiceCallback>();
        public void SendMessage(string message)
        {
            IChatServiceCallback callback = OperationContext.Current.GetCallbackChannel<IChatServiceCallback>();

            if (!_clients.Contains(callback))
            {
                _clients.Add(callback);
            }

            List<IChatServiceCallback> clientsToRemove = new List<IChatServiceCallback>();

            foreach (var client in _clients)
            {
                try
                {
                    if (((ICommunicationObject)client).State == CommunicationState.Opened)
                    {
                        client.BroadcastMessage(message);
                    }
                    else
                    {
                        clientsToRemove.Add(client);
                    }
                }
                catch (Exception exception)
                {
                    clientsToRemove.Add(client);
                }
            }


            foreach (var clientToRemove in clientsToRemove)
            {
                _clients.Remove(clientToRemove);
            }
        }

    }
}
