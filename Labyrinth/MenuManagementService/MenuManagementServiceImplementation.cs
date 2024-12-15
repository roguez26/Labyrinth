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
        private static readonly ILog _log = LogManager.GetLogger(typeof(MenuManagementServiceImplementation));
        private Dictionary<TransferUser, Dictionary<IMenuManagementServiceCallback, bool>> _players = new Dictionary<TransferUser, Dictionary<IMenuManagementServiceCallback, bool>>();
        public void Start(TransferUser user)
        {
            IMenuManagementServiceCallback callback = OperationContext.Current.GetCallbackChannel<IMenuManagementServiceCallback>();
            if (user == null || callback == null)
            {
                return;
            }
            else
            {
                if (!_players.ContainsKey(user))
                {
                    _players[user] = new Dictionary<IMenuManagementServiceCallback, bool>
                    {
                        { callback, true }
                    };
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
                    callbackDictionary[callback] = true;
                }
            }
        }


        public void ChangeAvailability(TransferUser user, bool availability)
        {
            if (user != null)
            {
                if (_players.ContainsKey(user))
                {
                    var callback = _players[user].Keys.FirstOrDefault();  
                    if (callback != null)
                    {
                        _players[user][callback] = availability;
                    }
                }
                else
                {
                    throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailCallbackNotFoundMessage"));
                }
            }
        }

        public void UpdateCallback(TransferUser user)
        {
            if (user != null)
            {
                IMenuManagementServiceCallback callback = OperationContext.Current.GetCallbackChannel<IMenuManagementServiceCallback>();
                if (callback != null)
                {
                    if (_players.ContainsKey(user))
                    {
                        var currentAvailability = _players[user].FirstOrDefault().Value;

                        _players[user] = new Dictionary<IMenuManagementServiceCallback, bool>
                        {
                            { callback, currentAvailability }
                        };
                    }
                    else
                    {
                        throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("UserNotRegistered"));
                    }
                }
            }

        }
        public void InviteFriend(TransferUser inviter, TransferUser invitee, string lobbyCode)
        {
            if (inviter != null && invitee != null)
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
    }
}
