using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TransferUser = LabyrinthCommon.TransferUser;

namespace MenuManagementService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]

    public class MenuManagementServiceImplementation : IMenuManagementService
    {
        private static Dictionary<IMenuManagementServiceCallback, TransferUser> _players = new Dictionary<IMenuManagementServiceCallback, TransferUser>();
        public void Start(TransferUser user)
        {
            if (user != null)
            {
                IMenuManagementServiceCallback callback = OperationContext.Current.GetCallbackChannel<IMenuManagementServiceCallback>();
                if (callback != null)
                {
                    if (!_players.ContainsKey(callback))
                    {
                        _players.Add(callback, user);
                    }
                }
            }
        }

        public void End(TransferUser user)
        {
            if (user != null)
            {
                IMenuManagementServiceCallback callback = OperationContext.Current.GetCallbackChannel<IMenuManagementServiceCallback>();
                if (callback != null)
                {
                    if (_players.ContainsKey(callback))
                    {
                        _players.Remove(callback);
                    }
                }
            }
        }

        public void InviteFriend(string username, string lobbyCode)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("El nombre de usuario no puede estar vacío.", nameof(username));
            }

            if (string.IsNullOrWhiteSpace(lobbyCode))
            {
                throw new ArgumentException("El código de lobby no puede estar vacío.", nameof(lobbyCode));
            }

            foreach (var entry in _players)
            {
                TransferUser user = entry.Value;
                if (user.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
                {
                    IMenuManagementServiceCallback callback = entry.Key;

                    if (callback != null)
                    {
                        callback.AttendInvitation(lobbyCode);
                        Console.WriteLine($"Invitación enviada a {username} para el lobby {lobbyCode}.");
                        return;
                    }
                }
            }

            Console.WriteLine($"El usuario {username} no fue encontrado o no tiene un callback asociado.");
        }


    }
}
