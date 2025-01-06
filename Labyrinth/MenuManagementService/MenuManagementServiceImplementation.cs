using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TransferUser = LabyrinthCommon.TransferUser;
using LabyrinthCommon;
using log4net;

namespace MenuManagementService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]

    public class MenuManagementServiceImplementation : IMenuManagementService
    {
        private readonly Dictionary<TransferUser, Dictionary<IMenuManagementServiceCallback, bool>> _players = new Dictionary<TransferUser, Dictionary<IMenuManagementServiceCallback, bool>>();
        public int Start(TransferUser user)
        {
            int result = 0;
            IMenuManagementServiceCallback callback = OperationContext.Current.GetCallbackChannel<IMenuManagementServiceCallback>();
            if (user == null || callback == null)
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailMenuError"));
            }

            if (!_players.ContainsKey(user))
            {
                _players[user] = new Dictionary<IMenuManagementServiceCallback, bool>
                {
                    { callback, true }
                };
                result = 1;
            }
            else
            {
                var callbackDictionary = _players[user];
                ICommunicationObject communicationObject = callbackDictionary.Keys.FirstOrDefault() as ICommunicationObject;

                if (communicationObject != null && communicationObject.State == CommunicationState.Opened)
                {
                    throw new FaultException<LabyrinthCommon.LabyrinthException>(
                        new LabyrinthCommon.LabyrinthException("FailActiveSessionAlready")
                    );
                } 
                else
                {
                    _players[user] = new Dictionary<IMenuManagementServiceCallback, bool>
                    {
                        { callback, true }
                    };
                }
                result = 1;
            }
            
            return result;
        }


        public int ChangeAvailability(TransferUser user, bool availability)
        {
            int result = 0;
            if (user == null)
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailLobbyError"));
            }

            if (_players.ContainsKey(user))
            {
                var callback = _players[user].Keys.FirstOrDefault();  
                if (callback != null)
                {
                    _players[user][callback] = availability;
                    result = 1;
                }
            }
            else
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailCallbackNotFoundMessage"));
            }
            return result;
        }

        public int UpdateCallback(TransferUser user)
        {
            IMenuManagementServiceCallback callback = OperationContext.Current.GetCallbackChannel<IMenuManagementServiceCallback>();
            int result = 0;
            if (user == null || callback == null)
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailLobbyError"));
            }
          
            if (_players.ContainsKey(user))
            {
                var currentAvailability = _players[user].FirstOrDefault().Value;

                _players[user] = new Dictionary<IMenuManagementServiceCallback, bool>
                {
                    { callback, currentAvailability }
                };
                result = 1;
            }
            else
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("UserNotRegistered"));
            }
            return result;
        }

        public void InviteFriend(TransferUser inviter, TransferUser invitee, string lobbyCode)
        {
            if (inviter != null && invitee != null && !string.IsNullOrEmpty(lobbyCode))
            {
                if (_players.TryGetValue(invitee, out var callback))
                {
                    if (callback?.Keys.Any() == true)
                    {
                        var firstKey = callback.Keys.ElementAt(0);
                        firstKey.AttendInvitation(inviter, lobbyCode);
                    }
                }
            }
        }

        public void DeleteUsers()
        {
            _players.Clear();
        }
    }
}
