using TransferUser = LabyrinthCommon.TransferUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace LobbyManagementService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class LobbyManagementServiceImplementation : ILobbyManagementService
    {
<<<<<<< HEAD
        private Dictionary<string, List<ILobbyManagementCallback>> _lobbies = new Dictionary<string, List<ILobbyManagementCallback>>();
        public void CreateLobby()
=======
        private Dictionary<string, Dictionary<ILobbyManagementCallback, TransferUser>> lobbies = new Dictionary<string, Dictionary<ILobbyManagementCallback, TransferUser>>();
        public string CreateLobby(TransferUser lobbyCreator)
>>>>>>> 786b1c85b3e846d2eb8fa5adc8a7f8dcbcf25ea2
        {
            ILobbyManagementCallback callback = OperationContext.Current.GetCallbackChannel<ILobbyManagementCallback>();
            string lobbyCode = GenerateLobbyCode();

            while (_lobbies.ContainsKey(lobbyCode))
            {
                lobbyCode = GenerateLobbyCode();
            }

<<<<<<< HEAD
            _lobbies[lobbyCode] = new List<ILobbyManagementCallback>();
            _lobbies[lobbyCode].Add(callback);
            Console.WriteLine(lobbyCode);
            callback.BroadcastCreated(lobbyCode);
        }

        public void JoinToGame(string lobbyCode, string userName)
=======
            var lobbyMembers = new Dictionary<ILobbyManagementCallback, TransferUser>();
            lobbyMembers.Add(callback, lobbyCreator);

            lobbies[lobbyCode] = lobbyMembers;
           
            return lobbyCode;
        }

        public List<TransferUser> JoinToGame(string lobbyCode, TransferUser user)
>>>>>>> 786b1c85b3e846d2eb8fa5adc8a7f8dcbcf25ea2
        {
            List<TransferUser>  members = new List<TransferUser>();
            ILobbyManagementCallback callback = OperationContext.Current.GetCallbackChannel<ILobbyManagementCallback>();

<<<<<<< HEAD
            if (_lobbies.ContainsKey(lobbyCode))
            {
                _lobbies[lobbyCode].Add (callback);
                List<ILobbyManagementCallback> lobbyMembers = _lobbies[lobbyCode];
=======
            if (callback == null)
            {
                //Lanzar excepcion
            }
>>>>>>> 786b1c85b3e846d2eb8fa5adc8a7f8dcbcf25ea2

            if (lobbies.TryGetValue(lobbyCode, out var lobbyMembers))
            {
                if (lobbyMembers.ContainsKey(callback))
                {
                    //Lanzar excepcion
                }

                lobbyMembers[callback] = user;
                members.Add(user);
                foreach (var member in lobbyMembers)
                {
                    // `member.Key` es el callback asociado a cada miembro, y `member.Value` es el usuario
                    if (!member.Value.Equals(user))
                    {
                        // Notificar a los demás usuarios
                        member.Key.NotifyUserHasJoined(user);
                        members.Add(member.Value);
                    }
                    
                }
            }
            else
            {
                //Lanzar excepcion
            }
            return members;
        }

        public static string GenerateLobbyCode()
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < 3; i++)
            {
                if (i > 0)
                {     
                    stringBuilder.Append('-');
                }

                for (int j = 0; j < 3; j++)
                {
                    stringBuilder.Append(chars[random.Next(chars.Length)]);
                }
            }

            return stringBuilder.ToString();
        }

    }
}
