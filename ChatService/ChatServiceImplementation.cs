using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using log4net;
using System.Text;
using System.Threading.Tasks;

namespace ChatService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class ChatServiceImplementation : IChatService
    {
        private static List<IChatServiceCallback> _clients = new List<IChatServiceCallback>();
        private static readonly ILog _log = LogManager.GetLogger(typeof(ChatServiceImplementation));

        public void Start()
        {
            IChatServiceCallback callback = OperationContext.Current.GetCallbackChannel<IChatServiceCallback>();

            if (!_clients.Contains(callback))
            {
                _clients.Add(callback);
            }
        }

        public void SendMessage(string message)
        {
            
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
                    _log.Error("SendMessageError", exception);
                }
            }


            foreach (var clientToRemove in clientsToRemove)
            {
                _clients.Remove(clientToRemove);
            }

            Console.WriteLine(message);
        }

    }
}
